using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceAnalyzer.Core.Sequences
{
    public class Entry
    {
        public SequenceAction Action;
        public int Thread;
        public string Resource;

        public Entry(SequenceAction action, int thread, string resource)
        {
            this.Action = action;
            this.Thread = thread;
            this.Resource = resource;
        }

        public bool IsValidEntry()
        {
            return (this.Action != SequenceAction.Undefined && this.Thread > 0 && !string.IsNullOrEmpty(this.Resource));
        }

        public override string ToString()
        {
            return string.Format("{0}{1}({2})", (this.Action == SequenceAction.Read ? 'r' : 'w'), this.Thread, this.Resource);
        }
    }
}
