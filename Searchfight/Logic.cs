using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Searchfight
{
    /// <summary>
    /// Searchfight logic for getting popularity number
    /// </summary>
    public static class Logic
    {
        //Instance client once 
        private static HttpClient client = new HttpClient();

        public enum SupportedSearchEngines
        {
            Google,
            Yahoo
        }
  
        public static async Task<Int64> GetPopularityNumber(SupportedSearchEngines engine, String term)
        {
            try
            {
                switch (engine)
                {
                    case SupportedSearchEngines.Google:
                        return await GetGoogleWebScrappingMethod1(term);
                    case SupportedSearchEngines.Yahoo:
                        return await GetYahooWebScrappingMethod1(term);
                }

                throw new Exception("Engine not supported");
            }
            catch (Exception ex)
            {
                throw new Exception("Method for getting popularity number failed: ",ex);
            }
        }


        #region Methods for obtaining popularity number
        public static async Task<Int64> GetYahooWebScrappingMethod1(String term)
        {
            var response = await client.GetAsync("https://search.yahoo.com/search?p=" + term);
            var dataObjects = response.Content.ReadAsStringAsync().Result;
            //Found unique class for webscrapping 
            List<String> LstStrings = new List<String>(dataObjects.Split("class=\"next\""));
            Int64 ResultsYahoo = Int64.Parse(LstStrings.Last().Split("span>")[1].Split(" ")[0].Replace(",", ""));
            return ResultsYahoo;
        }
        public static async Task<Int64> GetGoogleWebScrappingMethod1(String term)
        {
            //Adding user agent so that google page will return with the number of results
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1312.57 Safari/537.17");
            var response = await client.GetAsync("https://www.google.com/search?q=" + term);
            var dataObjects = response.Content.ReadAsStringAsync().Result;
            //Initial method for webscrapping
            String datos = dataObjects.Split("id=\"result-stats\">").Last().Split("resul")[0];
            Regex digitsOnly = new Regex(@"[^\d]");
            Int64 ResultsGoogle = Int64.Parse(digitsOnly.Replace(datos, ""));
            return ResultsGoogle;
        }
        #endregion

    }
}
