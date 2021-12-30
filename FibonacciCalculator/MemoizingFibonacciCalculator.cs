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

        public ulong CalculateFibonacciNumber(uint number)
        {
            // If the number is 0 or 1, return the number itself.
            if (number == 0 || number == 1)
            {
                return number;
            }

            // Otherwise, check if it was already calculated.
            if (!memory.ContainsKey(number))
            {
                // Check if the components of the number have already been calculated.
                if (!memory.ContainsKey(number - 1))
                {
                    memory[number - 1] = CalculateFibonacciNumber(number - 1);
                }

                if (!memory.ContainsKey(number - 2))
                {
                    memory[number - 2] = CalculateFibonacciNumber(number - 2);
                }

                // If not, calculate it.
                memory[number] = memory[number - 1] + memory[number - 2];
            }

            // In any case, return memory[number].
            return memory[number];
        }
    }
}
