using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerWithFuzzy.GrammarFuzzy;
using System.Runtime.Serialization;
using System.Collections;

namespace CompilerWithFuzzy.Compiler.Syn
{
    [DataContract]
    [Serializable]
    public class ValueState : IEquatable<ValueState>
    {
        [DataMember]
        public List<RuleProductionState> Rules { get; set; }




        public Hashtable HashCodeRules
        {
            get; set;
        }

        public ValueState()
        {
            Rules = new List<RuleProductionState>();
            HashCodeRules = new Hashtable();
        }

        public void AddRule(RuleProductionState rule)
        {
            Rules.Add(rule);
            HashCodeRules.Add(rule.HashCode, true);
        }

        public bool Equals(ValueState other)
        {
            if (Rules.Count != other.Rules.Count)
                return false;

            foreach (var key in other.HashCodeRules.Keys)
            {
                if (!HashCodeRules.ContainsKey(key))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            string str = string.Empty;
            foreach (var r in this.Rules)
            {
                string dot = "•";
                string strRule = string.Empty;
                for (int i = 0; i < r.Destiny.Count; i++)
                {
                    if (i == r.Pointer)
                    {
                        strRule += dot;
                    }
                    strRule += r.Destiny[i].Name;
                }
                if (r.Destiny.Count == r.Pointer)
                {
                    strRule += dot;
                }
                str = string.Format("{0}[{1},{2}]\r\n", str, strRule, r.Lookahead);
            }
            return str.Trim('\n').Trim('\r');
        }
    }
}
