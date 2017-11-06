using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core.Exceptions
{
    public class AlgorithmNotFoundException : Exception
    {
        public AlgorithmNotFoundException() { }

        public AlgorithmNotFoundException(string message) : base(message) { }
    }
}
