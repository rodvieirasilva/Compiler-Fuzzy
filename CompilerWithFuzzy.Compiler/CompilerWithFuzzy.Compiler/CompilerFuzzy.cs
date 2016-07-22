using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.Compiler.Lex.Base;
using CompilerWithFuzzy.Compiler.Syn;
using CompilerWithFuzzy.Compiler.Syn.ParseTree;
using CompilerWithFuzzy.Core;
using CompilerWithFuzzy.GrammarFuzzy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CompilerWithFuzzy.Compiler
{
    [DataContract]
    [Serializable]
    public class CompilerFuzzy
    {
        [DataMember]
        public Action<CompilerFuzzy> OnCompile { get; set; }


        [DataMember]
        public AbstractLexicalAnalysis Lex { get; set; }

        [DataMember]
        public SyntacticAnalysisAbstract Syn { get; set; }

        [DataMember]
        public Grammar Grammar { get; set; }

        [DataMember]
        public Container ContainerInitial { get; set; }

        [DataMember]
        public Dictionary<int, List<string>> NameVars { get; set; }

        [DataMember]
        public Dictionary<string, Func<Container, string>> DicCompile { get; set; }

        [DataMember]
        public string CodeCompiled { get; set; }

        [DataMember]
        public string CodeModifySource
        {
            get; set;
        }

        [DataMember]
        public double PertinenceLex
        {
            get; set;
        }

        [DataMember]
        public double PertinenceSyn
        {
            get;
            set;
        }

        [DataMember]
        public double PertinenceTotal
        { get; set; }

        [DataMember]
        public FixSyn FixSyntatic
        {
            get; set;
        }

        [DataMember]
        public NormAbstract Norm { get; set; }
        [DataMember]
        public ConormAbstract Conorm { get; set; }

        public CompilerFuzzy()
        {
        }
        public CompilerFuzzy(List<RecognitionToken> recs,
                Grammar grammar,
                Dictionary<int, List<string>> namesVars,
                Dictionary<string, Func<Container, string>> dicCompileExample,
                NormAbstract norm,
                ConormAbstract conorm)
        {
            this.Norm = norm;
            this.Conorm = conorm;
            this.DicCompile = dicCompileExample;
            this.Grammar = grammar;
            this.NameVars = namesVars;
        }

        public void Compile(string sourcecode)
        {
            CodeModifySource = string.Empty;

            DateTime date = DateTime.Now;
            var graph = Lex.Analysis(sourcecode);
            Utils.SaveTime("Lex", date);

            date = DateTime.Now;
            var paths = graph.AllPaths(Norm).OrderByDescending(p => p.Cost);
            Utils.SaveTime("GetPaths", date);

            date = DateTime.Now;
            for (int i = 0; i < paths.Count() && i < 3; i++)
            {
                var symbols = GetSymbols(paths.ElementAt(i));
                var pertinenceSyn = Syn.Validate(symbols);
                if (pertinenceSyn > 0)
                {
                    FixSyntatic = new FixSyn(Syn.GraphsSyntactic[0].Root, Grammar);

                    NodeToTreeContainer nttc = new NodeToTreeContainer(FixSyntatic.NodeResult, NameVars, symbols);

                    CodeModifySource = string.Empty;
                    MountCodeModifySource(FixSyntatic.NodeResult);
                    CodeModifySource = CodeModifySource.TrimStart();
                    Compile(nttc);

                    PertinenceLex = paths.ElementAt(i).Cost;
                    PertinenceSyn = pertinenceSyn;
                    PertinenceTotal = Norm.Calculate(PertinenceLex, PertinenceSyn);
                    break;
                }

            }
            Utils.SaveTime("ValidateSyn", date);

            GC.Collect();
            if (OnCompile != null)
            {
                OnCompile(this);
            }
        }

        private void MountCodeModifySource(Node<Symbol, double> node)
        {

            foreach (var transition in node.Edges)
            {
                MountCodeModifySource(transition.Destiny);
            }

            if (node.Info != null && node.Info.Terminal && node.Info != Symbol.EmptySymbol)
            {
                var temp = node.Info.GetCustomValue<Token>("Token");
                if (temp != null)
                {
                    if (string.IsNullOrWhiteSpace(temp.Word) || temp.RecToken.RegexFuzzy.Match(temp.Word) < 1)
                    {
                        CodeModifySource += " " + temp.RecToken.RegexFuzzy.RegexFuzzy;
                    }
                    else
                    {
                        CodeModifySource += " " + temp.Word;
                    }
                }
                else
                {
                    var temp1 = node.Info.GetCustomValue<RecognitionToken>("RecToken");
                    if (temp1 != null)
                    {
                        CodeModifySource += " " + temp1.RegexFuzzy.RegexFuzzy;
                    }
                }
            }

        }

        private void Compile(NodeToTreeContainer nttc)
        {
            if (DicCompile != null && DicCompile.ContainsKey(nttc.ContainerResult.TypeName))
                CodeCompiled = DicCompile[nttc.ContainerResult.TypeName](nttc.ContainerResult);
        }
        public List<Symbol> GetSymbols(GraphPath<Token, Token> path)
        {
            List<Symbol> symbols = new List<Symbol>();
            foreach (var item in path.Nodes.Skip(1))
            {
                var terminal = Syn.Grammar.Terminals.FirstOrDefault(t =>
                        t.Name == item.Info.RecToken.Name);

                if (terminal != null)
                {
                    var symbol = new Symbol(terminal.Id, terminal.Name, true, terminal.Value);
                    symbol.SetCustomValue("Token", item.Info);
                    symbols.Add(symbol);
                }
            }

            return symbols;
        }

    }
}
