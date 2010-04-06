using System;
using System.Windows.Forms;

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
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Logger.UnhandledException);

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
