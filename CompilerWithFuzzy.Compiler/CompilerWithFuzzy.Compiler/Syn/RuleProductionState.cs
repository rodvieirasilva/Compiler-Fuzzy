using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.GrammarFuzzy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.Compiler.Syn
{
    [DataContract]
    [Serializable]
    public class RuleProductionState : RuleProduction, IEquatable<RuleProductionState>
    {
        [DataMember]
        public int Pointer { get; set; }

        [DataMember]
        public Symbol Lookahead { get; set; }

        public RuleProductionState()
        {

            

        }


        public bool Equals(RuleProductionState other)
        {
            if (other == null)
                return false;
            return Pointer == other.Pointer && Lookahead == other.Lookahead && base.Equals(other);
        }

        public string HashCode
        {
            get; set;
        }

        public void CalculateHash()
        {
            HashCode = string.Format("{0}_{1}_{2}", this.Pointer, this.Lookahead, this.Id);
        }
    }
}
