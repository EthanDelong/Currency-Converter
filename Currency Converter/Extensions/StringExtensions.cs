using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currency_Converter.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Attempts to parse the string as an enum, otherwise returns the enum's default value.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="input">The input string to parse.</param>
        /// <returns>The parsed enum, or the default value.</returns>
        public static T ParseEnum<T>(this string input, T defaultValue = default) where T : struct, Enum => Enum.TryParse<T>(input, out T value) ? value : defaultValue;
    }
}
