using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        public float GetPrice(string symbol)
        {
            throw new NotImplementedException();
        }

        public List<Tuple<string, float>> GetPrices()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Key", Environment.GetEnvironmentVariable("API_KEY"));
            List<Tuple<string, float>> prices = null;
            HttpResponseMessage response = client.GetAsync("https://crypto-bot-challenge-api.herokuapp.com/api/trading/prices").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                prices = JsonConvert.DeserializeObject<List<Price>>(jsonResponse)
                    .Select(p => new Tuple<string, float>(p.Name, float.Parse(p.Value))).ToList();
            }
            return prices;
        }

        public void PlaceOrder(OrderEnum orderType, string symbol, float quantity)
        {
            throw new NotImplementedException();
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
