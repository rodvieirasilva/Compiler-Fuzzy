using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
namespace CompilerWithFuzzy.GrammarFuzzy
{
    [DataContract]
    [Serializable]
    public class RuleProduction : IEquatable<RuleProduction>
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public bool Default { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public RuleProduction Parent { get; set; }
        [DataMember]
        public Symbol Source { get; set; }
        [DataMember]
        public SymbolList Destiny { get; set; }

        [DataMember]
        public double Pertinence { get; set; }

        public RuleProduction()
        {
            Destiny = new SymbolList();
            Pertinence = 0;
        }
        public RuleProduction(Symbol source, SymbolList destiny, double pertinence = 1)
        {
            Source = source;
            Destiny = destiny;
            this.Pertinence = pertinence;
        }

        public override string ToString()
        {
            return Source.Name + "=>" + string.Join(" ", Destiny.Select(d => d.Name));
        }


        public bool IsUnityVariable()
        {
            return Destiny.Count == 1 && !Destiny[0].Terminal;
        }

        public bool IsFirstVariable()
        {
            return Destiny.Count > 0 && !Destiny[0].Terminal;
        }
        public bool IsFirstTerminal()
        {
            return Destiny.Count > 0 && Destiny[0].Terminal;
        }

        public bool IsUnitaryTerminal()
        {
            return Destiny.Count == 1 && Destiny[0].Terminal;
        }

        public string DestinyToString()
        {
            return string.Join("", Destiny.Select(d => d.Name));
        }

        public Symbol FirstDestiny()
        {
            return Destiny[0];
        }

        public Symbol SecondDestiny()
        {
            return Destiny[1];
        }


        public SymbolList SkipFirstDestiny()
        {
            return Destiny.SkipFirst();
        }
        public bool DestinyContainsTerminal()
        {

            foreach (var item in Destiny)
            {
                if (item.Terminal)
                    return true;
            }

            return false;
        }
        public bool Equals(RuleProduction other)
        {
            return other != null && Source == other.Source && Destiny == other.Destiny;
        }


        public bool IsUnitary()
        {
            return Destiny.Count == 1;
        }
    }
}

