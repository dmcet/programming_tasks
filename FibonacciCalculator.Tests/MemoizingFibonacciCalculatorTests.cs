using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace FibonacciCalculator.Tests
{    
    public class MemoizingFibonacciCalculatorTests
    {
        [Theory]
        [InlineData(5,5)]
        [InlineData(7,13)]
        [InlineData(11,89)]
        [InlineData(459, 15782647469193363394)]
        public void CalculatorMustCalculateProperFibonacciNumbersForExamples(uint number, ulong expected)
        {
            var calculator = new MemoizingFibonacciCalculator();

            Assert.Equal(expected, calculator.CalculateFibonacciNumber(number));
        }

        [Fact]
        public async Task CalculatorMustCalculateNumbersInUnder30Seconds()
        {
            var numberString = TestUtilities.GenerateNumberString(523);

            var numProvider = new StringNumberProvider(numberString);

            var numbers = await numProvider.ParseNumbers().ConfigureAwait(false);

            var calculator = new MemoizingFibonacciCalculator();

            var stopwatch = new Stopwatch();

            stopwatch.Start();
            foreach (var number in numbers)
            {
                calculator.CalculateFibonacciNumber(number);
            }
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds <= 30000);
        }
    }
}
