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

                List<Task> tlist = new List<Task>();
                bool FirstStoryDone = false;// first story started must print opening remarks.

                //iterate all characters' object and start to print their stories 
                foreach (StoryCharacter sc in scList)
                {
                    Action<object> printExperienceList = o =>
                    {
                        foreach (var exp in sc.Experience)
                        {
                            //check first story being printed first, then try get lock ,
                            //once into lock, do something must do, then check again for opening remark.

                            if (FirstStoryDone == false)//some threads start with check first story
                            {
                                lock (lockObj)  //some threads wait for lockObj, only one goes into lock each time.
                                {


                                    if (!cts.IsCancellationRequested)
                                    {

                                        LogHelper.LogConsole(o.ToString() + "--" + exp + getThreadTime(), sc.color);

                                        if (FirstStoryDone == false)
                                        {

                                            LogHelper.LogConsole("The stories begin......" + getThreadTime(), ConsoleColor.White);
                                            FirstStoryDone = true;

                                        }
                                    }

                                }
                            }
                            else
                            {

                                if (!cts.IsCancellationRequested)
                                {  LogHelper.LogConsole(o.ToString() + " " + exp + getThreadTime(), sc.color);}


                            }

                            if (cts.IsCancellationRequested)
                            {
                                break;//stop the following stories. 
                            }


                        }
                    };


                    tlist.Add(Task.Factory.StartNew(printExperienceList, sc.Name, cts.Token));



                }

                //anyone finish all stories will print below 
                Task.Factory.ContinueWhenAny(tlist.ToArray(), t =>
                {

                    if (!cts.IsCancellationRequested)
                    {
                        LogHelper.LogConsole($"{t.AsyncState} finish all stories......" + getThreadTime() , ConsoleColor.White);
                    }
                    
                });


                //start new thread for cancel all
                Task.Run(() =>
                {
                    int ran = 0;

                    while (isMonitor &&  ran != DateTime.Now.Year)
                    {
                        ran = new Random().Next(2010, 2090);
                        Thread.Sleep(10);
                    }
                    
                    if (isMonitor)
                    {
                        cts.Cancel();
                        LogHelper.LogConsole("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"+ ran + getThreadTime(), ConsoleColor.White);
                    }
                    else
                    {
                        LogHelper.LogConsole("no monitor is requiredxxxxxxxxxxx" + ran + getThreadTime(), ConsoleColor.White);
                    }
                });



                //all stories finish will print below
                Task.Factory.ContinueWhenAll(tlist.ToArray(), tArray =>
                {
                    if (!cts.IsCancellationRequested)
                    {
                        isMonitor = false;
                        sw.Stop();
                        LogHelper.LogConsole($"The stories come to the End*********Total: {sw.ElapsedMilliseconds} ms" + getThreadTime(), ConsoleColor.White);
                    }

                });









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
