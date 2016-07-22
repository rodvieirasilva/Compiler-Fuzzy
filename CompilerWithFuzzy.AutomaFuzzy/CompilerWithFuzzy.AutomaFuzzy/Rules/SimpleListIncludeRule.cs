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
    public class SimpleListIncludeRule<T> : AbstractRule<T>
    {

        [DataMember]
        public List<T> Includes { get; set; }


        public SimpleListIncludeRule()
        { }
        public SimpleListIncludeRule(double pertinence, params T[] includes)
            : this(pertinence, includes.ToList())
        {

        }
        public SimpleListIncludeRule(double pertinence, List<T> includes)
        {
            this.Pertinence = pertinence;
            Includes = includes;
        }

        public override double Math(T c)
        {
            if (Includes.Contains(c))
            {
                return Pertinence;
            }
            return 0;
        }

        public override string ToString()
        {
            return "Any";
        }
    }
}
