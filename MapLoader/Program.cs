using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MapLoader
{
    static class Program
    {

        public static string NIBBITS_LINK;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.Run(new Formsetup());
                Application.Exit();
                return;
            }

            NIBBITS_LINK = args[0];
            //NIBBITS_LINK = "nibbits://www.nibbits.com/sc/maps/get/131403/";
            //NIBBITS_LINK = "nibbits://wc3.nibbits.com/maps/get/131603/";
            //NIBBITS_LINK = "nibbits://www.nibbits.com/sc2/maps/get/131540/";


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
