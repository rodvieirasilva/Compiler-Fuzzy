using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.GrammarFuzzy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.Compiler.Syn.ParseTree
{
    [DataContract]
    [Serializable]
    public class FixSyn
    {
        [DataMember]
        public Node<Symbol, double> NodeResult { get; set; }

        [DataMember]
        public List<Symbol> Symbols { get; set; }

        [DataMember]
        public Grammar Grammar { get; set; }

        public FixSyn()
        { }
        public FixSyn(Node<Symbol, double> node, Grammar grammar)
        {
            this.Symbols = grammar.Symbols;
            this.Grammar = grammar;
            var rule = node.GetCustomOject<RuleProduction>("Rule");
            NodeResult = CreateNode(node);
            NodeResult.Name = rule.Source.Name;
            NodeResult = CreateTreeNode(NodeResult);
        }

        
        public Node<Symbol, double> CreateTreeNode(Node<Symbol, double> nodeParent)
        {
            nodeParent = CreateNode(nodeParent);
            for (int i = 0; i < nodeParent.Edges.Count; i++)
            {
                nodeParent.Edges[i].Destiny = CreateTreeNode(nodeParent.Edges[i].Destiny);
            }

            return nodeParent;
        }

        private bool DestinyIsTerminal(Node<Symbol, double> node)
        {
            return node.Info != null && node.Info.Terminal;
        }

        private Node<Symbol, double> CreateNode(Node<Symbol, double> node)
        {
            RuleProduction rule = node.GetCustomOject<RuleProduction>("Rule");

            if (rule != null && rule.Parent != null)
            {
                var parent = rule.Parent;
                node.SetCustomOject("Rule", rule.Parent);
                List<Node<Symbol, double>> founds = new List<Node<Symbol, double>>();
                for (int i = 0; i < parent.Destiny.Count; i++)
                {
                    bool found = false;

                    for (int j = node.Edges.Count - 1; j >= 0; j--)
                    {
                        if (parent.Destiny[i].Id == node.Edges[j].Destiny.Info.Id && !founds.Contains(node.Edges[j].Destiny))
                        {
                            found = true;
                            node.Edges[j].Destiny.SetCustomOject("CostEdge", node.Edges[j].Cost);
                            founds.Add(node.Edges[j].Destiny);
                            break;
                        }
                    }

                    if (!found)
                    {
                        var n = CreateNotFoundNode(parent.Destiny[i], rule.Pertinence);
                        n.SetCustomOject("CostEdge", rule.Pertinence);
                        founds.Add(n);
                    }
                }

                node.Edges.Clear();
                for (int i = 0; i < founds.Count; i++)
                {
                    var cost = founds[i].GetCustomOject<double>("CostEdge");
                    node.AddEdge(cost, founds[i], cost);
                }
            }

            return node;
        }

        private Node<Symbol, double> CreateNotFoundNode(Symbol symbol, double pertinence)
        {
            Node<Symbol, double> nActual = new Node<Symbol, double>(string.Empty, symbol.Copy());

            if (!symbol.Terminal)
            {
                var rule = Grammar.Rules.Find(r => r.Source == symbol && r.Default);
                nActual.SetCustomOject("Rule", rule);
                foreach (var destiny in rule.Destiny)
                {
                    var newNode = CreateNotFoundNode(destiny, pertinence);
                    Edge<Symbol, double> edge = new Edge<Symbol, double>(pertinence, newNode, pertinence);
                    nActual.Edges.Add(edge);
                }
            }
            return nActual;
        }

    }
}
