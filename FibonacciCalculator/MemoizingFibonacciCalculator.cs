using FibonacciCalculator.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FibonacciCalculator
{
    public class MemoizingFibonacciCalculator : IFibonacciCalculator
    {
        /// <summary>
        /// Memorizes already calculated Fibonacci numbers.
        /// </summary>
        private readonly Dictionary<uint, ulong> memory = new Dictionary<uint, ulong>();

        public async Task<ulong> CalculateFibonacciNumber(uint number)
        {
            // If the number is 0 or 1, return the number itself.
            if (number == 0 || number == 1)
            {
                return number;
            }

            // Otherwise, check if it was already calculated.
            if (!memory.ContainsKey(number))
            {
                // If not, calculate it.
                memory[number] = await CalculateFibonacciNumber(number - 1).ConfigureAwait(false) + await CalculateFibonacciNumber(number - 2).ConfigureAwait(false);
            }

            // In any case, return memory[number].
            return memory[number];
        }
    }
}
