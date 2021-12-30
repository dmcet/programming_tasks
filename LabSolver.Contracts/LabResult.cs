using System;
using System.Collections.Generic;
using System.Text;

namespace LabSolver.Contracts
{
    /// <summary>
    /// Encapsulates the result of any kind of operation. 
    /// This is intentionally not designed as an interface as there is little sense to re-implement this objects, it's basically just a wrapper for a bool and some result object.
    /// Another option would be the use of a tuple. However, I would prefer an object in this case as its more verbose. However, this is just personal preference.
    /// </summary>
    public class LabResult<T>
    {
        /// <summary>
        /// Tells us whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// An information message for the user.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// This is the result of the given operation.
        /// </summary>
        public T Result { get; set; }
    }
}
