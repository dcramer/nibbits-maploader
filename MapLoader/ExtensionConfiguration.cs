using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace Nibbler
{
    class ExtensionConfiguration
    {

        private Dictionary<string, Extension> dictExtConfiguration;

        /// <summary>
        /// Checks if there's an action defined for this extension
        /// </summary>
        /// <returns></returns>
        public bool IsValidExtension(string extension)
        {
            return dictExtConfiguration.ContainsKey(extension);
        }


        /// <summary>
        /// Returns the path to copy the file to for a given extension.
        /// Will return a custom path for each OS if specified in extension.xml
        /// </summary>
        /// <returns></returns>
        public string GetCopyPathForExtension(string extension) {
            
            // check if we have something for this extension in store
            if (!dictExtConfiguration.ContainsKey(extension.ToLower())) return "";
            
            Extension currentExtension = dictExtConfiguration[extension.ToLower()];
            
            // Determining OS
            OperatingSystem os = System.Environment.OSVersion;
            Version osVersion = os.Version;

            // good ol' Xp
            if (osVersion.Major == 5)
            {
                if (currentExtension.dictActionsPerOS.ContainsKey("xp")) return currentExtension.dictActionsPerOS["xp"];
                else return currentExtension.dictActionsPerOS["default"];
            }
            // Windows Vista
            else if (osVersion.Major == 6 && osVersion.Minor == 0)
            {
                if (currentExtension.dictActionsPerOS.ContainsKey("vista")) return currentExtension.dictActionsPerOS["vista"];
                else return currentExtension.dictActionsPerOS["default"];
            }
            // Windows 7
            else if (osVersion.Major == 6 && osVersion.Minor == 1)
            {
                if (currentExtension.dictActionsPerOS.ContainsKey("windows7")) return currentExtension.dictActionsPerOS["windows7"];
                else return currentExtension.dictActionsPerOS["default"];
            }

            return currentExtension.dictActionsPerOS["default"];
        }


        /// <summary>
        /// Little container class for extensions & there copy-path's per OS
        /// </summary>
        class Extension
        {
            public string ExtensionName;            
            public Dictionary<string, string> dictActionsPerOS;

            public Extension(string name)
            {
                this.ExtensionName = name;
                dictActionsPerOS = new Dictionary<string, string>();
            }
        }

        public ExtensionConfiguration()
        {
            dictExtConfiguration = new Dictionary<string, Extension>();
        }

        /// <summary>
        /// Reads the extension.xml
        /// </summary>
        public void ReadConfig()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            XmlTextReader reader = new XmlTextReader( appPath + "/extensions.xml");


            Extension currentExtension = null;
            string currentAction = "";
            while (reader.Read())
            {

                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "extension")
                        {
                            string extName = reader.GetAttribute("type").ToLower();
                            currentExtension = new Extension(extName);
                        }
                        break;
                    case XmlNodeType.Text:
                        currentAction = reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "extension") dictExtConfiguration.Add(currentExtension.ExtensionName, currentExtension);
                        else currentExtension.dictActionsPerOS.Add(reader.Name, currentAction);
                        break;
                }
            }
            reader.Close();         

        }
    }
}
