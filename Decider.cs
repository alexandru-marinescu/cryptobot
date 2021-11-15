using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netdockerworker
{
    public class Decider
    {
        private List<HistoryItem> _history = new List<HistoryItem>();
        private ICryptoAPI _cryptoAPI;
        private readonly decimal _tax = 0.998M;

        public Decider()
        {
            _cryptoAPI = new CryptoAPI();
        }
        public void AddCoins(List<Coin> coinList)
        {
            _history.Add(new HistoryItem
            {
                Timestamp = DateTime.Now,
                Coins = coinList
            });
        }

        public void DecideOrders(List<Coin> coinList)
        {
            var debug = coinList.Select(c => $"{c.Symbol} - {c.GetMovingAveragesScore(IntervalEnum.FiveMinutes) + c.GetOscilatorsScore(IntervalEnum.FiveMinutes)}");

            //choose only coins that are on daily uptrend
            coinList = coinList.Where(c => c.GetMovingAveragesScore(IntervalEnum.OneDay) + c.GetOscilatorsScore(IntervalEnum.OneDay) > 10).ToList();

            var neutralCandidates = coinList
                .Where(c =>
                c.GetMovingAveragesScore(IntervalEnum.FiveMinutes) + c.GetOscilatorsScore(IntervalEnum.FiveMinutes) < 0
                &&
                c.GetMovingAveragesScore(IntervalEnum.FiveMinutes) + c.GetOscilatorsScore(IntervalEnum.FiveMinutes) >= -6);

            var sellCandidates = coinList
                .Where(c => 
                c.GetMovingAveragesScore(IntervalEnum.FiveMinutes) + c.GetOscilatorsScore(IntervalEnum.FiveMinutes) < - 6
                &&
                c.GetMovingAveragesScore(IntervalEnum.FiveMinutes) + c.GetOscilatorsScore(IntervalEnum.FiveMinutes) >= - 12);

            var strongSellCandidates = coinList
                .Where(c => c.GetMovingAveragesScore(IntervalEnum.FiveMinutes) + c.GetOscilatorsScore(IntervalEnum.FiveMinutes) < -12);

            var accountOrders = _cryptoAPI.GetAccountOrders();
            var account = _cryptoAPI.GetAccount();

            foreach (var item in neutralCandidates)
            {
                if (item.Quantity > 0.0001M)
                {
                    var lastBuyOrder = accountOrders.Where(x => x.Side == "BUY" && x.Symbol == item.Symbol).OrderByDescending(x => x.Date).First();

                    if (lastBuyOrder.Price < item.Price * _tax)
                        _cryptoAPI.PlaceOrder(OrderEnum.SELL, item.Symbol, item.Quantity / 4);
                }
            }

            foreach (var item in sellCandidates)
            {
                if (item.Quantity > 0.0001M)
                {
                    var lastBuyOrder = accountOrders.Where(x => x.Side == "BUY" && x.Symbol == item.Symbol).OrderByDescending(x => x.Date).First();

                    if(lastBuyOrder.Price < item.Price * _tax)
                        _cryptoAPI.PlaceOrder(OrderEnum.SELL, item.Symbol, item.Quantity / 2);
                }
            }

            foreach (var item in strongSellCandidates)
            {
                if (item.Quantity > 0.0001M)
                {
                    var lastBuyOrder = accountOrders.Where(x => x.Side == "BUY" && x.Symbol == item.Symbol).OrderByDescending(x => x.Date).First();

                    if (lastBuyOrder.Price < item.Price * _tax)
                        _cryptoAPI.PlaceOrder(OrderEnum.SELL, item.Symbol, item.Quantity);
                }
            }

            foreach (var symbol in account.Symbols)
            {
                if(coinList.Exists(x => x.Symbol == symbol.Name))
                    coinList.First(x => x.Symbol == symbol.Name).Quantity = symbol.Quantity;
            }

            var USDTQuantity = account.Symbols.First(s => s.Name == "USDT").Quantity;
            if (USDTQuantity > 20)
            {
                var buyCandidates = coinList
                    .Where(c => c.GetMovingAveragesScore(IntervalEnum.FiveMinutes) + c.GetOscilatorsScore(IntervalEnum.FiveMinutes) > 8);

                foreach (var item in buyCandidates)
                {
                    _cryptoAPI.PlaceOrder(OrderEnum.BUY, item.Symbol, CalculateQuantity(USDTQuantity, buyCandidates, item));
                }
            }
        }

        private decimal CalculateQuantity(decimal quantity, IEnumerable<Coin> buyCandidates, Coin item)
        {
            var sumOfAllBuyCandidateScores = buyCandidates.Sum(c => c.GetMovingAveragesScore(IntervalEnum.FiveMinutes) + c.GetOscilatorsScore(IntervalEnum.FiveMinutes));
            var itemScore = item.GetOscilatorsScore(IntervalEnum.FiveMinutes) + item.GetMovingAveragesScore(IntervalEnum.FiveMinutes);

            return (quantity * itemScore) / sumOfAllBuyCandidateScores;
        }
    }
}
