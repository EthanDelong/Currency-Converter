using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currency_Converter.API
{
    public class ExchangeRatesAPIResponse
    {
        /// <summary>
        /// The input rates string from the api.
        /// </summary>
        [JsonProperty("rates")]
        public Dictionary<string, double> rates { get; set; }

        /// <summary>
        /// The input date string from the api.
        /// </summary>
        [JsonProperty("date")]
        public string date { get; set; }
    }
}
