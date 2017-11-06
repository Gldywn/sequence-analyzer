using SequenceAnalyzer.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core.Sequences
{
    public class Sequence
    {
        public List<Entry> Entries = new List<Entry>();
        public bool IsClone;

        public Sequence() { }

        public Sequence(List<Entry> entries)
        {
            this.Entries = entries;
        }

        public int[] Threads
        {
            get
            {
                return this.Entries.Select(x => x.Thread).Distinct().ToArray();
            }
        }

        public string Order
        {
            get
            {
                return string.Join("", this.Threads);
            }
        }

        public string[] Resources
        {
            get
            {
                return this.Entries.Select(x => x.Resource).Distinct().ToArray();
            }
        }

        public Dictionary<Entry, Entry> ReadsFrom
        {
            get
            {
                return SequenceUtilities.ExtractReadsFrom(this);
            }
        }

        public Dictionary<string, Entry> FinalWrites
        {
            get
            {
                return SequenceUtilities.ExtractFinalWrites(this);
            }
        }

        public List<string> PossiblePermutations
        {
            get
            {
                return SequenceUtilities.GetSequencePermutations(this.Threads.ToList(), this.Threads.Length);
            }
        }

        public Sequence Serialize(string permutation)
        {
            if (permutation.Length != this.Threads.Length)
                throw new InvalidPermutationException(string.Format("Permutation for this sequence must be {0} length", permutation.Length));

            List<Entry> permutatedEntries = new List<Entry>(this.Entries);
            permutatedEntries.Sort((x, y) =>
            {
                return permutation.IndexOf(x.Thread.ToString()) - permutation.IndexOf(y.Thread.ToString());
            });

            return new Sequence(permutatedEntries);
        }

        public override string ToString()
        {
            StringBuilder sequenceBuilder = new StringBuilder();

            for (int i = 0; i < this.Entries.Count; i++)
            {
                sequenceBuilder.Append(this.Entries[i].ToString());
                if (i + 1 != this.Entries.Count)
                    sequenceBuilder.Append(", ");
            }

            return sequenceBuilder.ToString();
        }
    }
}
