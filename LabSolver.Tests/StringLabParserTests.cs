using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LabSolver.Tests
{
    public class StringLabParserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Hello World")]
        public async Task MustFailForInvalidInput(string input)
        {
            var parser = new StringLabParser(input);
            var parseResult = await parser.Parse().ConfigureAwait(false);
            Assert.False(parseResult.Success);
        }


        [Fact]
        public async Task MustReturnTwoStartNodes()
        {
            var exampleLabyrinth =
                "3 4 5\r\n" +
                "S....\r\n" +
                ".###.\r\n" +
                ".##..\r\n" +
                "###.#\r\n" +
                "\r\n" +
                "#####\r\n" +
                "#####\r\n" +
                "##.##\r\n" +
                "##...\r\n" +
                "\r\n" +
                "#####\r\n" +
                "#####\r\n" +
                "#.###\r\n" +
                "####E\r\n" +
                "\r\n" +
                "1 3 3\r\n" +
                "S##\r\n" +
                "#E#\r\n" +
                "###\r\n" +
                "\r\n" +
                "0 0 0";

            var parser = new StringLabParser(exampleLabyrinth);
            var parseResult = await parser.Parse().ConfigureAwait(false);

            // Parsing should pass.
            Assert.True(parseResult.Success);

            // Parsing should return 2 labyrinths.
            Assert.Equal(2, parseResult.Result.Count());
        }
    }
}