using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P05.ThreadFramework.Log
{
    public class LogHelper
    {
        static LogHelper()
        {
            if (!Directory.Exists(StaticConstant.LogPath))
            {
                Directory.CreateDirectory(StaticConstant.LogPath);
            }
        }
        private static readonly object LogLock = new object();

        public static void LogConsole(string msg, ConsoleColor color)
        {
            lock (LogLock)
            {
                foreach (char c in msg.ToCharArray())
                {
                    //Thread.Sleep(5);
                    Console.ForegroundColor = color;
                    Console.Write($"{c}");
                }
                Console.WriteLine(); 
                Log(msg);//log files put into lock--make sure early print, get lock early and print early. 
            }
        }

        public static void Log(string msg)
        {
            try
            {
                string fileName = "log.txt";
                string fullPath = Path.Combine(StaticConstant.LogPath, fileName);
                lock (LogLock)
                {
                    using (StreamWriter sw = File.AppendText(fullPath))
                    {
                        sw.WriteLine(string.Format("{0}:{1}", DateTime.Now, msg));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}