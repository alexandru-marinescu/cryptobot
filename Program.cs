using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace netdockerworker
{
    class Program
    {
        static void Main(string[] args)
        {
            //heroku
            var chromeOptions = new ChromeOptions()
            {
                BinaryLocation = Environment.GetEnvironmentVariable("GOOGLE_CHROME_BIN"),
            };
            chromeOptions.AddArguments(new List<string>() { "headless", "disable-gpu", "no-sandbox", "disable-dev-shm-usage" });
            IWebDriver driver = new ChromeDriver(Environment.GetEnvironmentVariable("CHROMEDRIVER_PATH"), chromeOptions);

            //local
            //var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments(new List<string>() { "headless", "disable-gpu"});
            //IWebDriver driver = new ChromeDriver(chromeOptions);

            var technicalsURL = "https://www.tradingview.com/symbols/MATICBTC/technicals/";
            driver.Navigate().GoToUrl(technicalsURL);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(15));
            Console.WriteLine("waiting 14 seconds");
            Thread.Sleep(14000);
            Console.WriteLine("done waiting");
            wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/ideas/relativestrengthindex/']")));
            IWebElement element = driver.FindElement(By.XPath("//a[@href='/ideas/relativestrengthindex/']"));

            Console.WriteLine(element.GetAttribute("innerText"));
        }
    }
}
