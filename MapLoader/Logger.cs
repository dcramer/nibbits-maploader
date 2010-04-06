using System;
using System.IO;

namespace Nibbler
{
    /// <summary>
    /// Logs exceptions.
    /// </summary>
    class Logger
    {
        public static void UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            string logPath = Environment.GetCommandLineArgs()[0] + ".log";

            if (!File.Exists(logPath))
            {
                File.AppendText(logPath);
            }
            StreamWriter logFile = new StreamWriter(logPath, true);
            logFile.WriteLine();
            logFile.WriteLine();
            logFile.WriteLine();
            logFile.WriteLine("-----------------------------------------");
            logFile.WriteLine("System Environment is " + System.Environment.OSVersion.Platform + " With " + System.Environment.ProcessorCount + " Logical Cores");
            logFile.WriteLine("Application Arguments were " + Environment.GetCommandLineArgs());
            logFile.WriteLine("Exception occurred at {0}", DateTime.Now.ToString());
            logFile.WriteLine("-----------------------------------------");
            logFile.WriteLine(ex.ToString());
            logFile.WriteLine("-----------------------------------------");
            logFile.Flush();
            logFile.Close();
        }
    }
}