using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Threading;

namespace Nibbler
{
    public partial class LoaderForm : Form
    {
        ExtensionConfiguration extConf;
        PathFinder pathFinder;
        Uri fileUri;
        string fileLink;

        public LoaderForm(string fileLink)
        {
            this.fileLink = fileLink;
            InitializeComponent(); 
        }

        private void LoaderForm_Load(object sender, EventArgs e)
        {
            // ==========================================================
            // Reading XML Extension Config
            // ==========================================================
            extConf = new ExtensionConfiguration();
            extConf.ReadConfig();

            // ==========================================================
            // Find Pathes for Game Folders etc.
            // ==========================================================
            pathFinder = new PathFinder();
            pathFinder.GatherPathes();


            // ==========================================================
            // Start Download
            // ==========================================================
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(this.fileLink);

            HttpWebResponse resp = (HttpWebResponse)wrGETURL.GetResponse();
            int statusCode = (int)resp.StatusCode;

            // ==========================================================
            // Check Status Code
            // ==========================================================
            if (statusCode == 200)
            {
                // Construct URI
                fileUri = resp.ResponseUri;
                // ==========================================================
                // Check Extension
                // ==========================================================
                string fileName = fileUri.Segments[fileUri.Segments.Length - 1];
                string extension = Path.GetExtension(fileName).ToLower().Substring(1);

                if (extConf.IsValidExtension(extension))
                {
                    lblFilename.Text = fileName; // Display filename

                    //ask user if they want to download file
                    if (MessageBox.Show("Do you want to download " + fileName, "Confirm download", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                    MessageBox.Show("Error!", "Timeout!", MessageBoxButtons.OK);
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
            string sUrlToReadFileFrom = fileUri.OriginalString;
            // the path to write the file to
            string fileName = fileUri.Segments[fileUri.Segments.Length - 1];
            string sFilePathToWriteFileTo = Path.GetTempPath() + "/" + fileName;

            // first, we need to get the exact size (in bytes) of the file we are downloading
            Uri url = new Uri(sUrlToReadFileFrom);
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            response.Close();
            // gets the size of the file in bytes
            Int64 iSize = response.ContentLength;

            // keeps track of the total bytes downloaded so we can update the progress bar
            Int64 iRunningByteTotal = 0;

            // use the webclient object to download the file
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                // open the file at the remote URL for reading
                using (System.IO.Stream streamRemote = client.OpenRead(new Uri(sUrlToReadFileFrom)))
                {
                    // using the FileStream object, we can write the downloaded bytes to the file system
                    using (Stream streamLocal = new FileStream(sFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.None))
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
                            double dIndex = (double)(iRunningByteTotal);
                            double dTotal = (double)byteBuffer.Length;
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
            string fileName = fileUri.Segments[fileUri.Segments.Length - 1];
            string extension = Path.GetExtension(fileName).ToLower().Substring(1);
            
            // Move
            string moveTo = extConf.GetCopyPathForExtension(extension);

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
                string realPath = pathFinder.GetPath(pathIdentfier);

                // 3. Still no path, query the user for setup
                if (string.IsNullOrEmpty(realPath))
                {
                    if (MessageBox.Show("No path was found to save " + fileName + " to. Do you want to go to setup?", "No path found!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        // open setup if user say yes
                        System.Threading.Thread setup = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadProc));
                        setup.SetApartmentState(ApartmentState.STA);
                        setup.Start();
                        setup.Join();
                    }

                    realPath = pathFinder.GetPath(pathIdentfier);
                }

                // 4. Still no path?? Okay lets just save the file somewhere else
                if (string.IsNullOrEmpty(realPath))
                {
                    moveTo = "";
                    break;
                }

                moveTo = moveTo.Replace(pathIdentfier, realPath);
            }


            if (!string.IsNullOrEmpty(moveTo))
            {
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
