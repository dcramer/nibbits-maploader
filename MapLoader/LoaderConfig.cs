using System;
using System.Windows.Forms;

namespace Nibbler
{
    public partial class LoaderConfig : Form
    {
        ExtensionConfiguration _extConf;
        PathFinder _pathFinder;

        public LoaderConfig()
        {
            InitializeComponent();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            Properties.Client.Default.SC1_INSTALL_PATH = tb_sc1dir.Text;
            Properties.Client.Default.SC2_INSTALL_PATH = tb_sc2dir.Text;
            Properties.Client.Default.WC3_INSTALL_PATH = tb_wc3dir.Text;
            Properties.Client.Default.Save();
            Close();
            // Application.Exit();
        }

        private void Formsetup_Load(object sender, EventArgs e)
        {
            // ==========================================================
            // Reading XML Extension Config
            // ==========================================================
            _extConf = new ExtensionConfiguration();
            _extConf.ReadConfig();

            // ==========================================================
            // Find Pathes for Game Folders etc.
            // ==========================================================
            _pathFinder = new PathFinder();
            _pathFinder.GatherPathes();

            //Check if we know our dirs
            tb_sc1dir.Text = _pathFinder.GetPath("%SC1_INSTALL_PATH%");

            tb_sc2dir.Text = _pathFinder.GetPath("%SC2_INSTALL_PATH%");

            tb_wc3dir.Text = _pathFinder.GetPath("%WC3_INSTALL_PATH%");
        }

        private void BtnSc1DirClick(object sender, EventArgs e)
        {
            string sc1 = _pathFinder.QueryUserForPath(this, "%SC1_INSTALL_PATH%");
            tb_sc1dir.Text = sc1;
        }

        private void BtnSc2DirClick(object sender, EventArgs e)
        {
            string sc2 = _pathFinder.QueryUserForPath(this, "%SC2_INSTALL_PATH%");
            tb_sc2dir.Text = sc2;
        }

        private void BtnWc3DirClick(object sender, EventArgs e)
        {
            string wc3 = _pathFinder.QueryUserForPath(this, "%WC3_INSTALL_PATH%");
            tb_wc3dir.Text = wc3;
        }
    }
}
