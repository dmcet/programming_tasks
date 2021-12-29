using LabSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabSolver.Nodes
{
    public class LabNodeBase : ILabNode
    {
        public IEnumerable<ILabNode> Neighbours { get; } = new LinkedList<ILabNode>();
    }
}
