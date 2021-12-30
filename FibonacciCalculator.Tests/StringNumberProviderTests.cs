using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FibonacciCalculator.Tests
{
    public class StringNumberProviderTests
    {
        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(99)]
        public async Task NumberProviderMustParseNumbers(int count)
        {
            var numberString = TestUtilities.GenerateNumberString(count);

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
    }
}