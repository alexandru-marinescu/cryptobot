﻿using OpenQA.Selenium;
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
            var chromeOptions = new ChromeOptions()
            {
                BinaryLocation = Environment.GetEnvironmentVariable("GOOGLE_CHROME_BIN"),
            };
            chromeOptions.AddArguments(new List<string>() { "headless", "disable-gpu", "no-sandbox", "disable-dev-shm-usage" });
            IWebDriver driver = new ChromeDriver(Environment.GetEnvironmentVariable("CHROMEDRIVER_PATH"), chromeOptions);
            var technicalsURL = "https://www.tradingview.com/symbols/MATICBTC/technicals/";
            driver.Navigate().GoToUrl(technicalsURL);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(15));
            Thread.Sleep(5000);
            wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/ideas/relativestrengthindex/']")));
            IWebElement element = driver.FindElement(By.XPath("//a[@href='/ideas/relativestrengthindex/']"));

            Console.WriteLine(element.GetAttribute("innerText"));
        }
    }
}
