using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.GrammarFuzzy;
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
    public class NodeToTreeContainer
    {
        [DataMember]
        public Container ContainerResult { get; set; }

        [DataMember]
        public Dictionary<int, List<string>> NamesVars { get; set; }

        [DataMember]
        public List<Symbol> Symbols { get; set; }

        public NodeToTreeContainer()
        { }
        public NodeToTreeContainer(Node<Symbol, double> node, Dictionary<int, List<string>> namesVars, List<Symbol> symbols)
        {
            this.NamesVars = namesVars;
            this.Symbols = symbols;
            var rule = node.GetCustomOject<RuleProduction>("Rule");
            ContainerResult = CreateContainer(node);
            ContainerResult.Name = rule.Source.Name;
            CreateTreeContainer(ContainerResult, node);
        }

        public void CreateTreeContainer(Container parent, Node<Symbol, double> nodeParent)
        {
            RuleProduction rule = null;

            // if (prod < Productions.Count)
            {
                rule = nodeParent.GetCustomOject<RuleProduction>("Rule");

                if (nodeParent.Edges.Count > 0)
                {

                    for (int i = nodeParent.Edges.Count - 1; i >= 0; i--)
                    {
                        var transition = nodeParent.Edges[i];


                        //if (!DestinyIsTerminal(transition.Destiny))
                        //    prod++;

                        var next = CreateContainer(transition.Destiny);

                        //rule = transition.Destiny.GetCustomOject<RuleProduction>("Rule");

                        next.Name = NamesVars[rule.Id][i];
                        if (!parent.ContainsKey(next.Name))
                        {
                            parent.Add(next.Name, next);

                        }
                        else
                        {
                            parent[next.Name] = next;
                        }

                        CreateTreeContainer(next, transition.Destiny);
                    }
                }
                else
                {
                    if (nodeParent.CustomOject.ContainsKey("Destiny"))
                    {
                        var next = CreateContainer(nodeParent.GetCustomOject<Symbol>("Destiny"));
                        next.Name = NamesVars[nodeParent.GetCustomOject<RuleProduction>("Rule").Id][0];
                        if (!parent.ContainsKey(next.Name))
                        {
                            parent.Add(next.Name, next);

                        }
                        else
                        {
                            parent[next.Name] = next;
                        }

                        //parent.Add(next.Name, next);
                    }
                }

            }
        }

        private bool DestinyIsTerminal(Node<Symbol, double> node)
        {
            return node.Info != null && node.Info.Terminal;
        }

        private Container CreateContainer(Node<Symbol, double> node)
        {
            RuleProduction rule = node.GetCustomOject<RuleProduction>("Rule");
            Container container = new Container();
            container.Value = string.Empty;
            if (DestinyIsTerminal(node))
            {
                var token = node.Info.GetCustomValue<Token>("Token");
                RecognitionToken recToken = null;
                if (token != null)
                {
                    recToken = token.RecToken;
                    container.Value = token.Word;
                    container.Column = token.Collumn;
                    container.Line = token.Line;
                }
                if (recToken == null)
                {
                    recToken = node.Info.GetCustomValue<RecognitionToken>("RecToken");
                }


                if ((recToken != null)
                            && (token == null || recToken.RegexFuzzy.Match(token.Word) < 1)
                        )
                {
                    container.Value = recToken.RegexFuzzy.RegexFuzzy;
                }

                container.TypeName = node.Info.Name;
            }
            else
            {
                if (rule != null)
                    container.TypeName = rule.TypeName;
            }

            return container;
        }

        private Container CreateContainer(Symbol symbol)
        {
            //RuleProduction rule = node.GetCustomOject<RuleProduction>("Rule");
            Container container = new Container();
            container.Value = string.Empty;
            if (symbol.Terminal)
            {
                var token = symbol.GetCustomValue<Token>("Token");
                if (token != null)
                {
                    container.Value = token.Word;
                    container.Column = token.Collumn;
                    container.Line = token.Line;
                }
                var recToken = symbol.GetCustomValue<RecognitionToken>("RecToken");
                if ((recToken != null)
                        && (token == null || recToken.RegexFuzzy.Match(token.Word) < 1)
                    )
                {
                    container.Value = symbol.GetCustomValue<RecognitionToken>("RecToken").RegexFuzzy.RegexFuzzy;
                }
                container.TypeName = symbol.Name;
            }

            return container;
        }
    }
}
