using System;
using System.Threading.Tasks;

namespace LabSolver.Contracts
{
    /// <summary>
    /// Implements an algorithm to determine the amount of time it takes to leave a given labyrinth.
    /// </summary>
    public interface ILabSolver
    {
        /// <summary>
        /// Tries to solve the given labyrinth starting.
        /// </summary>
        /// <param name="startNode">the start node of this labyrinth</param>
        /// <returns>a LabResult object encapsulating the result.</returns>
        LabResult<uint> SolveLabyrinth(ILabStartNode startNode);
    }
}
