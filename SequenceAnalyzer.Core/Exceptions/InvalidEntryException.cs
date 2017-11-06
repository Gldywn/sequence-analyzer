using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core.Exceptions
{
    public class InvalidEntryException : Exception
    {
        public InvalidEntryException() { }

        public InvalidEntryException(string message) : base(message) { }
    }
}
