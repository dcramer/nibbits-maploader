using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using Utility.ModifyRegistry;

namespace Nibbler
{
    /// <summary>
    /// This class trys to automatically find pathes to game folders etc and
    /// keeps track of them.
    /// </summary>
    class PathFinder
    {

        public Dictionary<string, string> DictPathIdentifiers;

        public PathFinder()
        {
            DictPathIdentifiers = new Dictionary<string, string>();
        }

        /// <summary>
        /// Tries to find game pathes etc. in the registry or other places
        /// and stores them in a Dictionary.
        /// Must be called before pathes are accessed through GetPath.
        /// </summary>
        public void GatherPathes()
        {
            // =======================================
            // Misc
            // =======================================
            string pathToProgramFiles = Environment.GetEnvironmentVariable("ProgramFiles");
            string pathToTempFiles    = Path.GetTempPath();
            string pathToAppData      = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string pathToAppDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string pathToDocuments    = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            DictPathIdentifiers.Add("%PROGRAM_FILES%", pathToProgramFiles);
            DictPathIdentifiers.Add("%TEMP%", pathToTempFiles);
            DictPathIdentifiers.Add("%APP_DATA%", pathToAppData);
            DictPathIdentifiers.Add("%APP_DATA_LOCAL%", pathToAppDataLocal);
            DictPathIdentifiers.Add("%DOCUMENTS%", pathToDocuments);

            // =======================================
            // STARCRAFT II
            // =======================================
            // Install Path + add modifyregistry

            if (string.IsNullOrEmpty(Properties.Client.Default.SC2_INSTALL_PATH))
            {
                var mysc2Registry = new ModifyRegistry
                                        {
                                            SubKey = "Software\\Blizzard Entertainment\\Starcraft II\\",
                                            ShowError = true
                                        };

                // 1a. Try Registry LocalMachine
                string pathToStarcraft2Folder = mysc2Registry.Read("InstallPath");

                // 1b. Try Registry CurrentUser
                if (string.IsNullOrEmpty(pathToStarcraft2Folder))
                {
                    mysc2Registry.BaseRegistryKey = Registry.CurrentUser;
                    pathToStarcraft2Folder = mysc2Registry.Read("InstallPath");
                }

                if (string.IsNullOrEmpty(pathToStarcraft2Folder))
                {
                    pathToStarcraft2Folder = "";
                }

                // 2. Try Default Path
                if (string.IsNullOrEmpty(pathToStarcraft2Folder))
                {
                    string sc2DefaultPath = pathToProgramFiles + "/" + "StarCraft II/";
                    if (File.Exists(pathToProgramFiles + "StarCraft II.exe")) pathToStarcraft2Folder = sc2DefaultPath;
                }

                // 3. Try Default Path (Beta)
                if (string.IsNullOrEmpty(pathToStarcraft2Folder))
                {
                    string defaultPath = pathToProgramFiles + "/" + "StarCraft II Beta/";
                    if (File.Exists(pathToProgramFiles + "StarCraft II.exe")) pathToStarcraft2Folder = defaultPath;
                }

                // 4. Default to empty path
                if (string.IsNullOrEmpty(pathToStarcraft2Folder))
                {
                    pathToStarcraft2Folder = "";
                }

                Properties.Client.Default.SC2_INSTALL_PATH = pathToStarcraft2Folder;
            }
            DictPathIdentifiers.Add("%SC2_INSTALL_PATH%", Properties.Client.Default.SC2_INSTALL_PATH);
            

            // =======================================
            // STARCRAFT I
            // =======================================

            if (string.IsNullOrEmpty(Properties.Client.Default.SC1_INSTALL_PATH))
            {
                var mysc1Registry = new ModifyRegistry
                                        {
                                            SubKey = "Software\\Blizzard Entertainment\\Starcraft\\",
                                            ShowError = true
                                        };

                // 1a. Try Registry LocalMachine (no idea if localmachine is used, dont think so, but i let it stay)
                string pathToStarcraft1Folder = mysc1Registry.Read("InstallPath");

                // 1b. Try Registry CurrentUser
                if (string.IsNullOrEmpty(pathToStarcraft1Folder))
                {
                    mysc1Registry.BaseRegistryKey = Registry.CurrentUser;
                    pathToStarcraft1Folder = mysc1Registry.Read("InstallPath");
                }

                if (string.IsNullOrEmpty(pathToStarcraft1Folder))
                {
                    pathToStarcraft1Folder = "";
                }

                // 2. Try Default Path
                if (string.IsNullOrEmpty(pathToStarcraft1Folder))
                {
                    string sc1DefaultPath = pathToProgramFiles + "/" + "StarCraft/";

                    if (File.Exists(pathToProgramFiles + "StarCraft.exe")) pathToStarcraft1Folder = sc1DefaultPath;
                }

                // 3. Default to empty path
                if (string.IsNullOrEmpty(pathToStarcraft1Folder))
                {
                    pathToStarcraft1Folder = "";
                }
                Properties.Client.Default.SC1_INSTALL_PATH = pathToStarcraft1Folder;
            }
            DictPathIdentifiers.Add("%SC1_INSTALL_PATH%", Properties.Client.Default.SC1_INSTALL_PATH);
            

            //=====================
            //Warcraft 3
            //=====================

            if (string.IsNullOrEmpty(Properties.Client.Default.WC3_INSTALL_PATH))
            {
                var mywc3Registry = new ModifyRegistry
                                        {
                                            SubKey = "Software\\Blizzard Entertainment\\Warcraft III\\",
                                            ShowError = true
                                        };

                // 1a. Try Registry LocalMachine (no idea if localmachine is used, dont think so, but i let it stay)
                string pathToWarcraft3Folder = mywc3Registry.Read("InstallPath");

                // 1b. Try Registry CurrentUser
                if (string.IsNullOrEmpty(pathToWarcraft3Folder))
                {
                    mywc3Registry.BaseRegistryKey = Registry.CurrentUser;
                    pathToWarcraft3Folder = mywc3Registry.Read("InstallPath");
                }

                if (string.IsNullOrEmpty(pathToWarcraft3Folder))
                {
                    pathToWarcraft3Folder = "";
                }

                Properties.Client.Default.WC3_INSTALL_PATH = pathToWarcraft3Folder;
            }
            DictPathIdentifiers.Add("%WC3_INSTALL_PATH%", Properties.Client.Default.WC3_INSTALL_PATH);

            // TODO: this isnt always nescesary
            Properties.Client.Default.Save();
        }

        
        public string GetPath(string pathIdentifier)
        {
            if (DictPathIdentifiers.ContainsKey(pathIdentifier))
            {
                string path = DictPathIdentifiers[pathIdentifier];
                return path;
            }
            else
                return "";
        }

        public string QueryUserForPath(LoaderConfig form, string pathIdentifier)
        {
            string returnValue = "";
            switch (pathIdentifier)
            {
                case "%SC1_INSTALL_PATH%":
                    form.openFileDialog1.CheckFileExists = true;
                    form.openFileDialog1.Filter = "Starcraft.exe|Starcraft.exe";
                    form.openFileDialog1.FileName = "Starcraft.exe";
                    if (form.openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        returnValue = form.openFileDialog1.FileName;
                        returnValue = returnValue.ToLower().Replace("starcraft.exe", "");
                        Properties.Client.Default.SC1_INSTALL_PATH = returnValue;
                        Properties.Client.Default.Save();
                        DictPathIdentifiers.Remove(pathIdentifier);
                        DictPathIdentifiers.Add(pathIdentifier, returnValue);
                    }
                    break;

                case "%WC3_INSTALL_PATH%":
                    form.openFileDialog1.CheckFileExists = true;
                    form.openFileDialog1.Filter = "Warcraft III.exe|Warcraft III.exe";
                    form.openFileDialog1.FileName = "Warcraft III.exe";
                    if (form.openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        returnValue = form.openFileDialog1.FileName;
                        returnValue = returnValue.ToLower().Replace("warcraft iii.exe", "");
                        Properties.Client.Default.WC3_INSTALL_PATH = returnValue;
                        Properties.Client.Default.Save();
                        DictPathIdentifiers.Remove(pathIdentifier);
                        DictPathIdentifiers.Add(pathIdentifier, returnValue);
                    }
                    break;

                case "%SC2_INSTALL_PATH%":
                    form.openFileDialog1.CheckFileExists = true;
                    form.openFileDialog1.Filter = "Starcraft II.exe|Starcraft II.exe";
                    form.openFileDialog1.FileName = "Starcraft II.exe";
                    if (form.openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        returnValue = form.openFileDialog1.FileName;
                        returnValue = returnValue.ToLower().Replace("starcraft ii.exe", "");
                        Properties.Client.Default.SC2_INSTALL_PATH = returnValue;
                        Properties.Client.Default.Save();
                        DictPathIdentifiers.Remove(pathIdentifier);
                        DictPathIdentifiers.Add(pathIdentifier, returnValue);
                    }
                    break;
            }
            return returnValue;
        }
    }
}
