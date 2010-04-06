using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;

namespace Nibbler
{
    public partial class LoaderForm : Form
    {
        ExtensionConfiguration _extConf;
        PathFinder _pathFinder;
        Uri _fileUri;
        readonly string _fileLink;

        public LoaderForm(string fileLink)
        {
            _fileLink = fileLink;
            InitializeComponent();
        }

        private void LoaderForm_Load(object sender, EventArgs e)
        {
            // ==========================================================
            // Reading XML Extension Config
            // ==========================================================
            _extConf = new ExtensionConfiguration();
            _extConf.ReadConfig();

            // ==========================================================
            // Find Pathes for Game Folders etc.
            // ==========================================================
            _pathFinder = new PathFinder();
            _pathFinder.GatherPathes();

            // ==========================================================
            // Start Download
            // ==========================================================
            WebRequest wrGeturl = WebRequest.Create(_fileLink);

            var resp = (HttpWebResponse)wrGeturl.GetResponse();
            int statusCode = (int)resp.StatusCode;

            // ==========================================================
            // Check Status Code
            // ==========================================================
            if (statusCode == 200)
            {
                // Construct URI
                _fileUri = resp.ResponseUri;
                // ==========================================================
                // Check Extension
                // ==========================================================
                string fileName = _fileUri.Segments[_fileUri.Segments.Length - 1].Replace('+', ' ');
                string extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                {
                    MessageBox.Show("The link you followed cannot be handled by Nibbler.", "Nibbler Error", MessageBoxButtons.OK);
                    Application.Exit();
                }

                extension = extension.ToLower().Substring(1);

                if (_extConf.IsValidExtension(extension))
                {
                    lblFilename.Text = fileName; // Display filename

                    //ask user if they want to download file
                    if (MessageBox.Show(string.Format("Do you want to download {0}?",fileName), "Confirm download", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // a 'DialogResult.Yes' value was returned from the MessageBox
                        bgWorker.RunWorkerAsync();
                    }
                    else
                    {
                        Application.Exit();
                    }

                }
                
                else
                {
                    // No action for this Extension defined
                    // Either link is broken or someone tampered with the XML

                    // Quit application, maybe should do something better, like pop-up a message, but
                    // that would be annoying too.. so lets just exit here...
                    // or not. waiting for program to do nothing then quit is stupid
                    // show user a popup!
                    MessageBox.Show("Nibbler does not handle files of type " + extension.ToUpper() + ".", "Nibbler Error", MessageBoxButtons.OK);
                    Application.Exit(); 
                }
            }
        }

        // ==========================================================
        // Downloads the File
        // ==========================================================
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // the URL to download the file from
            string sUrlToReadFileFrom = _fileUri.OriginalString;
            // the path to write the file to
            string fileName = _fileUri.Segments[_fileUri.Segments.Length - 1].Replace('+', ' ');
            string sFilePathToWriteFileTo = Path.GetTempPath() + "/" + fileName;

            // first, we need to get the exact size (in bytes) of the file we are downloading
            var url = new Uri(sUrlToReadFileFrom);
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            response.Close();
            // gets the size of the file in bytes
            long iSize = response.ContentLength;

            // keeps track of the total bytes downloaded so we can update the progress bar
            long iRunningByteTotal = 0;

            // use the webclient object to download the file
            using (var client = new WebClient())
            {
                // open the file at the remote URL for reading
                using (var streamRemote = client.OpenRead(new Uri(sUrlToReadFileFrom)))
                {
                    // using the FileStream object, we can write the downloaded bytes to the file system
                    using (var streamLocal = new FileStream(sFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // loop the stream and get the file into the byte buffer
                        int iByteSize = 0;
                        byte[] byteBuffer = new byte[iSize];
                        while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                        {
                            // write the bytes to the file system at the file path specified
                            streamLocal.Write(byteBuffer, 0, iByteSize);
                            iRunningByteTotal += iByteSize;

                            // calculate the progress out of a base "100"
                            double dIndex = iRunningByteTotal;
                            double dTotal = byteBuffer.Length;
                            double dProgressPercentage = (dIndex / dTotal);
                            int iProgressPercentage = (int)(dProgressPercentage * 100);

                            // update the progress bar
                            bgWorker.ReportProgress(iProgressPercentage);
                        }

                        // clean up the file stream
                        streamLocal.Close();
                    }

                    // close the connection to the remote server
                    streamRemote.Close();
                }
            }
        }


        // ==========================================================
        // Update ProgressBar
        // ==========================================================
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbDownload.Value = e.ProgressPercentage;
        }

        // ==========================================================
        // Download completed, Move File
        // ==========================================================
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string fileName = _fileUri.Segments[_fileUri.Segments.Length - 1].Replace('+', ' ');
            string extension = Path.GetExtension(fileName).ToLower().Substring(1);
            
            // Move
            string moveTo = _extConf.GetCopyPathForExtension(extension);

            // Construct Path
            while (moveTo.IndexOf("%") != -1)
            {
                int pos1 = moveTo.IndexOf("%");
                int pos2 = moveTo.IndexOf("%",pos1+1);
                 
                // something is wrong with our XML, save the file somwhere else
                if (pos2 == -1) {
                    moveTo = "";
                    break;
                }
                
                string pathIdentfier = moveTo.Substring(pos1, pos2 - pos1 + 1);
                
                // ==============================================
                // Translate our PathIdentifier
                // to the real Path
                // ==============================================
                // 1. Check if PathFinder know our Path
                string realPath = _pathFinder.GetPath(pathIdentfier);

                

                // 3. Still no path, query the user for setup
                if (string.IsNullOrEmpty(realPath))
                {
                    if (MessageBox.Show("Missing installation path for " + fileName + ". Would you like configure Nibbler now?", "No path found!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // open setup if user say yes
                        Thread setup = new Thread(ThreadProc);
                        setup.SetApartmentState(ApartmentState.STA);
                        setup.Start();
                        setup.Join();

                        realPath = _pathFinder.GetPath(pathIdentfier);
                    }
                }

                // 4. Still no path?? Okay lets just save the file somewhere else
                if (string.IsNullOrEmpty(realPath))
                {
                    moveTo = "";
                }

                moveTo = moveTo.Replace(pathIdentfier, realPath);
            }

            if (string.IsNullOrEmpty(moveTo))
            {
                var fileDialog = new SaveFileDialog {FileName = fileName};
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    moveTo = fileDialog.FileName.Replace("\\" + fileName, "");
                }
            }

            if (!string.IsNullOrEmpty(moveTo))
            {
                if (!Directory.Exists(moveTo))
                {
                    if (MessageBox.Show("Your save path is set to " + moveTo + ", but that directory does not exist. Would you like to create it now?", "Directory Not Found", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(moveTo);
                    }
                    else
                    {
                        var fileDialog = new SaveFileDialog { FileName = fileName };
                        if (fileDialog.ShowDialog() == DialogResult.OK)
                        {
                            moveTo = fileDialog.FileName.Replace("\\" + fileName, "");
                        }
                    }
                }
                if (!File.Exists(moveTo + "/" + fileName))
                {
                    // If the file does not excist. Copy it to the correct folder
                    File.Copy(Path.GetTempPath() + "/" + fileName, moveTo + "/" + fileName, false);
                }

                else
                {

                    // If file excist, ask for overwrite.
                    if (MessageBox.Show("Do you want to overwrite " + fileName, "File already excist!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // a 'DialogResult.Yes' value was returned from the MessageBox
                        File.Copy(Path.GetTempPath() + "/" + fileName, moveTo + "/" + fileName, true);
                    }
                }
            }

            // be gone!
            Application.Exit();
        }

        public static void ThreadProc()
        {
            Application.Run(new LoaderConfig());
        }
    }
}
