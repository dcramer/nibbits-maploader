using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Xml;
using System.Windows.Forms;
using Utility.ModifyRegistry;

namespace MapLoader
{
    /// <summary>
    /// This class trys to automatically find pathes to game folders etc and
    /// keeps track of them.
    /// </summary>
    class PathFinder
    {

        public Dictionary<string, string> dictPathIdentifiers;

        public PathFinder()
        {
            dictPathIdentifiers = new Dictionary<string, string>();
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

            dictPathIdentifiers.Add("%PROGRAM_FILES%", pathToProgramFiles);
            dictPathIdentifiers.Add("%TEMP%", pathToTempFiles);
            dictPathIdentifiers.Add("%APP_DATA%", pathToAppData);
            dictPathIdentifiers.Add("%APP_DATA_LOCAL%", pathToAppDataLocal);
            dictPathIdentifiers.Add("%DOCUMENTS%", pathToDocuments);

            // =======================================
            // STARCRAFT II
            // =======================================
            // Install Path + add modifyregistry

            if (string.IsNullOrEmpty(ClientSettings.Default.SC2_INSTALL_PATH))
            {
                string pathToStarcraft2Folder = "";
                ModifyRegistry mysc2Registry = new ModifyRegistry();
                mysc2Registry.SubKey = "Software\\Blizzard Entertainment\\Starcraft II\\";
                mysc2Registry.ShowError = true;

                // 1a. Try Registry LocalMachine
                pathToStarcraft2Folder = mysc2Registry.Read("InstallPath");

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
                    string sc2_defaultPath = pathToProgramFiles + "/" + "StarCraft II/";
                    if (File.Exists(pathToProgramFiles + "StarCraft II.exe")) pathToStarcraft2Folder = sc2_defaultPath;
                }

                // 3. Try Default Path (Beta)
                if (string.IsNullOrEmpty(pathToStarcraft2Folder))
                {
                    string defaultPath = pathToProgramFiles + "/" + "StarCraft II Beta/";
                    if (File.Exists(pathToProgramFiles + "StarCraft II.exe")) pathToStarcraft2Folder = defaultPath;
                }
                ClientSettings.Default.SC2_INSTALL_PATH = pathToStarcraft2Folder;
            }
            dictPathIdentifiers.Add("%SC2_INSTALL_PATH%", ClientSettings.Default.SC2_INSTALL_PATH);
            

            // =======================================
            // STARCRAFT I
            // =======================================

            if (string.IsNullOrEmpty(ClientSettings.Default.SC1_INSTALL_PATH))
            {
                string pathToStarcraft1Folder = "";
                ModifyRegistry mysc1Registry = new ModifyRegistry();
                mysc1Registry.SubKey = "Software\\Blizzard Entertainment\\Starcraft\\";
                mysc1Registry.ShowError = true;

                // 1a. Try Registry LocalMachine (no idea if localmachine is used, dont think so, but i let it stay)
                pathToStarcraft1Folder = mysc1Registry.Read("InstallPath");

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
                    string sc1_defaultPath = pathToProgramFiles + "/" + "StarCraft/";

                    if (File.Exists(pathToProgramFiles + "StarCraft.exe")) pathToStarcraft1Folder = sc1_defaultPath;
                }
                ClientSettings.Default.SC1_INSTALL_PATH = pathToStarcraft1Folder;
            }
            dictPathIdentifiers.Add("%SC1_INSTALL_PATH%", ClientSettings.Default.SC1_INSTALL_PATH);
            

            //=====================
            //Warcraft 3
            //=====================

            if (string.IsNullOrEmpty(ClientSettings.Default.WC3_INSTALL_PATH))
            {
                string pathToWarcraft3Folder = "";
                ModifyRegistry mywc3Registry = new ModifyRegistry();
                mywc3Registry.SubKey = "Software\\Blizzard Entertainment\\Warcraft III\\";
                mywc3Registry.ShowError = true;

                // 1a. Try Registry LocalMachine (no idea if localmachine is used, dont think so, but i let it stay)
                pathToWarcraft3Folder = mywc3Registry.Read("InstallPath");

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
                ClientSettings.Default.WC3_INSTALL_PATH = pathToWarcraft3Folder;
            }
            dictPathIdentifiers.Add("%WC3_INSTALL_PATH%", ClientSettings.Default.WC3_INSTALL_PATH);

            // TODO: this isnt always nescesary
            ClientSettings.Default.Save();
        }

        
        public string GetPath(string pathIdentifier)
        {
            if (dictPathIdentifiers.ContainsKey(pathIdentifier))
            {
                string path = dictPathIdentifiers[pathIdentifier];
                return path;
            }
            else return "";
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
                        MapLoader.ClientSettings.Default.SC1_INSTALL_PATH = returnValue;
                        MapLoader.ClientSettings.Default.Save();
                        dictPathIdentifiers.Remove(pathIdentifier);
                        dictPathIdentifiers.Add(pathIdentifier, returnValue);
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
                        MapLoader.ClientSettings.Default.WC3_INSTALL_PATH = returnValue;
                        MapLoader.ClientSettings.Default.Save();
                        dictPathIdentifiers.Remove(pathIdentifier);
                        dictPathIdentifiers.Add(pathIdentifier, returnValue);
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
                        MapLoader.ClientSettings.Default.SC2_INSTALL_PATH = returnValue;
                        MapLoader.ClientSettings.Default.Save();
                        dictPathIdentifiers.Remove(pathIdentifier);
                        dictPathIdentifiers.Add(pathIdentifier, returnValue);
                    }
                    break;
            }
            return returnValue;
        }
    }
}
