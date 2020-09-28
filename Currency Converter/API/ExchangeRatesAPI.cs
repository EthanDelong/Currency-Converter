using Currency_Converter.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Currency_Converter.API
{
    /// <summary>
    /// Communicates with https://exchangeratesapi.io/ to provide exchange rates.
    /// </summary>
    public class ExchangeRatesAPI : IDisposable
    {
        /// <summary>
        /// The current instance.
        /// </summary>
        private static readonly ExchangeRatesAPI instance = new ExchangeRatesAPI();

        /// <summary>
        /// The client to make api requests from.
        /// </summary>
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// The cache of exchange rates.
        /// </summary>
        private static readonly Dictionary<Currency, Dictionary<Currency, ExchangeRate>> rateCache = new Dictionary<Currency, Dictionary<Currency, ExchangeRate>>();

        /// <summary>
        /// Locking for cache read/writes.
        /// </summary>
        private static readonly object cacheLock = new object();

        /// <summary>
        /// Attempts to get the latest exchange rate.
        /// </summary>
        /// <param name="from">The from currency.</param>
        /// <param name="to">The to currency.</param>
        /// <returns></returns>
        public static decimal GetRate(Currency from, Currency to)
        {
            try
            {
                if (from != Currency.UNDEFINED && to != Currency.UNDEFINED)
                {
                    lock (cacheLock)
                    {
                        // Update the cache
                        if (!rateCache.ContainsKey(from) || !rateCache[from].ContainsKey(to) || rateCache[from][to].Date < DateTime.Today)
                        {
                            var ratesTask = GetBaseRates(from);
                            ratesTask.Wait(5000);

                            rateCache[from] = ratesTask.Result;
                        }

                        return rateCache[from][to].Rate;
                    }
                }
            }
            catch (Exception ex)
            {
                // Issue either getting the cache or with the api request, so eat this and return blanks.
                Debug.WriteLine(ex.ToString());
            }

            // If we were unable to get any rates, return an empty value.
            return 0m;
        }

        /// <summary>
        /// The url for for api.
        /// </summary>
        private const string apiUrl = "https://api.exchangeratesapi.io";

        /// <summary>
        /// Gets all the rates from the base currency specified.
        /// https://api.exchangeratesapi.io/latest?base={baseCurrency}
        /// </summary>
        /// <param name="baseCurrency">The base currency.</param>
        private async static Task<Dictionary<Currency, ExchangeRate>> GetBaseRates(Currency baseCurrency)
        {
            var jsonResult = await instance.client.GetStringAsync($"{apiUrl}/latest?base={baseCurrency}");

            var result = JsonSerializer.Deserialize<ExchangeRatesAPIResponse>(jsonResult);

            var rates = result.rates;
            var date = DateTime.TryParse(result.date, out DateTime value) ? value : DateTime.MinValue;

            return rates.ToDictionary(item => item.Key.ParseEnum<Currency>(), item => new ExchangeRate
            {
                From = baseCurrency,
                To = item.Key.ParseEnum<Currency>(),
                Rate = (decimal)item.Value,
                Date = date
            });
        }

        /// <summary>
        /// Whether we are currently disposing.
        /// </summary>
        private bool isDisposing = false;

        /// <summary>
        /// The lock for disposing.
        /// </summary>
        private readonly object disposeLock = new object();

        /// <summary>
        /// Dispose of the httpclient.
        /// </summary>
        public void Dispose()
        {
            lock (disposeLock)
            {
                if (isDisposing)
                {
                    return;
                }

                isDisposing = true;
            }

            client.Dispose();
        }
    }
}
