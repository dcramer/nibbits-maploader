using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace MapLoader
{
    class ExtensionConfiguration
    {

        public Dictionary<string, Extension> dictExtConfiguration;


        public class Extension
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
