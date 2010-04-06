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

        private readonly Dictionary<string, Extension> _dictExtConfiguration;

        /// <summary>
        /// Checks if there's an action defined for this extension
        /// </summary>
        /// <returns></returns>
        public bool IsValidExtension(string extension)
        {
            return _dictExtConfiguration.ContainsKey(extension);
        }


        /// <summary>
        /// Returns the path to copy the file to for a given extension.
        /// Will return a custom path for each OS if specified in extension.xml
        /// </summary>
        /// <returns></returns>
        public string GetCopyPathForExtension(string extension) {
            
            // check if we have something for this extension in store
            if (!_dictExtConfiguration.ContainsKey(extension.ToLower())) return "";
            
            Extension currentExtension = _dictExtConfiguration[extension.ToLower()];
            
            // Determining OS
            var os = Environment.OSVersion;
            var osVersion = os.Version;

            // good ol' Xp
            if (osVersion.Major == 5)
            {
                if (currentExtension.DictActionsPerOs.ContainsKey("xp")) return currentExtension.DictActionsPerOs["xp"];
                else return currentExtension.DictActionsPerOs["default"];
            }
            // Windows Vista
            else if (osVersion.Major == 6 && osVersion.Minor == 0)
            {
                if (currentExtension.DictActionsPerOs.ContainsKey("vista")) return currentExtension.DictActionsPerOs["vista"];
                else return currentExtension.DictActionsPerOs["default"];
            }
            // Windows 7
            else if (osVersion.Major == 6 && osVersion.Minor == 1)
            {
                if (currentExtension.DictActionsPerOs.ContainsKey("windows7")) return currentExtension.DictActionsPerOs["windows7"];
                else return currentExtension.DictActionsPerOs["default"];
            }

            return currentExtension.DictActionsPerOs["default"];
        }


        /// <summary>
        /// Little container class for extensions & there copy-path's per OS
        /// </summary>
        class Extension
        {
            public readonly string ExtensionName;            
            public readonly Dictionary<string, string> DictActionsPerOs;

            public Extension(string name)
            {
                ExtensionName = name;
                DictActionsPerOs = new Dictionary<string, string>();
            }
        }

        public ExtensionConfiguration()
        {
            _dictExtConfiguration = new Dictionary<string, Extension>();
        }

        /// <summary>
        /// Reads the extension.xml
        /// </summary>
        public void ReadConfig()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            var reader = new XmlTextReader( appPath + "/extensions.xml");


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
                        if (currentExtension != null)
                        {
                            if (reader.Name == "extension") _dictExtConfiguration.Add(currentExtension.ExtensionName, currentExtension);
                            else currentExtension.DictActionsPerOs.Add(reader.Name, currentAction);
                        }
                        else
                            throw new XmlException("Malformed XML");
                        break;
                }
            }
            reader.Close();         

        }
    }
}
