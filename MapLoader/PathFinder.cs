using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace MapLoader
{
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
            // Install Path
            string pathToStarcraft2Folder = "";

            // 1. Try Registry
            pathToStarcraft2Folder = GetRegistryValueFromLocalMachine("Software\\Blizzard Entertainment\\Starcraft II", "InstallPath");

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
            // =======================================
            // 1. Try Default Path
            string pathToStarcraft1Folder = "";
            string sc1_defaultPath = pathToProgramFiles + "/" + "StarCraft/";
            if (File.Exists(pathToProgramFiles + "StarCraft.exe")) pathToStarcraft1Folder = sc1_defaultPath;

            dictPathIdentifiers.Add("%SC1_INSTALL_PATH%", pathToStarcraft1Folder);
        }

        
        public string GetPath(string pathIdentifier)
        {
            if (dictPathIdentifiers.ContainsKey(pathIdentifier)) return dictPathIdentifiers[pathIdentifier];
            else return "";
        }

     


        private string GetRegistryValueFromLocalMachine(string key, string value)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
            string retVal = "";
            if (registryKey != null)
            {
                retVal = registryKey.GetValue(value).ToString();
            }
            else
            {
                retVal = "";
            }
            registryKey.Close();
            return retVal;
        }

    }
}
