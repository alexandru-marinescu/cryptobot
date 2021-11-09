using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netdockerworker
{
    public static class Extensions
    {
        public static string ToLinkIds(this IntervalEnum interval)
        {
            switch (interval)
            {
                case IntervalEnum.OneMinute:
                    return "1m"; break;
                case IntervalEnum.FiveMinutes:
                    return "5m"; break;
                case IntervalEnum.FifteenMinutes:
                    return "15m"; break;
                case IntervalEnum.OneHour:
                    return "1h"; break;
                case IntervalEnum.FourHours:
                    return "4h"; break;
                case IntervalEnum.OneDay:
                    return "1D"; break;
                case IntervalEnum.OneWeek:
                    return "1W"; break;
                default:
                    return "1D";
            }
        }

        public static IntervalEnum ToIntervalEnum(this string interval)
        {
            switch (interval)
            {
                case "1m":
                    return IntervalEnum.OneMinute; break;
                case "5m":
                    return IntervalEnum.FiveMinutes; break;
                case "15m":
                    return IntervalEnum.FifteenMinutes; break;
                case "1h":
                    return IntervalEnum.OneHour; break;
                case "4h":
                    return IntervalEnum.FourHours; break;
                case "1D":
                    return IntervalEnum.OneDay; break;
                case "1W":
                    return IntervalEnum.OneWeek; break;
                default:
                    return IntervalEnum.OneDay;
            }
        }
    }
}
