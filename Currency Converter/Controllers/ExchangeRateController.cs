using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Currency_Converter.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Currency_Converter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        /// <summary>
        /// Attempts to get the rate from the api.
        /// </summary>
        /// <param name="from">The from currency.</param>
        /// <param name="to">The to currency.</param>
        /// <returns>An <see cref="ExchangeRate"/> object representing the conversion between the two currencies.</returns>
        [HttpGet("convert")]
        public decimal Get(string from, string to) => ExchangeRate.Get(from, to);

        /// <summary>
        /// Gets the available currency selections.
        /// </summary>
        /// <returns>A list of available currency selections.</returns>
        [HttpGet("available")]
        public string[] Get() => Enum.GetValues(typeof(Currency)).Cast<Currency>().Skip(1).Select(currency => currency.ToString()).ToArray();
    }
}
