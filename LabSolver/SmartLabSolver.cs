using LabSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSolver
{
    public class SmartLabSolver : ILabSolver
    {
        public async Task<LabResult<uint>> SolveLabyrinth(ILabStartNode startNode)
        {
            return await SolveSmart(default, startNode).ConfigureAwait(false);
        }

        private async Task<LabResult<uint>> SolveSmart(ILabNode lastNode, ILabNode nextNode)
        {
            // If the given node is the end node, we've solved the lab.
            if (nextNode is ILabEndNode)
            {
                return new LabResult<uint>
                {
                    Success = true,
                    Result = 0,
                };
            }

            // Remove lastNode from neighbours so that we don't go the same way multiple times.
            if (lastNode != null)
            {
                nextNode.Neighbours.Remove(lastNode);
            }

            // Otherwise, we must determine if we have non-stone neighbours.
            var maybeNonStoneNeighbours = nextNode.Neighbours.Where(node => node is ILabAirNode || node is ILabEndNode);

            // If there are no non-stone neighbours, we can't escape this way.
            if (!maybeNonStoneNeighbours.Any())
            {
                return new LabResult<uint>
                {
                    Success = false,
                    Result = 0,
                };
            }

            // Save the results.
            var results = new List<LabResult<uint>>();

            // Now, we must recursively continue our search.
            foreach (var neighbour in maybeNonStoneNeighbours)
            {
                results.Add((await SolveSmart(nextNode, neighbour).ConfigureAwait(false)));
            }

            var firstResult = results.FirstOrDefault(results => results.Success);
            if (firstResult != default)
            {
                firstResult.Result++;
                return firstResult;
            }

            return results.FirstOrDefault();
        }
    }
}
