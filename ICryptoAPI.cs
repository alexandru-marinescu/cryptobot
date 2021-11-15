using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netdockerworker
{
    interface ICryptoAPI
    {
        decimal GetPrice(string symbol);
        List<Tuple<string, decimal>> GetPrices();
        Account GetAccount();
        List<Order> GetAccountOrders();
        void PlaceOrder(OrderEnum orderType, string symbol, decimal quantity);
    }
}
