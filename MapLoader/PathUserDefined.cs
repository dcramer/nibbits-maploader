using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapLoader
{
    /// <summary>
    /// This class keeps track of user defined pathes.
    /// e.g. when PathFinder couldn't find the SC1 Path, we query the user to enter the SC1 Path.
    /// The input get saved so next time we don't need to query the user
    /// 
    /// </summary>
    class PathUserDefined
    {
        private Dictionary<string, string> dictPathIdentifiers;

        public PathUserDefined()
        {
            dictPathIdentifiers = new Dictionary<string, string>();
            dictPathIdentifiers.Add("%SC1_INSTALL_PATH%", "");
        }

        public string GetPath(string pathIdentifier) {
            if (!dictPathIdentifiers.ContainsKey(pathIdentifier)) return "";

            string path = dictPathIdentifiers[pathIdentifier];
            return path;
        }

        public string QueryUserForPath(Form1 form, string pathIdentifier)
        {
            string returnValue = "";
            switch (pathIdentifier)
            {
                case "%SC1_INSTALL_PATH%":
                    form.openFileDialog1.CheckFileExists = true;
                    form.openFileDialog1.FileName = "Starcraft.exe";
                    if (form.openFileDialog1.ShowDialog() == DialogResult.OK) {                        
                        returnValue = form.openFileDialog1.FileName;
                        returnValue = returnValue.ToLower().Replace("starcraft.exe", "");
                    }
                    break;
            }
            return returnValue;
        }
    }
}
