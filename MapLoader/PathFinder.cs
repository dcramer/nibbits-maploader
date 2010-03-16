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

        private Dictionary<string, string> dictPathIdentifiers;

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
            string pathToStarcraft2Folder = "";
            ModifyRegistry mysc2Registry = new ModifyRegistry();
            mysc2Registry.SubKey = "Software\\Blizzard Entertainment\\Starcraft\\";
            mysc2Registry.ShowError = true;

            // 1a. Try Registry LocalMachine
            pathToStarcraft2Folder = mysc2Registry.Read("InstallPath");

            // 1b. Try Registry CurrentUser
            if (pathToStarcraft2Folder == null)
            {
                mysc2Registry.BaseRegistryKey = Registry.CurrentUser;
                pathToStarcraft2Folder = mysc2Registry.Read("InstallPath");
            }

            if (pathToStarcraft2Folder == null)
            {
                pathToStarcraft2Folder = "";
            }

            // 2. Try Default Path
            if (pathToStarcraft2Folder == "")
            {
                string sc2_defaultPath = pathToProgramFiles + "/" + "StarCraft II/";
                if (File.Exists(pathToProgramFiles + "StarCraft II.exe")) pathToStarcraft2Folder = sc2_defaultPath;
            }

            // 3. Try Default Path (Beta)
            if (pathToStarcraft2Folder == "")
            {
                string defaultPath = pathToProgramFiles + "/" + "StarCraft II Beta/";
                if (File.Exists(pathToProgramFiles + "StarCraft II.exe")) pathToStarcraft2Folder = defaultPath;
            }

            dictPathIdentifiers.Add("%SC2_INSTALL_PATH%", pathToStarcraft2Folder);

            // =======================================
            // STARCRAFT I
            // (no registry key to my knowledge)
            // ^-- (well there is!)
            // =======================================
            //
            string pathToStarcraft1Folder = "";
            ModifyRegistry mysc1Registry = new ModifyRegistry();
            mysc1Registry.SubKey = "Software\\Blizzard Entertainment\\Starcraft\\";
            mysc1Registry.ShowError = true;

            // 1a. Try Registry LocalMachine (no idea if localmachine is used, dont think so, but i let it stay)
            pathToStarcraft1Folder = mysc1Registry.Read("InstallPath");

            // 1b. Try Registry CurrentUser
            if (pathToStarcraft1Folder == null)
            {
                mysc1Registry.BaseRegistryKey = Registry.CurrentUser;
                pathToStarcraft1Folder = mysc1Registry.Read("InstallPath");
            }

            if (pathToStarcraft1Folder == null)
            {
                pathToStarcraft1Folder = "";
            }

            // 2. Try Default Path
            if (pathToStarcraft1Folder == "")
            {
                string sc1_defaultPath = pathToProgramFiles + "/" + "StarCraft/";

                if (File.Exists(pathToProgramFiles + "StarCraft.exe")) pathToStarcraft1Folder = sc1_defaultPath;
            }

            dictPathIdentifiers.Add("%SC1_INSTALL_PATH%", pathToStarcraft1Folder);
        }

        
        public string GetPath(string pathIdentifier)
        {
            if (dictPathIdentifiers.ContainsKey(pathIdentifier)) return dictPathIdentifiers[pathIdentifier];
            else return "";
        }


        // TRY TO READ REGISTRY FOR SC2 KEY (THIS IS NOT NEEDED ANYMORE)
        private string GetRegistryValueFromLocalMachine(string key, string value)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
            string retVal = "";
            if (registryKey != null)
            {
                retVal = registryKey.GetValue(value).ToString();
            }

            RegistryKey registryKeyCurrent = Registry.CurrentUser.OpenSubKey(key);
            if (registryKeyCurrent != null)
            {
                retVal = registryKeyCurrent.GetValue(value).ToString();
            }

            else
            {
                return null;
            }
            registryKey.Close();
            return retVal;
        }

    }
}
