using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P05.ThreadFramework.Helper
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

        public static void LogConsole(string msg,bool appendTimeThread, ConsoleColor color)
        {
            
            lock (LogLock)
            {
                Console.ForegroundColor = color;
                foreach (char c in msg.ToCharArray())
                {
                    Thread.Sleep(5);                
                    Console.Write($"{c}");
                }
                string ThreadIdTime = "";
                if (appendTimeThread)
                {
                    ThreadIdTime = getThreadTime();
                    Console.Write(ThreadIdTime);
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Log(msg + ThreadIdTime);//log files put into lock--make sure early print, get lock early and print early. 
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
                        sw.WriteLine(String.Format("{0}:{1}", DateTime.Now, msg));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        ///append thread and time at the end of msg inside lock, get lock early and print early.
        /// as thread execute early not necessarily get lock early. 
        /// </summary>
        /// <returns></returns>
        public static string getThreadTime()
        {
            string threadId = Thread.CurrentThread.ManagedThreadId.ToString("00");
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return " In Thread: " + threadId + " Time: " + time;
        }
    }
}