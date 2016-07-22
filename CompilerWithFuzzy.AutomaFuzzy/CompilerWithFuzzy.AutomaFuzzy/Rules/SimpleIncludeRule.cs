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
    public class SimpleIncludeRule<T> : AbstractRule<T>
    {


        public SimpleIncludeRule()
        {

        }

        public SimpleIncludeRule(T symbol, double pertinence)
        {
            this.Symbol = symbol;
            base.Pertinence = pertinence;
        }

        public override double Math(T c)
        {
            if (Symbol == null && c == null)
                return Pertinence;
            return Symbol.Equals(c) ? Pertinence : 0;
        }

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }
}
