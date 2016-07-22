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
    public class NormalFormChomsky
    {
        [DataMember]
        public Grammar Normalized { get; set; }

        private Grammar simplified;

        [DataMember]
        public List<RuleProduction> NewRoles { get; set; }

        private SymbolList NewVariables { get; set; }

        Dictionary<Symbol, Symbol> DicVariablesTerminals;

        private int newsIds;
        public NormalFormChomsky(Grammar simplified)
        {
            newsIds = 1000;
            DicVariablesTerminals = new Dictionary<Symbol, Symbol>();
            NewRoles = new List<RuleProduction>();
            this.simplified = simplified;
            GenerateNewVars();
            Normalize();
        }

        private void GenerateNewVars()
        {
            Normalized = new Grammar();
            NewVariables = new SymbolList();
            foreach (var item in simplified.Terminals)
            {
                int newId = GetNewId();
                Symbol s = new Symbol(newId, string.Empty, false);
                NewVariables.Add(s);
                NewRoles.Add(Normalized.AddRule(newId, item));
                DicVariablesTerminals.Add(item, s);
            }
        }

        private void Normalize()
        {
            Normalized.VariableStart = simplified.VariableStart;
            Normalized.Terminals.AddRange(simplified.Terminals);
            Normalized.Variables.AddRange(simplified.Variables);

            for (int i = 0; i < simplified.Rules.Count; i++)
            {
                RuleProduction rgActual = simplified.Rules[i];

                var destiny = rgActual.Destiny;

                foreach (var item in DicVariablesTerminals)
                {
                    destiny = destiny.Replace(item.Key, item.Value);
                }

                if (destiny.Unitary)
                {
                    Normalized.AddRule(rgActual.Source, destiny);
                }
                else
                {
                    if (destiny.Count == 2 && !rgActual.DestinyContainsTerminal())
                    {
                        Normalized.AddRule(rgActual.Source, destiny);
                    }
                    else
                    {

                        List<SymbolList> destinys = new List<SymbolList>();

                        while (destiny.Count > 2)
                        {
                            destinys.Clear();
                            for (int k = 0; k < destiny.Count; k += 2)
                            {
                                if (k + 1 < destiny.Count)
                                {
                                    destinys.Add(new SymbolList(destiny[k], destiny[k + 1]));
                                }
                                else
                                {
                                    destinys.Add(new SymbolList(destiny[k]));
                                }
                            }

                            destiny = new SymbolList();
                            foreach (var des in destinys)
                            {
                                Symbol destinyVariable = des[0];
                                if (!des.Unitary)
                                {
                                    destinyVariable = Normalized.GetExclusiveVars(des);

                                    if (destinyVariable == Symbol.EmptyVariable)
                                    {
                                        destinyVariable = new Symbol(GetNewId(), string.Empty, false);
                                        Normalized.Variables.Add(destinyVariable);
                                        Normalized.AddRule(destinyVariable, des);
                                    }
                                }
                                destiny.Add(destinyVariable);
                            }

                        }
                        Normalized.AddRule(rgActual.Source, destiny);
                    }
                }
            }
        }

        private int GetNewId()
        {
            int nnewId = newsIds;
            while (Normalized.Terminals.ContainsId(nnewId) || Normalized.Variables.ContainsId(nnewId) ||
                simplified.Terminals.ContainsId(nnewId) ||
                simplified.Variables.ContainsId(nnewId) ||
                DicVariablesTerminals.Values.FirstOrDefault(vt => vt.Id == nnewId) != null)
            {

                newsIds++;
                nnewId = newsIds;
            }
            return nnewId;
        }
    }
}
