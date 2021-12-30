using System;
using System.Text;

namespace FibonacciCalculator.Tests
{
    /// <summary>
    /// Contains static helper methods for tests.
    /// I'm not a big fan of utility classes with static members in production code. However, this is fine for tests IMHO.
    /// </summary>
    internal static class TestUtilities
    {
        /// <summary>
        /// Generates a valid string with numbers.
        /// </summary>
        /// <param name="amount">the amount of numbers this string should contain.</param>
        /// <returns></returns>
        public static string GenerateNumberString(int amount)
        {
            var random = new Random();

            var stringBuilder = new StringBuilder();
            for (var index = 0; index < amount; index++)
            {
                stringBuilder.Append(random.Next(0, 5001)).Append("\r\n");
            }

            return stringBuilder.ToString();
        }
    }
}
