using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MapLoader
{
    public partial class LoaderConfig : Form
    {
        ExtensionConfiguration extConf;
        PathFinder pathFinder;

        public LoaderConfig()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            ClientSettings.Default.SC1_INSTALL_PATH = tb_sc1dir.Text;
            ClientSettings.Default.SC2_INSTALL_PATH = tb_sc2dir.Text;
            ClientSettings.Default.WC3_INSTALL_PATH = tb_wc3dir.Text;
            ClientSettings.Default.Save();
            this.Close();
            // Application.Exit();
        }

        private void Formsetup_Load(object sender, EventArgs e)
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

            //Check if we know our dirs
            tb_sc1dir.Text = pathFinder.GetPath("%SC1_INSTALL_PATH%");

            tb_sc2dir.Text = pathFinder.GetPath("%SC2_INSTALL_PATH%");

            tb_wc3dir.Text = pathFinder.GetPath("%WC3_INSTALL_PATH%");
        }

        private void btn_sc1dir_Click(object sender, EventArgs e)
        {
            string sc1 = pathFinder.QueryUserForPath(this, "%SC1_INSTALL_PATH%");
            tb_sc1dir.Text = sc1;
        }

        private void btn_sc2dir_Click(object sender, EventArgs e)
        {
            string sc2 = pathFinder.QueryUserForPath(this, "%SC2_INSTALL_PATH%");
            tb_sc2dir.Text = sc2;
        }

        private void btn_wc3dir_Click(object sender, EventArgs e)
        {
            string wc3 = pathFinder.QueryUserForPath(this, "%WC3_INSTALL_PATH%");
            tb_wc3dir.Text = wc3;
        }
    }
}
