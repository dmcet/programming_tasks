using System;
using System.Collections.Generic;
using System.Text;

namespace FibonacciCalculator.Contracts
{
    /// <summary>
    /// Parses CRLF-separated numbers from a string.
    /// </summary>
    public interface INumberParser
    {
        /// <summary>
        /// Parses CRLF-separated numbers.
        /// </summary>
        /// <param name="value">the string to parse the numbers from</param>
        /// <returns></returns>
        IEnumerable<uint> ParseNumbers(string value);
    }
}
