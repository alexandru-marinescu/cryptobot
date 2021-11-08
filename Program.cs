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
            Console.WriteLine("I am here:");
            Console.WriteLine(Environment.CurrentDirectory);
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
