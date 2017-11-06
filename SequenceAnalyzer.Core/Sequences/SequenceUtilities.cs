using SequenceAnalyzer.Core.Exceptions;
using SequenceAnalyzer.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core.Sequences
{
    public static class SequenceUtilities
    {
        public static Sequence GetSequenceFromString(string sequenceAsString)
        {
            var entriesAsString = sequenceAsString.Replace(" ", "").Split(',');
            if (!(entriesAsString.Count() > 0))
                throw new InvalidSequenceException(string.Format("Unable to find any entry in your sequence {0}", sequenceAsString));

            List<Entry> sequenceEntries = new List<Entry>();

            foreach (var entryAsString in entriesAsString)
            {
                InvalidEntryException invalidEntryException = new InvalidEntryException(string.Format("Unable to parse your entry {0}", entryAsString));

                try
                {
                    var entryBase = entryAsString.Split('(')[0];

                    var actionLetter = entryBase.Substring(0, 1)[0];
                    SequenceAction action = (actionLetter == 'r' || actionLetter == 'R') ? SequenceAction.Read : SequenceAction.Write;

                    var thread = int.Parse(entryBase.Split(actionLetter)[1]);
                    var resource = entryAsString.Split(')')[0].Split('(')[1].ToString();

                    Entry entry = new Entry(action, thread, resource);
                    if (entry.IsValidEntry())
                        sequenceEntries.Add(entry);
                    else
                        throw invalidEntryException;
                }
                catch(Exception) { throw invalidEntryException; }
            }

            if (sequenceEntries.Count > 0)
                return new Sequence(sequenceEntries);
            else
                throw new InvalidTimeZoneException(string.Format("Unable to parse any entry in your sequence {0}", sequenceAsString));
        }

        public static Dictionary<Entry, Entry> ExtractReadsFrom(Sequence sequence)
        {
            Dictionary<Entry, Entry> readsFrom = new Dictionary<Entry, Entry>();

            for (int i = 0; i < sequence.Entries.Count; i++)
            {
                if (i > 0)
                {
                    Entry entry = sequence.Entries[i];
                    if (entry.Action == SequenceAction.Read)
                    {
                        Entry lastFormerWriteEntry = null;

                        for(int y = 0; y < i; y++)
                        {
                            Entry _entry = sequence.Entries[y];
                            if (_entry.Action == SequenceAction.Write && _entry.Thread != entry.Thread
                                && _entry.Resource == entry.Resource)
                            {
                                lastFormerWriteEntry = _entry;
                            }
                        }

                        if (lastFormerWriteEntry != null)
                        {
                            readsFrom.Add(sequence.Entries[i], lastFormerWriteEntry);
                        }
                    }
                }
            }

            return readsFrom;
        }

        public static Dictionary<string, Entry> ExtractFinalWrites(Sequence sequence)
        {
            Dictionary<string, Entry> resFinalWrites = new Dictionary<string, Entry>();

            for (int i = 0; i < sequence.Entries.Count; i++)
            {
                Entry entry = sequence.Entries[i];
                if (entry.Action == SequenceAction.Write)
                {
                    resFinalWrites[entry.Resource] = entry;
                }
            }

            return resFinalWrites;
        }

        public static List<string> GetSequencePermutations(IEnumerable<int> threads, int count)
        {
            List<string> permutations = new List<string>();

            foreach (IEnumerable<int> permutation in PermuteUtilities.Permute<int>(threads, count))
            {
                string permutationAsString = null;

                foreach (int i in permutation)
                {
                    permutationAsString += i;
                }

                permutations.Add(permutationAsString);
            }

            return permutations;
        }
    }
}
