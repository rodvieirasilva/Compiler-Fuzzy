using CompilerWithFuzzy.AutomaFuzzy.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.AutomaFuzzy
{

    [DataContract]
    [Serializable]
    public class Transition<T>
    {
        [DataMember]
        public AbstractRule<T> Rule { get; set; }

        [DataMember]
        public State<T> From { get; set; }

        [DataMember]
        public State<T> To { get; set; }

      
        public Transition()
        {

        }

        public Transition(State<T> stateFrom, State<T> stateTo, AbstractRule<T> rule)
        {
            // TODO: Complete member initialization
            this.From = stateFrom;
            this.To = stateTo;
            this.Rule = rule;
        }

        
        public override string ToString()
        {
            return string.Format("({0})|{1}", Rule.ToString(), Rule.Pertinence);
        }
    }
}
