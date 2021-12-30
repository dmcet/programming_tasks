using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciCalculator.Contracts
{
    /// <summary>
    /// Parses CRLF-separated numbers from any kind of source.
    /// </summary>
    public interface INumberProvider
    {
        /// <summary>
        /// Triggers the parsing.
        /// </summary>
        /// <returns>the parsed numbers.</returns>
        /// <exception cref="ArgumentException">if the string is malformed.</exception>
        Task<IEnumerable<uint>> ParseNumbers();
    }
}
