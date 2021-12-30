using LabSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LabSolver.Tests
{
    public class SmartLabSolverTests
    {
        private ILabParser parser;

        public SmartLabSolverTests()
        {
            string exampleLabyrinth =
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

            parser = new StringLabParser(exampleLabyrinth);
        }

        [Fact]
        public async Task SolverMustSolveFirstExample()
        {
            var firstLab = (await parser.Parse().ConfigureAwait(false)).Result.First();

            var solver = new SmartLabSolver();
            var solveResult = solver.SolveLabyrinth(firstLab);

            Assert.True(solveResult.Success);

            Assert.Equal(11u, solveResult.Result);
        }

        [Fact]
        public async Task SolverMustSolveSecondExample()
        {
            var firstLab = (await parser.Parse().ConfigureAwait(false)).Result.ElementAt(1);

            var solver = new SmartLabSolver();
            var solveResult = solver.SolveLabyrinth(firstLab);

            Assert.False(solveResult.Success);
        }
    }
}
