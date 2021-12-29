using System;
using System.Collections.Generic;
using System.Text;

namespace LabSolver.Contracts
{
    /// <summary>
    /// Symbolizes one single labyrinth node.
    /// </summary>
    public interface ILabNode
    {
        /// <summary>
        /// Contains this nodes' adjacent nodes.
        /// </summary>
        public List<ILabNode> Neighbours { get; }
    }
}
