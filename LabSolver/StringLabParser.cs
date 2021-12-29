using LabSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LabSolver
{
    public class StringLabParser : ILabParser
    {
        private readonly Regex ParameterRegex = new Regex(@"(\d{1,2})\s(\d{1,2})\s(\d{1,2})");

        private readonly HashSet<char> AllowedCharacters =
            new HashSet<char> { '\r', '\n', '.', '#', 'S', 'E', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ' };

        private string labyrinth;

        public StringLabParser(string labyrinth)
        {
            this.labyrinth = labyrinth;
        }

        /// <summary>
        /// Performs some very rough validation of the input.
        /// </summary>
        /// <returns>a positive result if the input passes this first test, false otherwise. </returns>
        private Task<LabResult<IEnumerable<ILabStartNode>>> ValidateInput()
        {
          // Check for null. I prefer the default keyword over null.
            if (labyrinth == default)
            {
                return Task.FromResult(
                    new LabResult<IEnumerable<ILabStartNode>>()
                    {
                        Success = false,
                        Message = "null was passed.",
                    });
            }

            // Check if an empty IEnumerable was given.
            if (labyrinth.Length == 0)
            {
                return Task.FromResult(
                    new LabResult<IEnumerable<ILabStartNode>>()
                    {
                        Success = false,
                        Message = "Empty string was passed.",
                    });
            }

            // Check if there are any illegal symbols. One could probably design a regex for this task :-)
            if (labyrinth.Any(line => !AllowedCharacters.Contains(line)))
            {
                return Task.FromResult(
                new LabResult<IEnumerable<ILabStartNode>>()
                {
                    Success = false,
                    Message = "The given labyrinth contains illegal characters.",
                });
            }

            return Task.FromResult(new LabResult<IEnumerable<ILabStartNode>>() { Success = true });
        }

        public async Task<LabResult<IEnumerable<ILabStartNode>>> Parse()
        {
            try
            {
                var validationResult = await ValidateInput().ConfigureAwait(false);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                return await ConstructLabyrinth().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Any sort of exception results in a negative result.
                return new LabResult<IEnumerable<ILabStartNode>>
                { 
                    Message = $"Parsing the given labyrinth led to an exception: {ex.Message}",
                    Success = false,
                };
            }
        }

        private async Task<LabResult<IEnumerable<ILabStartNode>>> ConstructLabyrinth()
        {
            // Split string at CRLF (\r\n).
            var lines = labyrinth.Split("\r\n");

            // Each labyrinth must contain at least three rows (parameters, empty line and trailing zeros).
            // If there are less than three lines, something is really wrong with the format.
            if (lines.Length < 3)
            {
                return new LabResult<IEnumerable<ILabStartNode>>()
                {
                    Message = $"Expecting at least three rows, but found {lines.Length}.",
                    Success = false,
                };
            }

            var labyrinths = new List<ILabStartNode>();

            foreach (var line in lines)
            {
                if (!ParameterRegex.IsMatch(line))
                {
                    continue;
                }

                var groups = ParameterRegex.Match(line).Groups;

                var layerCount = int.Parse(groups[1].Value);
                var rowCount = int.Parse(groups[2].Value);
                var columnCount = int.Parse(groups[3].Value);

                if (layerCount == 0 && rowCount == 0 && columnCount == 0)
                {
                    break;
                }
            }



            return new LabResult<IEnumerable<ILabStartNode>>
            {
                Success = true,
                Message = $"{labyrinths.Count} labyrinths were found.",
                Result = labyrinths,
            };
        }
    }
}
