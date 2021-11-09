using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netdockerworker
{
    interface ICryptoAPI
    {
        float GetPrice(string symbol);
        List<Tuple<string, float>> GetPrices();
        Account GetAccount();
        void PlaceOrder(OrderEnum orderType, string symbol, float quantity);
    }
}
