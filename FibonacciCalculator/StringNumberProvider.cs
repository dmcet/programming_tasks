using FibonacciCalculator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FibonacciCalculator
{
    /// <summary>
    /// Parses numbers from strings.
    /// </summary>
    public class StringNumberProvider : INumberProvider
    {
        private string source;

        /// <summary>
        /// Constructs a new StringNumberProvider.
        /// </summary>
        /// <param name="source">the source string</param>
        public StringNumberProvider(string source)
        {
            this.source = source;
        }

        public Task<IEnumerable<uint>> ParseNumbers()
        {
            try
            {
                return Task.FromResult<IEnumerable<uint>>(
                    source
                    .Split("\r\n") // Split at CRLF.
                    .Where(str => str.Length > 0) // Remove non-empty strings.
                    .Select(str => uint.Parse(str)) // Convert to uint.
                    .ToList()); // Use ToList() so that the query is performed once and not evaluated lazily.
            }   
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to parse numbers.", ex);
            }
        }
    }
}
