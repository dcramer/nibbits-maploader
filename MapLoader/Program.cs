using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

//AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
//static void UnhandledException(object sender, UnhandledExceptionEventArgs ex);

namespace Nibbler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.Run(new LoaderConfig());
                Application.Exit();
                return;
            }

            string link = args[0];
            //string link = "nibbits://wc3.nibbits.com/maps/get/131603/"
            
            link = link.Replace("nibbits://", "http://");
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoaderForm(link));
        }


        static void UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
        }
    }
}
