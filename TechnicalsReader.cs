using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace netdockerworker
{
    public class TechnicalsReader
    {
        private ICryptoAPI _cryptoAPI;
        private readonly string[] _allowedCoins = { "BTC", "ETH", "DOGE", "ADA", "LTC", "MATIC", "BNB", "EGLD", "NEO", "SOL", "XRP" };
        public TechnicalsReader()
        {
            _cryptoAPI = new CryptoAPI();
        }

        public List<Coin> GetAccountCoins()
        {
            var returnList = new List<Coin>();
            returnList.Add(new Coin
            {
                Symbol = "USDT",
                Oscilators = new List<Indicator>(),
                MovingAverages = new List<Indicator>()
            }); ;
            Parallel.ForEach(_allowedCoins, new ParallelOptions { MaxDegreeOfParallelism = 2 }, allowedCoin =>
            {
                returnList.Add(GetCoinWithTechnicals(allowedCoin));
            });
            UpdateAccountQuantities(returnList);
            UpdateCoinPrices(returnList);
            return returnList;
        }

        public void UpdateCoinPrices(List<Coin> returnList)
        {
            var pricesPairs = _cryptoAPI.GetPrices();

            foreach (var pair in pricesPairs)
            {
                returnList.First(x => x.Symbol == pair.Item1).Price = pair.Item2;
            }
        }

        public void UpdateAccountQuantities(List<Coin> returnList)
        {
            var account = _cryptoAPI.GetAccount();

            foreach (var symbol in account.Symbols)
            {
                returnList.First(x => x.Symbol == symbol.Name).Quantity = symbol.Quantity;
            }
        }

        private Coin GetCoinWithTechnicals(string coin)
        {
            var returnCoin = new Coin
            {
                Symbol = coin,
                Quantity = 0,
                Price = 0
            };

            returnCoin.Oscilators = GetTechnicalsMAs(coin, "relativestrengthindex");
            returnCoin.MovingAverages = GetTechnicalsMAs(coin, "ema");

            return returnCoin;
        }

        private List<Indicator> GetTechnicalsMAs(string coin, string side)
        {
            var returnList = new List<Indicator>();
            List<string> intervals = new List<string> { "1m", "1D" };

            Parallel.ForEach(intervals, new ParallelOptions { MaxDegreeOfParallelism = 2 }, interval =>
            {
                WebDriver _driver;

                if (Environment.GetEnvironmentVariable("ENV") == "HEROKU")
                {
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

                    _driver = new ChromeDriver(Environment.GetEnvironmentVariable("CHROMEDRIVER_PATH"), chromeOptions);
                }
                else
                {
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("window-size=1920,1080");
                    chromeOptions.AddArguments("disable-gpu");
                    chromeOptions.AddArguments("enable-javascript");
                    chromeOptions.AddArguments("disable-extensions");
                    chromeOptions.AddArguments("proxy-server='direct://'");
                    chromeOptions.AddArguments("proxy-bypass-list=*");
                    chromeOptions.AddArguments("start-maximized");
                    chromeOptions.AddArguments("headless");
                    _driver = new ChromeDriver(chromeOptions);
                }
                
                try
                {
                    var technicalsURL = "https://www.tradingview.com/symbols/" + coin + "USDT/technicals/";
                    _driver.Navigate().GoToUrl(technicalsURL);
                    Thread.Sleep(500);

                    Actions actions = new Actions(_driver);

                    Console.WriteLine($"Getting {interval}");
                    IWebElement page = _driver.FindElement(By.Id(interval));
                    actions.MoveToElement(page).Click().Perform();
                    Thread.Sleep(200);

                    var table = _driver.FindElements(By.XPath($"//a[@href='/ideas/{side}/']//ancestor::table[1]//descendant::tr")).ToList();

                    for (int i = 1; i < table.Count(); i++)
                    {
                        var nam = table[i].FindElements(By.XPath("./descendant::td")).ToList()[0].FindElement(By.XPath("./span/a")).GetAttribute("innerText");
                        var val = float.Parse(table[i].FindElements(By.XPath("./descendant::td")).ToList()[1].GetAttribute("innerText").Replace('−', '-'));
                        var decis = (DecisionEnum)Enum.Parse(typeof(DecisionEnum), table[i].FindElements(By.XPath("./descendant::td")).ToList()[2].GetAttribute("innerText"));

                        var indicator = new Indicator()
                        {
                            Name = nam,
                            Value = val,
                            Interval = interval.ToIntervalEnum(),
                            Decision = decis
                        };

                        returnList.Add(indicator);
                    }
                }
                finally
                {
                    _driver.Quit();
                }
            });

            return returnList;
        }
    }
}
