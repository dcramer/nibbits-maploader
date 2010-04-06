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
                return;
            }

            string link = args[0];
            
            //I recommend relocating "nibbits" somewhere, and adding some logic to register the helper.
            link = link.Replace(link.Substring(0, "nibbler".Length), "http");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoaderForm(link));
        }
    }
}
