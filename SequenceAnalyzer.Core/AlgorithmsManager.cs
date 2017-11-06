using SequenceAnalyzer.Core.Algorithms;
using SequenceAnalyzer.Core.Exceptions;
using SequenceAnalyzer.Core.Reflection;
using SequenceAnalyzer.Core.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core
{
    public class AlgorithmsManager : Singleton<AlgorithmsManager>
    {
        private Dictionary<string, MethodInfo> algorithmsHandlers = new Dictionary<string, MethodInfo>();
        private static bool initialized = false;

        public void RegisterAssembly()
        {
            if (initialized) return;

            foreach (var t in Assembly.GetAssembly(typeof(AlgorithmsManager)).GetTypes())
            {
                foreach (var method in t.GetMethods())
                {
                    if (method.GetCustomAttribute<AlgorithmHandler>() != null)
                    {
                        this.algorithmsHandlers.Add(method.GetCustomAttribute<AlgorithmHandler>().Name, method);
                    }
                }
            }

            initialized = true;
        }

        public List<SequenceResult> ProcessEntry(string algorithm, List<string> sequences)
        {
            if (!string.IsNullOrEmpty(algorithm) && this.algorithmsHandlers.ContainsKey(algorithm))
            {
                return (List< SequenceResult>)this.algorithmsHandlers[algorithm].Invoke(null, new object[1] { sequences });
            }
            else
            {
                throw new AlgorithmNotFoundException(string.Format("Unable to find {0} in Core.Algorithms", algorithm));
            }
        }
    }
}
