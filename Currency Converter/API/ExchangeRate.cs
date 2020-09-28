using Currency_Converter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currency_Converter.API
{
    /// <summary>
    /// Represents an exchange rate from one currency to another.
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// The starting currency.
        /// </summary>
        public Currency From { get; set; }

        /// <summary>
        /// The destination currency.
        /// </summary>
        public Currency To { get; set; }

        /// <summary>
        /// The decimal to multiply the starting currency by to produce the equivelant value in the destination currency.
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// The date conversion rate is linked to.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Returns created from the two currencies.
        /// </summary>
        /// <param name="currencyFromStr">The starting currency, as a string.</param>
        /// <param name="currencyToStr">The destination currency, as a string.</param>
        /// <returns>A new <see cref="ExchangeRate"/> corresponding to the given parameters.</returns>
        public static decimal Get(string currencyFromStr, string currencyToStr)
        {
            var currencyFrom = currencyFromStr?.ToUpper().ParseEnum(Currency.UNDEFINED) ?? Currency.UNDEFINED;
            var currencyTo = currencyToStr?.ToUpper().ParseEnum(Currency.UNDEFINED) ?? Currency.UNDEFINED;
            return Get(currencyFrom, currencyTo);
        }

        /// <summary>
        /// Returns created from the two currencies.
        /// </summary>
        /// <param name="currencyFrom">The starting currency.</param>
        /// <param name="currencyTo">The destination currency.</param>
        /// <returns>A new <see cref="ExchangeRate"/> corresponding to the given parameters.</returns>
        public static decimal Get(Currency currencyFrom, Currency currencyTo) => ExchangeRatesAPI.GetRate(currencyFrom, currencyTo);
    }
}
