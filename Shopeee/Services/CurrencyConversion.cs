using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shopeee.Class;

namespace Shopeee.Services
{
    class Rates
    {
        public static async Task<string> ConvertCurrency(string amount)
        {
            string EX_ACCESS_TOKEN = GlobalVariables.ExchangeRateKey;
            string EX_BASE_ADDRESS = GlobalVariables.ExchangeRateAPI;
            string FROM = "USD";
            string TO = GlobalVariables.Rate;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(EX_BASE_ADDRESS);

                var parametters = new Dictionary<string, string>
                {
                    {"from", FROM},
                    { "to", TO },
                    { "amount", amount },
                };
                var encodedContent = new FormUrlEncodedContent(parametters);

                var result = httpClient.GetAsync($"{EX_ACCESS_TOKEN}/pair/{FROM}/{TO}/{amount}");
                string returnValueFromAPI = await result.Result.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(returnValueFromAPI);
                string convertResultValue = json["conversion_result"].ToString();
                return convertResultValue;
                //var msg = result.EnsureSuccessStatusCode();
                //return await msg.Content.ReadAsStringAsync();
            }
        }
    }
}
