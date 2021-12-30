using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FibonacciCalculator.Tests
{
    public class StringNumberProviderTests
    {
        private Random random = new Random();

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(99)]
        public async Task NumberProviderMustParseNumbers(int count)
        {
            var numberString = GenerateNumberString(count);

            var numProvider = new StringNumberProvider(numberString);

            var numbers = await numProvider.ParseNumbers().ConfigureAwait(false);

            // Must be same amount of numbers.
            Assert.Equal(count, numbers.Count());

            // Test first entry.
            Assert.Equal(numberString.Split("\r\n")[0], numbers.First().ToString());
        }

        [Fact]
        public async Task NumberProviderMustThrowIfInvalidString()
        {
            var numProvider = new StringNumberProvider("ThisIsSomeInvalidString");

            await Assert.ThrowsAsync<ArgumentException>(async () => await numProvider.ParseNumbers().ConfigureAwait(false));
        }

        /// <summary>
        /// Generates a valid string with numbers.
        /// </summary>
        /// <param name="amount">the amount of numbers this string should contain.</param>
        /// <returns></returns>
        private string GenerateNumberString(int amount)
        {
            var stringBuilder = new StringBuilder();
            for (var index = 0; index < amount; index++)
            {
                stringBuilder.Append(random.Next(0, 5001)).Append("\r\n");
            }
            return stringBuilder.ToString();
        }
    }
}