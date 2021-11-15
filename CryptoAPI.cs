using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace netdockerworker
{
    public class CryptoAPI : ICryptoAPI
    {
        public Account GetAccount()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Key", Environment.GetEnvironmentVariable("API_KEY"));
            Account account = null;
            HttpResponseMessage response = client.GetAsync("https://crypto-bot-challenge-api.herokuapp.com/api/trading/account").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                account = JsonConvert.DeserializeObject<Account>(jsonResponse);
            }
            return account;
        }

        public decimal GetPrice(string symbol)
        {
            throw new NotImplementedException();
        }

        public List<Tuple<string, decimal>> GetPrices()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Key", Environment.GetEnvironmentVariable("API_KEY"));
            List<Tuple<string, decimal>> prices = null;
            HttpResponseMessage response = client.GetAsync("https://crypto-bot-challenge-api.herokuapp.com/api/trading/prices").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                prices = JsonConvert.DeserializeObject<List<Price>>(jsonResponse)
                    .Select(p => new Tuple<string, decimal>(p.Name, decimal.Parse(p.Value))).ToList();
            }
            return prices;
        }

        public List<Order> GetAccountOrders()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Key", Environment.GetEnvironmentVariable("API_KEY"));
            List<Order> orders = null;
            HttpResponseMessage response = client.GetAsync("https://crypto-bot-challenge-api.herokuapp.com/api/trading/orderHistory").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                orders = JsonConvert.DeserializeObject<List<Order>>(jsonResponse);
            }
            return orders;
        }

        public void PlaceOrder(OrderEnum orderType, string symbol, decimal quantity)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Key", Environment.GetEnvironmentVariable("API_KEY"));

            quantity = Math.Floor(quantity * 100000000) / 100000000;

            var dict = new Dictionary<string, string>();
            dict.Add("symbol", symbol);
            dict.Add("side", orderType.ToString());
            dict.Add("quantity", quantity.ToString());

            var response = client.PostAsync("https://crypto-bot-challenge-api.herokuapp.com/api/trading/order", new FormUrlEncodedContent(dict)).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("********************************************************************");
                Console.WriteLine($"{orderType} - {symbol} - {quantity}");
                Console.WriteLine("********************************************************************");
            }
            else
            {
                Console.WriteLine($"Tried to {orderType} {quantity} {symbol} but failed:");
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        private class Price
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private class Symbol
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
        }
    }

}
