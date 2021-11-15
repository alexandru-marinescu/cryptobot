using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace netdockerworker
{
    class Program
    {
        static void Main(string[] args)
        {
            var tr = new TechnicalsReader();

            var decider = new Decider();


            Console.WriteLine("Started app");
            int iteration = 0;
            int maxFailure = 10;
            while (maxFailure > 0)
            {
                iteration++;
                try
                {
                    Console.WriteLine("Iteration " + iteration);
                    DateTime start = DateTime.Now;
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    var coins = tr.GetAccountCoins();
                    decider.DecideOrders(coins);
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    var remaining = new TimeSpan(0, 5, 0) - ts;
                    Thread.Sleep(remaining);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    maxFailure--;
                }
            }
            Console.WriteLine("Stopped app");
            Console.ReadKey();
        }
    }
}
