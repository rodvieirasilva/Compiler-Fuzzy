using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CompilerWithFuzzy.AutomaFuzzy.Rules
{
    [DataContract]
    [Serializable]
    public class SimpleExclusionRule<T> : AbstractRule<T>
    {
        [DataMember]
        public List<T> Inclusion { get; set; }
        [DataMember]
        public T Exclusion { get; set; }

        public SimpleExclusionRule()
        { }
        public SimpleExclusionRule(T exclusionRule, List<T> alphabet, double pertinence)
        {
            Inclusion = new List<T>();
            Inclusion = alphabet.Where(a => !a.Equals(exclusionRule)).ToList();
            this.Exclusion = exclusionRule;
            base.Pertinence = pertinence;
        }

        public override double Math(T c)
        {
            return Inclusion.Contains(c) ? Pertinence : 0;
        }

        public override string ToString()
        {
            return "{Alpha}-{" + Exclusion.ToString() + "}";
        }
    }
}
