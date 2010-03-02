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

namespace MapLoader
{
    public partial class Form1 : Form
    {
        ExtensionConfiguration extConf;
        PathFinder pathFinder;
        Uri fileUri;

        public Form1()
        {
            InitializeComponent(); 
        }

        private void Form1_Load(object sender, EventArgs e)
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
            try
            {

                string link = Program.NIBBITS_LINK.Replace("nibbits://", "http://");

                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(link);

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
                    if (extConf.dictExtConfiguration.ContainsKey(extension))
                    {
                        lblFilename.Text = fileName; // Display filename
                        bgWorker.RunWorkerAsync();
                    }
                }

            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.ToString());
                Application.Exit();
            }; 
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
            if (extConf.dictExtConfiguration.ContainsKey(extension))
            {

                string moveTo = extConf.dictExtConfiguration[extension].dictActionsPerOS["default"];

                if (!File.Exists(moveTo + "/" + fileName)) 
                    File.Copy(Path.GetTempPath() + "/" + fileName, moveTo + "/" + fileName, false);
            }

            // be gone!
            Application.Exit();
        }
    }
}
