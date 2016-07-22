using CompilerWithFuzzy.AutomaFuzzy;
using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.GrammarFuzzy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.Compiler.Syn
{
    [DataContract]
    public class Operation : IEquatable<Operation>
    {
        private string[] enumNames = new string[] { "r", "s", "g", "a" };
        [DataMember]
        public State<Symbol> State { get; set; }
        [DataMember]
        public TypeOperation Type { get; set; }

        [DataMember]
        public Symbol Symbol { get; set; }

        [DataMember]
        public RuleProductionState Rule { get; set; }

        [DataMember]
        public double Pertinence { get; set; }

        public Operation()
        {
        }

        public Operation(State<Symbol> state, RuleProductionState rule)
        {
            this.State = state;
            this.Rule = rule;
        }

        public override string ToString()
        {
            string returns = enumNames[(int)Type];
            if (Type == TypeOperation.Reduce)
            {
                return string.Format("{0}({1})", returns, Rule.ToString());
            }
            return string.Format("{0}({1})", returns, State.Name);
        }

        public bool Equals(Operation other)
        {
            return this.Type == other.Type && this.Pertinence == other.Pertinence && this.ToString() == other.ToString();
        }
    }


    [Serializable]
    [DataContract]
    public enum TypeOperation
    {
        [DataMember]
        Reduce = 0,
        [DataMember]
        Shift = 1,
        [DataMember]
        Goto = 2,
        [DataMember]
        Acept = 3
    }
}
