using P05.ThreadFramework.Serialize;
using P05.ThreadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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







            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            Console.ReadKey();
        }
    }
}
