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
    public class NormalFormGreibach
    {
        [DataMember]
        public Grammar Normalized { get; set; }

        private Grammar simplified;
        [DataMember]
        public Dictionary<Symbol, Symbol> NewNames { get; set; }

        [DataMember]
        public List<RuleProduction> RolesArToGreatAs { get; set; }

        [DataMember]
        public List<RuleProduction> RolesSubstituiArGreatAs { get; set; }

        [DataMember]
        public List<RuleProduction> RolesReplaceArParaAr { get; set; }

        [DataMember]
        public List<RuleProduction> RolesArToAr { get; set; }

        private int newsIds;

        public NormalFormGreibach(Grammar simplified)
        {
            Normalized = new Grammar();
            newsIds = 100;
            this.simplified = simplified;
            //2
            FillNewsNames();
            //3,4
            PreencherRegrasArParaArEArParaMaiorAr();
            //5
            ColocarTerminalInicio();
            //6
            RemoveTerminalsNoMeio();

        }

        private void ColocarTerminalInicio()
        {
            for (int i = 0; i < Normalized.Rules.Count; i++)
            {
                RuleProduction rAtual = Normalized.Rules[i];

                if (!rAtual.Destiny[0].Terminal)
                {
                    //Removi regra volta regra
                    Normalized.Rules.Remove(rAtual);
                    i--;

                    var destino = rAtual.SkipFirstDestiny();

                    List<RuleProduction> regras = Normalized.GetRules(rAtual.FirstDestiny());

                    for (int j = 0; j < regras.Count; j++)
                    {
                        RuleProduction regraTemp = Normalized.AddRule(rAtual.Source, regras[j].Destiny.Copy().AddRange(destino));
                    }
                }

            }
        }


        //Removendo terminais no meio
        private void RemoveTerminalsNoMeio()
        {
            for (int i = 0; i < Normalized.Rules.Count; i++)
            {
                RuleProduction rAtual = Normalized.Rules[i];

                for (int j = 1; j < rAtual.Destiny.Count; j++)
                {
                    if (rAtual.Destiny[j].Terminal)
                    {
                        var variavelExclusiva = Normalized.CapturarVariavelExclusiva(rAtual.Destiny[j]);

                        if (variavelExclusiva == Symbol.EmptyVariable)
                        {
                            variavelExclusiva = new Symbol(GetNewId(), string.Empty, false);
                            Normalized.Variables.Add(variavelExclusiva);
                            Normalized.AddRule(variavelExclusiva, rAtual.Destiny);
                        }

                        rAtual.Destiny[j] = variavelExclusiva;
                    }
                }
            }
        }

        private void PreencherRegrasArParaArEArParaMaiorAr()
        {
            RolesArToGreatAs = new List<RuleProduction>();
            RolesArToAr = new List<RuleProduction>();
            RolesSubstituiArGreatAs = new List<RuleProduction>();
            RolesReplaceArParaAr = new List<RuleProduction>();

            for (int i = 0; i < Normalized.Rules.Count; i++)
            {
                RuleProduction rAtual = Normalized.Rules[i];
                var destino = rAtual.Destiny.Copy();
                if (rAtual.IsFirstVariable())
                {
                    if (NewNames.ContainsKey(rAtual.Source))
                    {
                        if (NewNames[rAtual.Source].Id > NewNames[rAtual.FirstDestiny()].Id)
                        {

                            Normalized.Rules.Remove(rAtual);
                            i--;

                            List<RuleProduction> regrasDestino = Normalized.GetRules(rAtual.FirstDestiny());
                            RolesArToGreatAs.Add(rAtual);
                            //Remoção de Regra, decrementa i

                            for (int j = 0; j < regrasDestino.Count; j++)
                            {
                                RuleProduction regraTemp = Normalized.AddRule(rAtual.Source,
                                            regrasDestino[j].Destiny.AddRange(rAtual.SkipFirstDestiny()));
                                RolesSubstituiArGreatAs.Add(regraTemp);
                            }

                        }
                        else if (NewNames[rAtual.Source].Name == NewNames[rAtual.FirstDestiny()].Name)
                        {
                            RolesArToAr.Add(rAtual);
                            var bAtual = new Symbol(GetNewId(), string.Empty, false);
                            Normalized.Variables.Add(bAtual);
                            destino = destino.SkipFirst();
                            RuleProduction regraTemp = Normalized.AddRule(bAtual, destino);
                            RolesReplaceArParaAr.Add(regraTemp);
                            regraTemp = Normalized.AddRule(bAtual, destino.Add(bAtual));
                            RolesReplaceArParaAr.Add(regraTemp);

                            //Removi volta o i
                            Normalized.Rules.Remove(rAtual);
                            i--;

                            List<RuleProduction> regrasOrigem = Normalized.GetRules(rAtual.Source);

                            for (int j = 0; j < regrasOrigem.Count; j++)
                            {
                                regraTemp = Normalized.AddRule(rAtual.Source, regrasOrigem[j].Destiny.Add(bAtual));
                                RolesReplaceArParaAr.Add(regraTemp);
                            }
                        }
                    }
                }
            }
        }

        private int GetNewId()
        {
            int nnewId = newsIds;
            while (Normalized.Terminals.ContainsId(nnewId) || Normalized.Variables.ContainsId(nnewId) ||
                simplified.Terminals.ContainsId(nnewId) ||
                simplified.Variables.ContainsId(nnewId))
            {

                newsIds++;
                nnewId = newsIds;
            }
            return nnewId;
        }

        private void RemoverArParaMenorAr()
        {

        }

        private void FillNewsNames()
        {
            NewNames = new Dictionary<Symbol, Symbol>();

            SymbolList visitados = new SymbolList();

            Queue<Symbol> variaveisAVisitar = new Queue<Symbol>();

            variaveisAVisitar.Enqueue(simplified.VariableStart);

            int i = 1;
            NewNames.Add(simplified.VariableStart, new Symbol(GetNewId(), string.Format("A{0:000}", i), false));
            visitados.Add(simplified.VariableStart);
            while (variaveisAVisitar.Count > 0)
            {
                var variavelAtual = variaveisAVisitar.Dequeue();

                List<RuleProduction> regrasAtuais = simplified.GetRules(variavelAtual);

                foreach (var item in regrasAtuais)
                {
                    foreach (var itemDestino in item.Destiny)
                    {
                        if (!itemDestino.Terminal && !visitados.Contains(itemDestino))
                        {
                            variaveisAVisitar.Enqueue(itemDestino);
                            i++;
                            NewNames.Add(itemDestino, new Symbol(GetNewId(), string.Format("A{0:000}", i), false));
                            visitados.Add(itemDestino);
                        }
                    }
                }
            }

            Normalized = new Grammar();
            Normalized.VariableStart = simplified.VariableStart;
            Normalized.Variables.AddRange(simplified.Variables);
            Normalized.Terminals.AddRange(simplified.Terminals);
            Normalized.VariablesEmpty = simplified.VariablesEmpty;
            for (i = 0; i < simplified.Rules.Count; i++)
            {
                Normalized.AddRule(simplified.Rules[i].Source, simplified.Rules[i].Destiny.Copy());
            }
        }

        public string[] GetNewsNames()
        {
            List<string> retorno = new List<string>();

            foreach (var item in NewNames)
            {
                retorno.Add(string.Format("{0} = {1}", item.Key.Name, item.Value.Name));
            }

            return retorno.ToArray();
        }

        public string[] GetProducoesNovosNomes()
        {
            return SubstituiRegras(Normalized.Rules);
        }

        public string[] GetRegrasArParaMaiorAs()
        {
            return SubstituiRegras(RolesArToGreatAs);
        }

        public string[] GetRegrasSubstituiArMaiorAs()
        {
            return SubstituiRegras(RolesSubstituiArGreatAs);
        }
        public string[] GetRegrasSubstituiArParaAr()
        {
            return SubstituiRegras(RolesReplaceArParaAr);
        }

        public string[] GetRegrasArParaAr()
        {
            return SubstituiRegras(RolesArToAr);
        }


        private string[] SubstituiRegras(List<RuleProduction> regras)
        {
            List<string> retorno = new List<string>();
            foreach (var item in regras)
            {
                var novoNome = item.Source;
                if (NewNames.ContainsKey(item.Source))
                {
                    novoNome = NewNames[item.Source];
                }

                SymbolList destino = item.Destiny.Copy();

                foreach (var novos in NewNames)
                {

                    destino = destino.Replace(novos.Key, novos.Value);
                }

                retorno.Add(string.Format("{0} => {1}", novoNome, destino));
            }

            return retorno.ToArray();
        }
    }
}
