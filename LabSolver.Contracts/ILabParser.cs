using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabSolver.Contracts
{
    /// <summary>
    /// Used to parse labyrinths from different sources. In theory, it is possible that labyrinths will be recieved via TCP/IP or a similar mechanism.
    /// </summary>
    public interface ILabParser
    {
        /// <summary>
        /// Attempts to parse the labyrinths from the implementation-specific source.
        /// </summary>
        /// <returns>a LabResult object containing a collection of start nodes (or an error message telling us what went wrong during parsing) </returns>
        Task<LabResult<IEnumerable<ILabStartNode>>> Parse();
    }
}
