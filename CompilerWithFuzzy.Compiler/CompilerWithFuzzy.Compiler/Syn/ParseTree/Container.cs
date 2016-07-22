using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.Compiler.Syn.ParseTree
{
    [DataContract]
    [Serializable]
    public class Container : Dictionary<string, Container>
    {
        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        //TODO:
        [DataMember]
        public string ValueRecToken { get; set; }

        [DataMember]
        public int Line { get; set; }

        [DataMember]
        public int Column { get; set; }

        [DataMember]
        public Func<Container, string> Compile;


        public Container()
        {

        }
    }
}