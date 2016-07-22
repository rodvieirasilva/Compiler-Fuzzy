using CompilerWithFuzzy.AutomaFuzzy;
using CompilerWithFuzzy.GrammarFuzzy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.Compiler.Syn
{
    public class CustomValueStateNodeLR1
    {

        public List<RuleProduction> Productions
        {
            get; set;
        }

        public int CoutPaths
        {
            get; set;
        }

        public List<Symbol> Tokens
        {
            get; set;
        }

        public int IToken
        {
            get; set;
        }

        public Symbol ASymbol
        {
            get; set;
        }

        public Symbol ASymbolReal
        {
            get; set;
        }

        public double Pertinence
        {
            get; set;
        }

        public CustomValueStateNodeLR1()
        {
            Productions = new List<RuleProduction>();
            Tokens = new List<Symbol>();
        }

        public int IndexInParent
        {
            get; set;
        }

        public CustomValueStateNodeLR1(CustomValueStateNodeLR1 old)
        {
            Productions = new List<RuleProduction>();
            Tokens = new List<Symbol>();

            this.IToken = old.IToken;
            this.CoutPaths = old.CoutPaths;
            this.Productions.AddRange(old.Productions);
            this.Tokens.AddRange(old.Tokens);
            this.ASymbolReal = old.ASymbolReal;
            this.ASymbol = old.ASymbol;
        }

    }
}

