using SequenceAnalyzer.Core.Sequences;
using SequenceAnalyzer.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core.Algorithms
{
    public static class ViewSerializabilityAlgorithm
    {
        [AlgorithmHandler("ViewSerializability")]
        public static List<SequenceResult> GetVerificationResults(List<string> sequences)
        {
            List<SequenceResult> verificationResults = new List<SequenceResult>();

            foreach (string sequenceAsString in sequences)
            {
                Sequence sequence = SequenceUtilities.GetSequenceFromString(sequenceAsString);
                SequenceResult result = new SequenceResult(sequence);

                int i = 0;
                bool correspondingSequence = false;

                while(i < sequence.PossiblePermutations.Count && !correspondingSequence)
                {
                    string currentPermutation = sequence.PossiblePermutations[i];
                    Sequence serializedSequence = sequence.Serialize(currentPermutation);

                    if (serializedSequence.ReadsFrom.DictionaryEqual(sequence.ReadsFrom)
                        && serializedSequence.FinalWrites.DictionaryEqual(sequence.FinalWrites))
                    {
                        correspondingSequence = true;
                        result.IsValid = true;
                    }

                    result.TestedPermutations.Add(serializedSequence, correspondingSequence);

                    i++;
                }

                verificationResults.Add(result);
            }

            return verificationResults;
        }
    }
}
