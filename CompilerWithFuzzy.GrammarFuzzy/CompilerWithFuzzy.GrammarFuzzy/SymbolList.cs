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
    [CollectionDataContractAttribute]
    public class SymbolList : List<Symbol>, ICloneable
    {

        public SymbolList()
        { }

        public SymbolList(params Symbol[] symbols)
            : this()
        {
            this.AddRange(symbols);
        }

        public static bool operator ==(SymbolList x, SymbolList y)
        {
            if (Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null))
            {
                return true;
            }
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null) || x.Count < 1 || y.Count < 1)
            {
                return false;
            }
            if (x.Count > 0 && x.Count == y.Count)
            {
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i] != y[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public static bool operator !=(SymbolList x, SymbolList y)
        {
            return !(x == y);
        }

        [DataMember]
        public bool Unitary
        {
            get
            {
                return base.Count == 1;
            }
        }
        public bool ContainsId(int id)
        {
            return !Object.ReferenceEquals(this.FirstOrDefault(s => s.Id == id), null);
        }

        public bool ContainsName(string name)
        {
            return this.FirstOrDefault(s => s.Name == name) != null;
        }
        public Symbol Find(char name)
        {
            return this.FirstOrDefault(s => s.Name == name + "");
        }

        public Symbol Find(int id)
        {
            return this.FirstOrDefault(s => s.Id == id);
        }

        public Symbol Find(string name)
        {
            return this.FirstOrDefault(s => s.Name == name);
        }

        public bool IsEmpty()
        {
            return base.Count < 1;
        }
        public SymbolList Replace(Symbol sym1, Symbol sym2)
        {
            SymbolList returns = new SymbolList();
            returns.AddRange(this.Select(s => s.Copy()));

            for (int i = 0; i < returns.Count; i++)
            {
                if (returns[i] == sym1)
                {
                    returns[i] = sym2.Copy();
                }
            }
            return returns;
        }

        public SymbolList ReplaceFirst(Symbol sym1, Symbol sym2)
        {
            SymbolList returns = new SymbolList();
            returns.AddRange(this.Select(s => s.Copy()));

            for (int i = 0; i < returns.Count; i++)
            {
                if (returns[i] == sym1)
                {
                    returns[i] = sym2.Copy();
                    return returns;
                }
            }
            return returns;
        }

        public SymbolList RemoveFirst(Symbol sym1)
        {
            SymbolList returns = new SymbolList();
            returns.AddRange(this.Select(s => s.Copy()));

            for (int i = 0; i < returns.Count; i++)
            {
                if (returns[i] == sym1)
                {
                    returns.RemoveAt(i);
                    return returns;
                }
            }
            return returns;
        }

        public SymbolList Add(Symbol s)
        {

            base.Add(s);
            return this;
        }

        public SymbolList AddRange(SymbolList s)
        {
            base.AddRange(s);
            return this;
        }

        public SymbolList AddRange(IEnumerable<Symbol> s)
        {
            base.AddRange(s);
            return this;
        }

        public object Clone()
        {
            SymbolList symbols = new SymbolList();
            foreach (var item in this)
            {
                symbols.Add(item.Copy());
            }
            return symbols;
        }

        public SymbolList Copy()
        {
            return (SymbolList)Clone();
        }


        public SymbolList SkipFirst()
        {
            SymbolList s = new SymbolList();
            s.AddRange(this.Skip(1));
            return s;
        }
    }
}
