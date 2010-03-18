using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility.ModifyRegistry;
using Microsoft.Win32;

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
        public Dictionary<string, string> dictPathIdentifiers;

        public PathUserDefined()
        {
            dictPathIdentifiers = new Dictionary<string, string>();
            //dictPathIdentifiers.Add("%SC1_INSTALL_PATH%", "");
            //dictPathIdentifiers.Add("%SC2_INSTALL_PATH%", "");
            //dictPathIdentifiers.Add("%WC3_INSTALL_PATH%", "");
        }

        public string GetPath(string pathIdentifier)
        {
            if (dictPathIdentifiers.ContainsKey(pathIdentifier))
            {
                string path = dictPathIdentifiers[pathIdentifier];
                MessageBox.Show(path, path, MessageBoxButtons.OK);
                return path;
            }
            else return "";
        }

        public string QueryUserForPath(Formsetup form, string pathIdentifier)
        {
            string returnValue = "";
            switch (pathIdentifier)
            {
                case "%SC1_INSTALL_PATH%":
                    form.openFileDialog1.CheckFileExists = true;
                    form.openFileDialog1.Filter = "Starcraft.exe|Starcraft.exe";
                    form.openFileDialog1.FileName = "Starcraft.exe";
                    if (form.openFileDialog1.ShowDialog() == DialogResult.OK) {                        
                        returnValue = form.openFileDialog1.FileName;
                        returnValue = returnValue.ToLower().Replace("starcraft.exe", "");
                        //Save path to registry
                        ModifyRegistry mysc1Registry = new ModifyRegistry();
                        mysc1Registry.BaseRegistryKey = Registry.CurrentUser;
                        mysc1Registry.SubKey = "Software\\Blizzard Entertainment\\Starcraft\\";
                        mysc1Registry.Write("InstallPath", returnValue);
                        dictPathIdentifiers.Remove("%SC1_INSTALL_PATH%");
                        dictPathIdentifiers.Add("%SC1_INSTALL_PATH%", returnValue);
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
                        //Save path to registry
                        ModifyRegistry mysc1Registry = new ModifyRegistry();
                        mysc1Registry.BaseRegistryKey = Registry.CurrentUser;
                        mysc1Registry.SubKey = "Software\\Blizzard Entertainment\\Warcraft III\\";
                        mysc1Registry.Write("InstallPath", returnValue);
                        dictPathIdentifiers.Remove("%WC3_INSTALL_PATH%");
                        dictPathIdentifiers.Add("%WC3_INSTALL_PATH%", returnValue);
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
                        //Save path to registry
                        ModifyRegistry mysc1Registry = new ModifyRegistry();
                        mysc1Registry.BaseRegistryKey = Registry.CurrentUser;
                        mysc1Registry.SubKey = "Software\\Blizzard Entertainment\\Starcraft II\\";
                        mysc1Registry.Write("InstallPath", returnValue);
                        dictPathIdentifiers.Remove("%SC2_INSTALL_PATH%");
                        dictPathIdentifiers.Add("%SC2_INSTALL_PATH%", returnValue);
                    }
                    break;


            }
            return returnValue;
        }
    }
}
