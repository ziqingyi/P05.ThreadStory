using P05.ThreadFramework.Serialize;
using P05.ThreadModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using P05.ThreadFramework.Log;

namespace P05.ThreadStory
{
    class Program
    {
        private static readonly object lockObj = new object();
        static void Main(string[] args)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                bool isMonitor = true;

                List<StoryCharacter> scList = JsonHelper.JsonFileToObject<List<StoryCharacter>>("StoryCharacter.json");
                Stopwatch sw = new Stopwatch();
                sw.Start();

                bool FirstStoryDone = false;// first story started must print opening remarks.

                //iterate all characters' object and start to print their stories 
                foreach (StoryCharacter sc in scList)
                {
                    Action printExperienceList = () =>
                    {
                        foreach (var exp in sc.Experience)
                        {
                            //check first story being printed first, then try get lock ,
                            //once into lock, do something must do, then check again for opening remark.
                            if (FirstStoryDone == false)
                            {
                                lock (lockObj)
                                {
                                    string idtime1 = getThreadTime(); 
                                    LogHelper.LogConsole(exp+idtime1, sc.color);
                                    if (FirstStoryDone == false)
                                    {
                                        string idtime2 = getThreadTime();
                                        LogHelper.LogConsole("The stories begin......"+idtime2,ConsoleColor.White);
                                        FirstStoryDone = true;
                                    }
                                }
                            }
                            else
                            {
                                string idtime = getThreadTime();
                                LogHelper.LogConsole(exp + idtime, sc.color);
                            }
                        }
                    };


                    Task.Run(printExperienceList);



                }






                //Task.Factory.ContinueWhenAny(taskList.ToArray(), rArray =>
                //{
                //    string idtime = getThreadTime();
                //    LogHelper.LogConsole("The stories begin......"+idtime,ConsoleColor.White);
                //});











            }
            catch (Exception e)
            {
                LogHelper.LogConsole(e.Message,ConsoleColor.DarkBlue);
                throw;
            }
            Console.ReadKey();
        }

        public static string getThreadTime()
        {
            string threadId = Thread.CurrentThread.ManagedThreadId.ToString("00");
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return " In Thread: " + threadId + " Time: " + time;
        }


    }
}
