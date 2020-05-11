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
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Receive search terms alredy in args
            List<String> SearchTerms = args.ToList();
            Int64 tmpPopularityNumber = -1;
            if(SearchTerms.Count==0)
            {
                Console.WriteLine("No terms provided.");
                return;
            }


            //Max popularity number by engine (value and name in tuple)
            Dictionary<Logic.SupportedSearchEngines, Tuple<Int64,String>> MaxPerEngine = new Dictionary<Logic.SupportedSearchEngines, Tuple<Int64,String>>();
            
            //Max popularity number overall
            Int64 MaxOverall = 0;
            String OverallWinner = "";

            foreach (var term in SearchTerms)
            {
                //Line start
                Console.Write(term + ":");


               

                // Run through all supported engines
                foreach (Logic.SupportedSearchEngines engine in Enum.GetValues(typeof(Logic.SupportedSearchEngines)))
                {

                    //Initialize if not alredy done so
                    if (!MaxPerEngine.ContainsKey(engine))
                        MaxPerEngine[engine] = new Tuple<long, string>(0,"");

                    try
                    {
                        Console.Write(" "+Enum.GetName(typeof(Logic.SupportedSearchEngines),engine)+": ");

                        //Get Popularity number from Logic and compare maximums
                        tmpPopularityNumber = await Logic.GetPopularityNumber(engine, term);
                        if(tmpPopularityNumber>MaxOverall)
                        {
                            //Handle possible ties
                            OverallWinner = tmpPopularityNumber == MaxOverall ? OverallWinner + "," + term : term;
                        }
                        MaxOverall = Math.Max(MaxOverall, tmpPopularityNumber);
                        if(tmpPopularityNumber> MaxPerEngine[engine].Item1)
                        {
                            //Same as previous, but for every engine
                            MaxPerEngine[engine] = new Tuple<long, string>(tmpPopularityNumber, (MaxPerEngine[engine].Item1 == tmpPopularityNumber ? MaxPerEngine[engine].Item2 + ", ": term) );

                        }

                        Console.Write(tmpPopularityNumber.ToString());
                    }
                    catch (Exception ex)
                    {
                        //Log errors if any arise
                        Console.Write(" - ");
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }

                //Finished line
                Console.Write("\n");

            }


            //Winners section
            List<String> winners = new List<string>();
            foreach (Logic.SupportedSearchEngines engine in Enum.GetValues(typeof(Logic.SupportedSearchEngines)))
                Console.WriteLine(Enum.GetName(typeof(Logic.SupportedSearchEngines),engine)+" winner: " + MaxPerEngine[engine].Item2);

            Console.WriteLine("Total winner: " + OverallWinner);

        }


    }
}
