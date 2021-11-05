using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace netdockerworker
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = Environment.GetEnvironmentVariable("API_KEY");
            Console.WriteLine($"Hello World. This is my key: {apiKey}");
            Thread.Sleep(60000);
            //HttpClient client = new HttpClient();
            //while(true) {
            //    client.Send(new HttpRequestMessage(
            //        HttpMethod.Post,
            //        "https://stefanini-counter.herokuapp.com/api/counter"
            //    ));     
            //    Console.WriteLine("Incrementing the counter at https://stefanini-counter.herokuapp.com/");
            //    Thread.Sleep(5000);
            //}
        }
    }
}
