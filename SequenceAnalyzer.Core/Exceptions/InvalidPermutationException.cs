using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core.Exceptions
{
    public class InvalidPermutationException : Exception
    {
        public InvalidPermutationException() { }

        public InvalidPermutationException(string message) : base(message) { }
    }
}
