using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.AutomaFuzzy.Rules
{
    [DataContract]
    [Serializable]
    public abstract class AbstractRule<T>
    {
        [DataMember]
        public T Symbol { get; set; }

        [DataMember]
        public double Pertinence { get; set; }


        public abstract double Math(T c);
    }
}
