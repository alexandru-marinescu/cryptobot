using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netdockerworker
{
    public class Coin
    {
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public List<Indicator> Oscilators { get; set; }
        public List<Indicator> MovingAverages { get; set; }

        public int GetOscilatorsScore(IntervalEnum interval)
        {
            var score = 0;

            foreach (var indicator in Oscilators.Where(x => x.Interval == interval))
            {
                switch(indicator.Decision)
                {
                    case DecisionEnum.Buy:
                        score += 1; break;
                    case DecisionEnum.Sell:
                        score -= 1; break;
                }
            }

            return score;
        }

        public int GetMovingAveragesScore(IntervalEnum interval)
        {
            var score = 0;

            foreach (var indicator in MovingAverages.Where(x => x.Interval == interval))
            {
                switch (indicator.Decision)
                {
                    case DecisionEnum.Buy:
                        score += 1; break;
                    case DecisionEnum.Sell:
                        score -= 1; break;
                }
            }

            return score;
        }
    }

    public class Indicator
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public IntervalEnum Interval { get; set; }
        public DecisionEnum Decision { get; set; }
    }

    public class Account
    {
        public bool IsOfficial { get; set; }
        public Symbol[] Symbols { get; set; }
        public string Name { get; set; }
        public int OrdersCounts { get; set; }
        public decimal EstimatedValue { get; set; }
        public DateTime LastUpdated { get; set; }

    }

    public class Symbol
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
    }

    public enum DecisionEnum
    {
        Buy,
        Sell,
        Neutral
    }

    public enum IntervalEnum
    {
        OneMinute,
        FiveMinutes,
        FifteenMinutes,
        OneHour,
        FourHours,
        OneDay,
        OneWeek
    }

    public enum OrderEnum
    {
        BUY,
        SELL
    }

    public class HistoryItem
    {
        public DateTime Timestamp { get; set; }
        public List<Coin> Coins { get; set; }
    }

    public class Order
    {
        public DateTime Date { get; set; }
        public string Symbol { get; set; }
        public string Side { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class Content
    {
        public string Symbol { get; set; }
        public string Side { get; set; }
        public string Quantity { get; set; }
    }
}
