using CompilerWithFuzzy.GrammarFuzzy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.Compiler.Syn
{
    [DataContract]
    [Serializable]
    public class KeyDicCYK : IEquatable<KeyDicCYK>
    {


        [DataMember]
        public Symbol Symbol { get; set; }
        [DataMember]
        public int IndexI { get; set; }
        [DataMember]
        public int IndexJ { get; set; }
        
        public KeyDicCYK(Symbol sym, int i, int j)
        {
            this.Symbol = sym;
            this.IndexI = i;
            this.IndexJ = j;
        }
        
        public bool Equals(KeyDicCYK other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            return Symbol == other.Symbol && IndexI == other.IndexI && IndexJ == other.IndexJ;
        }
        public bool Equals(Symbol symbol, int i, int j)
        {
            return Symbol == symbol && IndexI == i && IndexJ == j;
        }
        
        public static bool operator ==(KeyDicCYK x, KeyDicCYK y)
        {
            if (Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null))
                return true;
            if (Object.ReferenceEquals(x, null))
            {
                return false;
            }
            return x.Equals(y);
        }
        public static bool operator !=(KeyDicCYK x, KeyDicCYK y)
        {

            return !x.Equals(y);
        }
        
        public override string ToString()
        {
            return string.Format(" [{0}  | {1} | {2}", Symbol, IndexI, IndexJ);
        }
    }
}
