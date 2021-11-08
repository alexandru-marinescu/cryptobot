using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
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
            //for (int i = 0; i < 100; i++)
            //{
                //Console.WriteLine(Environment.GetEnvironmentVariable("GOOGLE_CHROME_BIN"));
                //Console.WriteLine(Environment.GetEnvironmentVariable("CHROMEDRIVER_PATH"));
                Thread.Sleep(1000);
                foreach (var item in Directory.EnumerateDirectories(Environment.CurrentDirectory))
                {
                    Console.WriteLine(item);
                }
                //Console.WriteLine(Directory.EnumerateDirectories(Environment.CurrentDirectory));
            //}
            //heroku
            var chromeOptions = new ChromeOptions()
            {
                BinaryLocation = Environment.GetEnvironmentVariable("GOOGLE_CHROME_BIN"),
            };
            chromeOptions.AddArguments("window-size=1920,1080");
            chromeOptions.AddArguments("disable-gpu");
            chromeOptions.AddArguments("enable-javascript");
            chromeOptions.AddArguments("disable-extensions");
            chromeOptions.AddArguments("proxy-server='direct://'");
            chromeOptions.AddArguments("proxy-bypass-list=*");
            chromeOptions.AddArguments("start-maximized");
            chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("no-sandbox");
            chromeOptions.AddArguments("disable-dev-shm-usage");
            IWebDriver driver = new ChromeDriver(Environment.GetEnvironmentVariable("CHROMEDRIVER_PATH"), chromeOptions);

            //local
            //var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("window-size=1920,1080");
            //chromeOptions.AddArguments("disable-gpu");
            //chromeOptions.AddArguments("enable-javascript");
            //chromeOptions.AddArguments("disable-extensions");
            //chromeOptions.AddArguments("proxy-server='direct://'");
            //chromeOptions.AddArguments("proxy-bypass-list=*");
            //chromeOptions.AddArguments("start-maximized");
            //chromeOptions.AddArguments("headless");
            //IWebDriver driver = new ChromeDriver(chromeOptions);


            var technicalsURL = "https://www.tradingview.com/symbols/MATICBTC/technicals/";
            driver.Navigate().GoToUrl(technicalsURL);
            Thread.Sleep(5000);

            Actions actions = new Actions(driver);
            //var startTime = DateTime.Now;

            while (true)
            {
                IWebElement page = driver.FindElement(By.Id("1m"));
                actions.MoveToElement(page).Click().Perform();
                Thread.Sleep(2000);

                List<IWebElement> table = driver.FindElements(By.XPath("//a[@href='/ideas/relativestrengthindex/']//ancestor::table[1]//descendant::tr")).ToList();

                for (int i = 1; i < table.Count(); i++)
                {
                    Console.WriteLine($"" +
                        $"{table[i].FindElements(By.XPath("./descendant::td")).ToList()[0].FindElement(By.XPath("./span/a")).GetAttribute("innerText")}" +
                        $"  -  " +
                        $"{table[i].FindElements(By.XPath("./descendant::td")).ToList()[1].GetAttribute("innerText")}" +
                        $"  -  " +
                        $"{table[i].FindElements(By.XPath("./descendant::td")).ToList()[2].GetAttribute("innerText")}");
                }
                Console.WriteLine();

                page = driver.FindElement(By.Id("5m"));
                actions.MoveToElement(page).Click().Perform();
                Thread.Sleep(2000);

                table = driver.FindElements(By.XPath("//a[@href='/ideas/relativestrengthindex/']//ancestor::table[1]//descendant::tr")).ToList();

                for (int i = 1; i < table.Count(); i++)
                {
                    Console.WriteLine($"" +
                        $"{table[i].FindElements(By.XPath("./descendant::td")).ToList()[0].FindElement(By.XPath("./span/a")).GetAttribute("innerText")}" +
                        $"  -  " +
                        $"{table[i].FindElements(By.XPath("./descendant::td")).ToList()[1].GetAttribute("innerText")}" +
                        $"  -  " +
                        $"{table[i].FindElements(By.XPath("./descendant::td")).ToList()[2].GetAttribute("innerText")}");
                }
                Console.WriteLine();

                page = driver.FindElement(By.Id("15m"));
                actions.MoveToElement(page).Click().Perform();
                Thread.Sleep(2000);

                table = driver.FindElements(By.XPath("//a[@href='/ideas/relativestrengthindex/']//ancestor::table[1]//descendant::tr")).ToList();

                for (int i = 1; i < table.Count(); i++)
                {
                    Console.WriteLine($"" +
                        $"{table[i].FindElements(By.XPath("./descendant::td")).ToList()[0].FindElement(By.XPath("./span/a")).GetAttribute("innerText")}" +
                        $"  -  " +
                        $"{table[i].FindElements(By.XPath("./descendant::td")).ToList()[1].GetAttribute("innerText")}" +
                        $"  -  " +
                        $"{table[i].FindElements(By.XPath("./descendant::td")).ToList()[2].GetAttribute("innerText")}");
                }
                Console.WriteLine();

                Thread.Sleep(30000);
            }
        }


    }
}
