using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RiotSharp;

namespace ChiatoBot
{
    public static class ApiCalls
    {
        public static string APIKey = ""; //riot api key
        
        public static HttpClient RiotApiClient { get; set; }

        public static void InitializeClient()
        {
            RiotApiClient = new HttpClient();
            RiotApi.GetDevelopmentInstance(APIKey);
            //RiotApiClient.BaseAddress = new Uri("");
            RiotApiClient.DefaultRequestHeaders.Accept.Clear();
            RiotApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }

    public class RiotApiProcessor
    {
        public static async Task<LoadSummonerDataToRead> LoadSummonerData(string summonerName)
        {
            string url = ($"https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/{summonerName}?api_key={ApiCalls.APIKey}");

            using (HttpResponseMessage response = await ApiCalls.RiotApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    LoadSummonerDataToRead readData = await response.Content.ReadAsAsync<LoadSummonerDataToRead>();
                    return readData;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<string[]> LoadRankedData(string accountID)
        {
            string url = ($"https://euw1.api.riotgames.com/lol/league/v4/entries/by-summoner/{accountID}?api_key={ApiCalls.APIKey}");

            using (HttpResponseMessage response = await ApiCalls.RiotApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var readRankedData = await response.Content.ReadAsStringAsync();
                    string[] dataSplitted = readRankedData.Split(',', '"', ':');

                    return dataSplitted;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<TestClass> LoadMatchHistoryData(string accountID)
        {
            string url = ($"https://euw1.api.riotgames.com/lol/match/v4/matchlists/by-account/{accountID}?api_key={ApiCalls.APIKey}&endIndex=2");

            using (HttpResponseMessage response = await ApiCalls.RiotApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {

                    var readMHData = await response.Content.ReadAsStringAsync();
                    var resultData = AllChildren(JObject.Parse(readMHData))
                    .First(c => c.Type == JTokenType.Array && c.Path.Contains("matches"))
                    .Children<JObject>();
                    
                    foreach (JObject result in resultData)
                    {
                        var idOfGame = result.Value<String>("gameId");
                        var champPlayed = result.Value<int>("champion");

                        foreach (JProperty property in result.Properties())
                        {
                        }
                        
                    }
                    return resultData.Value<TestClass>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            static IEnumerable<JToken> AllChildren(JToken json)
            {
                foreach (var c in json.Children())
                {
                    yield return c;
                    foreach (var cc in AllChildren(c))
                    {
                        yield return cc;
                    }
                }
            }
        }



    }

    public class TestClass
    {
        public string gameId { get; set; }
    }

    public class LoadSummonerDataToRead
    {
        public string id { get; set; }
        public string accountId { get; set; }
    }



}
