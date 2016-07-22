using CompilerWithFuzzy.GrammarFuzzy.Automa;
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
    public class Grammar
    {
        [DataMember]
        public SymbolList VariablesEmpty { get; set; }

        [DataMember]
        public SymbolList Variables { get; set; }
        [DataMember]
        public SymbolList Terminals { get; set; }

        [DataMember]
        public SymbolList Symbols
        {
            get
            {

                SymbolList symbols = new SymbolList();
                return symbols.AddRange(Variables).AddRange(Terminals);

            }
        }
        [DataMember]
        public List<RuleProduction> Rules { get; set; }

        [DataMember]
        public Symbol VariableStart { get; set; }
        public Grammar()
        {
            Variables = new SymbolList();
            Terminals = new SymbolList();
            VariablesEmpty = new SymbolList();
            Rules = new List<RuleProduction>();
            VariableStart = new Symbol(1, "S", false);
        }
        public RuleProduction AddRule(Symbol source, params Symbol[] destiny)
        {
            SymbolList sl = new SymbolList();
            sl.AddRange(destiny);
            return AddRule(source.Id, sl);
        }

        public RuleProduction AddRule(int idSource, params Symbol[] destiny)
        {
            SymbolList sl = new SymbolList();
            sl.AddRange(destiny);
            return AddRule(idSource, sl);
        }
        public RuleProduction AddRule(int idSource, SymbolList destiny)
        {
            return AddRule(Variables.Find(idSource), destiny);
        }
        public RuleProduction AddRule(Symbol source, SymbolList destiny)
        {
            if (Rules.Count == 0)
            {
                VariableStart = source;
            }
            RuleProduction rule = Rules.FirstOrDefault(r => r.Destiny == destiny && r.Source == source && r.Pertinence == 1);
            if (rule == null)
            {
                rule = new RuleProduction();
                rule.Source = source;
                rule.Destiny.AddRange(destiny.ToList());
                rule.Pertinence = 1;
                Rules.Add(rule);
            }
            return rule;
        }

        public List<RuleProduction> GetRules(Symbol symbol)
        {
            return GetRules(symbol.Name);
        }

        public List<RuleProduction> GetRules(string variableName)
        {
            return Rules.Where(c => c.Source.Name == variableName).ToList();
        }

        public bool ExistInRule(string destino)
        {
            return CapturarVariavel(destino) != Symbol.EmptyVariable;
        }


        public Symbol CapturarVariavel(string destino)
        {
            var regras = Rules.Where(c => c.DestinyToString() == destino);
            if (regras.Count() > 0)
                return regras.ElementAt(0).Source;
            return Symbol.EmptyVariable;
        }

        public Symbol CapturarVariavelExclusiva(Symbol destiny)
        {
            var origens = Rules.Where(c => c.Destiny.Unitary && c.Destiny[0] == destiny).Select(r => r.Source);
            foreach (var origem in origens)
            {
                if (GetRules(origem.Name).Count == 1)
                {
                    return origem;
                }
            }
            return Symbol.EmptyVariable;
        }

        public Symbol GetExclusiveVars(SymbolList destiny)
        {
            var origens = Rules.Where(c => c.Destiny == destiny).Select(r => r.Source);
            foreach (var origem in origens)
            {
                if (GetRules(origem.Name).Count == 1)
                {
                    return origem;
                }
            }
            return Symbol.EmptyVariable;
        }
    }
}
