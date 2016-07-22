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
    public class Simplification
    {
        [DataMember]
        public Grammar Simplified { get; set; }

        [DataMember]
        public Grammar Source
        {
            get; set;
        }
        [DataMember]
        public Grammar GrammarNoEmpty { get; set; }

        [DataMember]
        public Grammar GrammarNoUnitarianProductions { get; set; }

        [DataMember]
        public SymbolList VariablesEmpty { get; set; }

        [DataMember]
        public Dictionary<Symbol, SymbolList> Fasteners { get; set; }

        [DataMember]
        public List<string> FastenersString { get; set; }

        [DataMember]
        public SymbolList Visited
        {
            get; set;
        }

        [DataMember]
        public SymbolList VariablesCallTerminals;

        [DataMember]
        public SymbolList AcessiblesVariables;

        [DataMember]
        public SymbolList AcessiblesTerminals;

    
        public Simplification(Grammar source)
        {

            this.Source = source;
            Simplified = new Grammar();
            FillTableEmpty();
            EliminarProducoesVazio();
            FillClosures();
            EliminarProducoesUnitarias();
            FillVariablesAcessibleTerminals();
            FillAcessibles(GrammarNoUnitarianProductions);
            GenerateSimplified();
            if (VariablesEmpty.Contains(Simplified.VariableStart))
            {
                Simplified.AddRule(Simplified.VariableStart.Id, Symbol.EmptyTerminal);
            }

        }

        private void GenerateSimplified()
        {
            Simplified = new Grammar();

            Simplified.Terminals.AddRange(AcessiblesTerminals);
            Simplified.Variables.AddRange(AcessiblesVariables);
            Simplified.VariableStart = GrammarNoUnitarianProductions.VariableStart;
            Simplified.VariablesEmpty = VariablesEmpty;

            bool change = true;
            Simplified.Rules.AddRange(GrammarNoUnitarianProductions.Rules.Select(r => new RuleProduction() { Destiny = r.Destiny, Source = r.Source }));

            while (change)
            {
                change = false;
                if (Simplified.Terminals.RemoveAll(terminal => !AcessiblesTerminals.Contains(terminal)) > 0)
                {
                    change = true;
                }

                if (Simplified.Variables.RemoveAll(variavel => !AcessiblesVariables.Contains(variavel)) > 0)
                {
                    change = true;
                }

                for (int i = Simplified.Rules.Count - 1; i >= 0; i--)
                {
                    RuleProduction rAtual = Simplified.Rules[i];
                    if (!AcessiblesVariables.Contains(rAtual.Source)
                        || rAtual.Destiny.Exists
                        (simbolo =>
                            (
                                (simbolo.Terminal && !AcessiblesTerminals.Contains(simbolo)) ||
                                (!simbolo.Terminal && !AcessiblesVariables.Contains(simbolo))

                            )
                       )
                      )
                    {
                        change = true;
                        Simplified.Rules.Remove(rAtual);
                    }
                }

                FillAcessibles(Simplified);
            }
        }

        private void FillAcessibles(Grammar gramatica)
        {
            AcessiblesVariables = new SymbolList();
            AcessiblesTerminals = new SymbolList();

            Visited = new SymbolList();

            Queue<Symbol> variaveisAVisitar = new Queue<Symbol>();

            variaveisAVisitar.Enqueue(gramatica.VariableStart);
            AcessiblesVariables.Add(gramatica.VariableStart);
            while (variaveisAVisitar.Count > 0)
            {
                var variavelAtual = variaveisAVisitar.Dequeue();
                Visited.Add(variavelAtual);

                List<RuleProduction> regrasAtuais = gramatica.Rules.Where(r => r.Source == variavelAtual).ToList();

                foreach (var item in regrasAtuais)
                {

                    foreach (var itemDestino in item.Destiny)
                    {
                        if (itemDestino.Terminal)
                        {
                            if (!AcessiblesTerminals.Contains(itemDestino))
                                AcessiblesTerminals.Add(itemDestino);

                        }
                        else if (!Visited.Contains(itemDestino) && !AcessiblesVariables.Contains(itemDestino))
                        {
                            variaveisAVisitar.Enqueue(itemDestino);
                            AcessiblesVariables.Add(itemDestino);
                        }

                    }
                }
            }

        }

        private void FillVariablesAcessibleTerminals()
        {
            VariablesCallTerminals = new SymbolList();

            for (int i = 0; i < GrammarNoUnitarianProductions.Variables.Count; i++)
            {
                Visited = new SymbolList();

                Queue<Symbol> varsToVisit = new Queue<Symbol>();

                varsToVisit.Enqueue(GrammarNoUnitarianProductions.Variables[i]);

                while (varsToVisit.Count > 0)
                {
                    Symbol variavelAtual = varsToVisit.Dequeue();
                    Visited.Add(variavelAtual);

                    List<RuleProduction> regrasAtuais = GrammarNoUnitarianProductions.Rules.Where(r => r.Source == variavelAtual).ToList();

                    foreach (var item in regrasAtuais)
                    {

                        if (item.DestinyContainsTerminal())
                        {
                            VariablesCallTerminals.Add(Source.Variables[i]);
                            varsToVisit.Clear();
                            break;
                        }
                        else
                        {
                            foreach (var itemDestino in item.Destiny)
                            {
                                if (!Visited.Contains(itemDestino))
                                    varsToVisit.Enqueue(itemDestino);

                            }
                        }
                    }
                }
            }
        }

        private void FillClosures()
        {
            Fasteners = new Dictionary<Symbol, SymbolList>();
            FastenersString = new List<string>();

            for (int i = 0; i < GrammarNoEmpty.Rules.Count; i++)
            {
                RuleProduction rgAtual = GrammarNoEmpty.Rules[i];
                if (rgAtual.IsUnityVariable())
                {
                    if (Fasteners.ContainsKey(rgAtual.Source))
                    {
                        if (!Fasteners[rgAtual.Source].Contains(rgAtual.Destiny[0]))
                        {
                            Fasteners[rgAtual.Source].Add(rgAtual.Destiny[0]);
                        }
                    }
                    else
                    {
                        Fasteners.Add(rgAtual.Source, new SymbolList());
                        Fasteners[rgAtual.Source].Add(rgAtual.Destiny[0]);
                    }
                }
            }

            foreach (var item in Fasteners)
            {
                string fecho = "{";
                foreach (var charItem in item.Value)
                {
                    fecho += charItem.Name + ",";
                }
                FastenersString.Add(string.Format("Fecho({0}) = {1}}}", item.Key.Name, fecho.Trim(',')));
            }


        }

        private void EliminarProducoesUnitarias()
        {
            GrammarNoUnitarianProductions = new Grammar();
            GrammarNoUnitarianProductions.Terminals.AddRange(GrammarNoEmpty.Terminals);
            GrammarNoUnitarianProductions.Variables.AddRange(GrammarNoEmpty.Variables);
            for (int i = 0; i < GrammarNoEmpty.Rules.Count; i++)
            {
                RuleProduction rgAtual = GrammarNoEmpty.Rules[i];
                GrammarNoUnitarianProductions.AddRule(rgAtual.Source, rgAtual.Destiny.Copy());
            }
            bool trocou = true;
            while (trocou)
            {
                trocou = false;
                for (int i = GrammarNoUnitarianProductions.Rules.Count - 1; i >= 0; i--)
                {
                    RuleProduction rgAtual = GrammarNoUnitarianProductions.Rules[i];

                    if (rgAtual.IsUnityVariable())
                    {
                        var regras = GrammarNoUnitarianProductions.GetRules(rgAtual.FirstDestiny().Name);
                        foreach (var item in regras)
                        {
                            GrammarNoUnitarianProductions.AddRule(rgAtual.Source, item.Destiny.Copy());
                        }
                        trocou = true;
                        GrammarNoUnitarianProductions.Rules.Remove(rgAtual);
                    }
                }
            }
        }


        #region Eliminação de Produções vazias

        public void FillTableEmpty()
        {
            VariablesEmpty = new SymbolList();

            for (int i = 0; i < Source.Variables.Count; i++)
            {
                Visited = new SymbolList();

                Queue<Symbol> variaveisAVisitar = new Queue<Symbol>();

                variaveisAVisitar.Enqueue(Source.Variables[i]);

                while (variaveisAVisitar.Count > 0)
                {
                    Symbol variavelAtual = variaveisAVisitar.Dequeue();

                    Visited.Add(variavelAtual);
                    List<RuleProduction> regrasAtuais = Source.Rules.Where(r => r.Source == variavelAtual).ToList();

                    foreach (var item in regrasAtuais)
                    {

                        if (item.Destiny.Count == 1)
                        {
                            if (item.Destiny[0] == Symbol.EmptyTerminal)
                            {
                                VariablesEmpty.Add(Source.Variables[i]);
                                variaveisAVisitar.Clear();
                                break;
                            }

                            if (!item.Destiny[0].Terminal && !Visited.Contains(item.Destiny[0]))
                            {
                                variaveisAVisitar.Enqueue(item.Destiny[0]);
                            }

                        }
                    }
                }
            }
        }

  
        public void EliminarProducoesVazio()
        {
            GrammarNoEmpty = new Grammar();

            GrammarNoEmpty.Terminals.AddRange(Source.Terminals);
            GrammarNoEmpty.Variables.AddRange(Source.Variables);
            for (int i = 0; i < Source.Rules.Count; i++)
            {
                RuleProduction regraAtual = Source.Rules[i];
                Symbol orig = regraAtual.Source;

                List<SymbolList> destinos = new List<SymbolList>();

                if (Source.Rules[i].Destiny.Count == 1 && Source.Rules[i].Destiny[0] == Symbol.EmptyTerminal)
                {
                    //Do Nothing
                }
                else
                {

                    if (regraAtual.Destiny.Count > 0)
                    {
                        destinos.Add(regraAtual.Destiny);
                        for (int j = 0; j < destinos.Count; j++)
                        {
                            for (int k = 0; k < VariablesEmpty.Count; k++)
                            {
                                if (destinos[j].Contains(VariablesEmpty[k]))
                                {
                                    var destino = regraAtual.Destiny;

                                    while (destino.Contains(VariablesEmpty[k]))
                                    {
                                        destino = destino.RemoveFirst(VariablesEmpty[k]);

                                        if (destinos.FirstOrDefault(list => list == destino) == null
                                                && !destino.IsEmpty())
                                        {
                                            destinos.Add(destino);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    for (int j = 0; j < destinos.Count; j++)
                    {
                        var destino = destinos[j].Copy();
                        GrammarNoEmpty.AddRule(orig, destino);
                    }
                }
            }
        }
        #endregion Eliminação de Produções vazias

    }
}

