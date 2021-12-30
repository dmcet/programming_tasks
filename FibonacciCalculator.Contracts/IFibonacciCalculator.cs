using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FibonacciCalculator.Contracts
{
    /// <summary>
    /// An interface for classes that calculate Fibonacci numbers.
    /// </summary>
    public interface IFibonacciCalculator
    {
        /// <summary>
        /// Calculates the Fibonacci number for the given number.
        /// </summary>
        /// <param name="number">the number for which to calculate the Fibonacci number.</param>
        /// <returns>the respective Fibonacci number.</returns>
        ulong CalculateFibonacciNumber(uint number);
    }
}
