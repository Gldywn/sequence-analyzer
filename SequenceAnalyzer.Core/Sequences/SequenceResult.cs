using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core.Sequences
{
    public class SequenceResult
    {
        public Sequence Sequence { get; set; }
        public bool IsValid { get; set; }
        public Dictionary<Sequence, bool> TestedPermutations = new Dictionary<Sequence, bool>();

        public SequenceResult(Sequence sequence)
        {
            this.Sequence = sequence;
        }

        public SequenceResult(Sequence sequence, bool isValid, Dictionary<Sequence, bool> testedPermutations)
        {
            this.Sequence = sequence;
            this.IsValid = isValid;
            this.TestedPermutations = testedPermutations;
        }
    }
}
