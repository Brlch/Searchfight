using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;

namespace Searchfight
{
    class Program
    {
        static async Task Main(string[] args)
        {



            HttpClient client = new HttpClient();
            foreach (var arg in args)
            {
                Console.Write(arg + ":");

                //Yahoo Side
                Console.Write(" Yahoo: ");
                //var response = await client.GetAsync("");
                var response = await client.GetAsync("https://search.yahoo.com/search?p=" + arg);
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                List<String> LstStrings = new List<String>(dataObjects.Split("class=\"next\""));
                Double ResultsYahoo = Double.Parse(LstStrings.Last().Split("span>")[1].Split(" ")[0].Replace(",", ""));
                Console.Write(ResultsYahoo);


                //Google Side
                Console.Write(" Google: ");
                //var response = await client.GetAsync("");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1312.57 Safari/537.17");
                response = await client.GetAsync("https://www.google.com/search?q=" + arg);
                dataObjects = response.Content.ReadAsStringAsync().Result;
                String datos = dataObjects.Split("id=\"result-stats\">").Last().Split("resul")[0];
                Regex digitsOnly = new Regex(@"[^\d]");
                Double ResultsGoogle = Double.Parse(digitsOnly.Replace(datos, ""));
                Console.Write(ResultsGoogle);

                //Finished line
                Console.Write("\n");

            }
        }


    }
}
