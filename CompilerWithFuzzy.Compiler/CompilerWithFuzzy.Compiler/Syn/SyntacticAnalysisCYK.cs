using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerWithFuzzy.GrammarFuzzy;
using CompilerWithFuzzy.AutomaFuzzy;
using CompilerWithFuzzy.AutomaFuzzy.Rules;
using System.Data;
using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.Compiler.Lex;
using System.Runtime.Serialization;

namespace CompilerWithFuzzy.Compiler.Syn
{
    [DataContract]
    [Serializable]
    public class SyntacticAnalysisCYK : SyntacticAnalysisAbstract
    {

        [DataMember]
        public List<RuleProduction>[,] CYKMatrix;

        [DataMember]
        public DataTable DataTable { get; set; }

        [DataMember]
        public Dictionary<KeyDicCYK, Node<Symbol, double>> DicCYK { get; set; }

        [DataMember]
        public GrammarFuzzy.Grammar GrammarChomsky { get; set; }

        public SyntacticAnalysisCYK() : base(null, null, null)
        { }
        public SyntacticAnalysisCYK(GrammarFuzzy.Grammar grammar) : base(grammar, null, null)
        {
            List<Symbol> alphabet = new List<Symbol>();
            alphabet.AddRange(grammar.Variables);
            alphabet.AddRange(grammar.Terminals);
            this.Grammar = grammar;
            // NormalFormChomsky nfc = new NormalFormChomsky(grammar);
            this.GrammarChomsky = grammar;
        }

        private void AddInDic(Symbol source, int i, int j, double pertinence, Symbol destiny, RuleProduction rule)
        {
            KeyDicCYK key = new KeyDicCYK(source, i, j);
            if (!DicCYK.ContainsKey(key))
            {
                DicCYK.Add(key, null);
            }
            DicCYK[key] = new Node<Symbol, double>(string.Empty, source);
            DicCYK[key].SetCustomOject("Prob", pertinence);
            DicCYK[key].SetCustomOject("Start", i);
            DicCYK[key].SetCustomOject("End", i);
            DicCYK[key].SetCustomOject("Destiny", destiny);
            DicCYK[key].SetCustomOject("Rule", rule);

        }

        //            function CYK-PARSE(sentence,grammar) return P, a chart. {
        public override double Validate(List<Symbol> tokens)
        {
            GraphsSyntactic = new List<Graph<Symbol, double>>();
            //1. S -> Noun VP   [1.0]
            //2. VP -> Verb Noun [0.5]
            //3. VP -> Modal Verb [0.5]
            //4. Modal -> can   [1]
            //5. Noun -> can    [0.3]
            //6. Noun -> fish   [0.3]
            //7. Noun -> people [0.4]
            //8. Verb -> can    [0.1]
            //9. Verb -> fish   [0.8]
            //8. Verb -> people [0.1]

            DicCYK = new Dictionary<KeyDicCYK, Node<Symbol, double>>();
            //1. N = length(sentence);
            //2. for (i = 1 to N) {
            for (int i = 0; i < tokens.Count; i++)
            {
                //3.   word = sentence[i];
                //  var rules = GrammarChomsky.Rules.Where(r => r);
                //4.    for (each rule  "POS --> Word [prob]" in the grammar) 
                foreach (var rule in GrammarChomsky.Rules)
                {
                    if (rule.IsUnitaryTerminal() && rule.FirstDestiny() == tokens[i])
                    {
                        //5.       P[POS,i,i] = new Tree(POS,i,i,word,null,null,prob);
                        var symbolDestiny = rule.FirstDestiny();
                        symbolDestiny.SetCustomValue("Token", tokens[i].GetCustomValue<Token>("Token"));
                        AddInDic(rule.Source, i, i, rule.Pertinence, symbolDestiny, rule);
                    }
                }
                //6.    }                           % endfor line 2.  
            }

            //7. for (length = 2 to N)          % length = length of phrase
            for (int length = 1; length < tokens.Count; length++)
            {
                //8.   for (i = 1 to N+1-length) {  % i == start of phrase
                for (int i = 0; i < tokens.Count - length; i++)
                {
                    //9.     j = i+length-1;            % j == end of phrase
                    int j = i + length;
                    //10.    for (each NonTerm M)  {
                    foreach (var variable in GrammarChomsky.Variables)
                    {
                        //11.        P[M,i,j] = new Tree(M,i,j,null,null,null,0.0);
                        AddInDic(variable, i, j, 0, null, null);
                        //12.        for (k = i to j-1)    % k = end of first subphrase
                        for (int k = i; k < j; k++)
                        {
                            var rules = GrammarChomsky.Rules.Where(r => r.Source == variable && !r.IsUnitary());
                            //13.            for (each rule "M -> Y,Z [prob]" in the grammar) {
                            foreach (var rule in rules)
                            {
                                //14.                newProb = P[Y,i,k].prob * P[Z,k+1,j].prob * prob;
                                double newProb = GetProb(rule.FirstDestiny(), i, k) * GetProb(rule.SecondDestiny(), k + 1, j) * rule.Pertinence;
                                //15.                if (newProb > P[M,i,j].prob) {
                                if (newProb > GetProb(variable, i, j))
                                {
                                    var node = GetNode(variable, i, j);
                                    node.Edges.Clear();
                                    //16.                   P[M,i,j].left = P[Y,i,k];                                    
                                    node.AddEdge(rule.Pertinence, GetNode(rule.FirstDestiny(), i, k), rule.Pertinence);
                                    //17.                   P[M,i,j].right = P[Z,k+1,j];
                                    node.AddEdge(rule.Pertinence, GetNode(rule.SecondDestiny(), k + 1, j), rule.Pertinence);
                                    //18.                   P[M,i,j].prob = newProb;
                                    node.SetCustomOject("Prob", newProb);

                                    node.SetCustomOject("Rule", rule);
                                    //19.                }  % endif line 15 
                                }
                                //20.            }      % endfor line 13
                            }
                        }

                        //21.      }            % endfor line 10
                    }

                    //22.    }              % endfor line 8
                }
                //23. return P;

                //24. }  
            }



            var nodeResult = GetNode(GrammarChomsky.VariableStart, 0, tokens.Count - 1);

            iToken = 0;
            FillTerminals(nodeResult, tokens);
            if (nodeResult != null)
            {
                GraphsSyntactic.Add(nodeResult.ToGraph());

                return GetProb(GrammarChomsky.VariableStart, 0, tokens.Count - 1);
            }
            return 0;
        }
        private int iToken;
        public void FillTerminals(Node<Symbol, double> nodeActual, List<Symbol> tokens)
        {
            if (nodeActual != null)
            {
                for (int j = 0; j < nodeActual.Edges.Count; j++)
                {
                    FillTerminals(nodeActual.Edges[j].Destiny, tokens);
                }

                if (iToken < tokens.Count)
                {
                    var first = GrammarChomsky.Rules.FirstOrDefault(r => r.Source == nodeActual.Info && r.FirstDestiny().Id == tokens[iToken].Id);
                    if (first != null)
                    {
                        Node<Symbol, double> nodeChild = new Node<Symbol, double>(tokens[iToken].Name, tokens[iToken]);
                        nodeActual.AddEdge(first.Pertinence, nodeChild, first.Pertinence);
                        iToken++;
                    }
                }
            }
        }

        public double GetProb(Symbol symbol, int i, int j)
        {
            var key = DicCYK.Keys.FirstOrDefault(k => k.Equals(symbol, i, j));
            if (key == null)
                return 0;
            return DicCYK[key].GetCustomOject<double>("Prob");
        }

        public Node<Symbol, double> GetNode(Symbol symbol, int i, int j)
        {
            var key = DicCYK.Keys.FirstOrDefault(k => k.Equals(symbol, i, j));
            if (key == null)
                return null;
            return DicCYK[key];
        }

        public override DataTable GetDataTable()
        {
            return null;
        }
    }

}
