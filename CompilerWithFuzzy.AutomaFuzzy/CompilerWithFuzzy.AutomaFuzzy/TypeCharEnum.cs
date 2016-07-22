using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.AutomaFuzzy
{
    [DataContract]
    [Serializable]

    public enum TypeCharEnum
    {
        [DataMember]
        Symbol,
        [DataMember]
        Loop,
        [DataMember]
        Interval,
        [DataMember]
        Final
    }
}
