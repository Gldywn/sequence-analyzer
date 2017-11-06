using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core.Algorithms
{
    public class AlgorithmHandler : Attribute
    {
        public string Name { get; set; }

        public AlgorithmHandler(string name)
        {
            this.Name = name;
        }
    }
}
