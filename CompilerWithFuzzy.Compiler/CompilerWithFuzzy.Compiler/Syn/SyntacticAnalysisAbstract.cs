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
using CompilerWithFuzzy.Core;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.Compiler.Syn
{
    [DataContract]
    [Serializable]
    public abstract class SyntacticAnalysisAbstract
    {
        [DataMember]
        public List<Graph<Symbol, double>> GraphsSyntactic { get; set; }


        public abstract double Validate(List<Symbol> tokens);


        public abstract DataTable GetDataTable();

        [DataMember]
        public NormAbstract Norm
        {
            get; set;
        }

        [DataMember]
        public ConormAbstract Conorm
        {
            get; set;
        }

        public SyntacticAnalysisAbstract()
        {

        }
        public SyntacticAnalysisAbstract(Grammar grammar, NormAbstract norm, ConormAbstract conorm)
        {
            this.Norm = norm;
            this.Conorm = conorm;
            this.Grammar = grammar;
        }

        [DataMember]
        public Grammar Grammar
        {
            get; set;
        }
    }


}
