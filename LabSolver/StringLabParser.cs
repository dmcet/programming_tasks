using LabSolver.Contracts;
using LabSolver.Nodes;
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

            // Initialize return value and temporary variables.
            var labyrinths = new List<ILabStartNode>();
            var currentLayers = new List<ILabNode[,]>();
            var currentLayerLines = new List<string>();

            var layerCount = -1;
            var rowCount = -1;
            var columnCount = -1;

            foreach (var line in lines)
            {
                // If the line matches the regex, this is a parameter line.
                if (ParameterRegex.IsMatch(line))
                {
                    // If we have found all layers of the current labyrinth, finish its processing and add it to the list of found labyrinths.
                    // Cleanup temporary variables.
                    if (currentLayers.Count == layerCount)
                    {
                        labyrinths.Add(await ConnectNodes(currentLayers).ConfigureAwait(false));
                        currentLayers.Clear();
                        currentLayerLines.Clear();
                    }

                    // Update the parameters.
                    // We know that these are valid integers due to the Regex, so no exception handling required when parsing.
                    var groups = ParameterRegex.Match(line).Groups;
                    layerCount = int.Parse(groups[1].Value);
                    rowCount = int.Parse(groups[2].Value);
                    columnCount = int.Parse(groups[3].Value);

                    // If all parameters are 0, we've reached the end of the input. Return everything we've parsed so far.
                    if (layerCount == 0 && rowCount == 0 && columnCount == 0)
                    {
                        break;
                    }
                }
                // Empty line means that the current layer is over, its lines can be processed.
                else if (line.Length == 0)
                {
                    currentLayers.Add(await ProcessLayerLines(currentLayerLines, rowCount, columnCount).ConfigureAwait(false));
                    currentLayerLines.Clear();
                }
                // Otherwise, add the line to the lines of the current layer for processing.
                else
                {
                    currentLayerLines.Add(line);
                }
            }

            return new LabResult<IEnumerable<ILabStartNode>>
            {
                Success = true,
                Message = $"{labyrinths.Count} labyrinths were found.",
                Result = labyrinths,
            };
        }

        private Task<ILabStartNode> ConnectNodes(List<ILabNode[,]> collectedLayers)
        {
            // A variable to memorize the start node of the labyrinth, this will be our return value.
            ILabStartNode startNode = default;

            // Extract the row and the column count from the first layer, all layers must have the same dimensions.
            var rowCount = collectedLayers.First().GetLength(0);
            var columnCount = collectedLayers.First().GetLength(1);

            // Initialize the nextLayer variable with the first layer.
            var nextLayer = collectedLayers.FirstOrDefault();

            for (var layer = 0; layer < collectedLayers.Count(); layer++)
            {
                // If there's no next layer, we've finished processing.
                if (nextLayer == null)
                {
                    break;
                }

                // Move nextLayer to current layer.
                // Get next layer from the list.
                // The Skip().FirstOrDefault() trick allows us to just retrieve null instead of throwing an Exception if there are no more layers.
                var currentLayer = nextLayer;
                nextLayer = collectedLayers.Skip(layer + 1).FirstOrDefault();

                for (var row = 0; row < rowCount; row++)
                {
                    for (var column = 0; column < columnCount; column++)
                    {
                        // Get the current node for easy access.
                        var currentNode = currentLayer[row, column];

                        // If the current node is the start node, memorize it.
                        if (currentNode is ILabStartNode labStartNode)
                        {
                            startNode = labStartNode;
                        }

                        // First, connect the nodes on currentLayer.
                        if (row > 0)
                        {
                            // top neighbour
                            currentNode.Neighbours.Add(currentLayer[row - 1, column]);
                        }

                        if (row < rowCount - 1)
                        {
                            // bottom neighbour.
                            currentNode.Neighbours.Add(currentLayer[row + 1, column]);
                        }

                        if (column > 0)
                        {
                            // left neighbour.
                            currentNode.Neighbours.Add(currentLayer[row, column - 1]);
                        }

                        if (column < columnCount - 1)
                        {
                            // right neighbour.
                            currentNode.Neighbours.Add(currentLayer[row, column + 1]);
                        }

                        // Also connect the currentLayer with the nextLayer (if there is a next layer).
                        if (nextLayer != null)
                        {
                            nextLayer[row, column].Neighbours.Add(currentLayer[row, column]);
                            currentLayer[row, column].Neighbours.Add(nextLayer[row, column]);
                        }
                    }
                }
            }

            return Task.FromResult(startNode);
        }

        private Task<ILabNode[,]> ProcessLayerLines(List<string> currentLayerLines, int rowCount, int columnCount)
        {
            ILabNode[,] layer = new ILabNode[rowCount, columnCount];

            // Iterate through all rows and columns.
            for (var row = 0; row < rowCount; row++)
            {
                var currentRow = currentLayerLines[row];

                for (var col = 0; col < columnCount; col++)
                {
                    // Convert the chars to objects.
                    layer[row, col] = currentRow[col] switch
                    {
                        'S' => new LabStartNode(),
                        '.' => new LabAirNode(),
                        '#' => new LabStoneNode(),
                        'E' => new LabEndNode(),
                        _ => throw new FormatException("Illegal character was found. This should not happen because the input was already checked.")
                    };
                }
            }

            return Task.FromResult(layer);
        }
    }
}
