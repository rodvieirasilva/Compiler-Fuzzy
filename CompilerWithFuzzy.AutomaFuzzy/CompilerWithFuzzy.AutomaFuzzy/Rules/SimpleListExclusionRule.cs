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
    public class SimpleListExclusionRule<T> : AbstractRule<T>
    {
        [DataMember]
        public List<T> Inclusion { get; set; }
        [DataMember]
        public List<T> Exclusion { get; set; }

        public SimpleListExclusionRule()
        { }
        public SimpleListExclusionRule(List<T> exclusionRule, List<T> alphabet, double pertinence)
        {
            Inclusion = new List<T>();
            Inclusion = alphabet.Where(a => !exclusionRule.Contains(a)).ToList();
            this.Exclusion = exclusionRule;
            base.Pertinence = pertinence;
        }
        
        public override double Math(T c)
        {
            return Inclusion.Contains(c) ? Pertinence : 0;
        }
        public override string ToString()
        {
            return string.Format("{{Alpha}} - {0}", Exclusion.Count);
        }
    }
}
