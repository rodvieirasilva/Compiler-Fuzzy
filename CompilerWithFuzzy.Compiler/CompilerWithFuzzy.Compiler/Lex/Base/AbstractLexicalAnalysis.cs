using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace CompilerWithFuzzy.Compiler.Lex.Base
{
    [DataContract]
    [Serializable]
    public abstract class AbstractLexicalAnalysis
    {
        [DataMember]
        public List<RecognitionToken> Rules { get; set; }

        [DataMember]
        public List<RecognitionToken> RulesUnitys { get; set; }

        [DataMember]
        public int IdsToken { get; set; }

        [DataMember]
        public int IdsNode { get; set; }

        [DataMember]
        public Graph<Token, Token> GraphTokens { get; set; }
        [DataMember]
        public double MinMath { get; set; }

        [DataMember]
        public NormAbstract Norm { get; set; }
        [DataMember]
        public ConormAbstract Conorm { get; set; }

        public AbstractLexicalAnalysis()
        {
        }
        public AbstractLexicalAnalysis(List<RecognitionToken> rules, NormAbstract norm, ConormAbstract conorm)
        {
            this.Norm = norm;
            this.Conorm = conorm;
            Rules = rules;
            IdsToken = 0;
            IdsNode = 0;
            RulesUnitys = rules.Where(r => r.Unitary).ToList();
        }

        /// <summary>
        /// 1. Create Structs
        /// 2. Separators unitys
        /// 2.1   
        /// </summary>
        /// <param name="sourcecode"></param>
        /// <returns></returns>

        public abstract Graph<Token, Token> Analysis(string sourcecode);

        protected int NextIdToken()
        {
            return ++IdsToken;
        }

        protected int NextIdNode()
        {
            return ++IdsNode;
        }

        protected List<RecognitionToken> FindRules(string strActual)
        {
            List<RecognitionToken> returns = new List<RecognitionToken>();

            for (int i = 0; i < Rules.Count; i++)
            {
                if (i == 107)
                {

                }
                double match = Rules[i].RegexFuzzy.Match(strActual);
                if (match > MinMath)
                {
                    Rules[i].LastMatchValue = match;
                    returns.Add(Rules[i]);
                }
            }

            return returns;
        }

        protected List<RecognitionToken> FindRulesUnitys(char c)
        {
            List<RecognitionToken> returns = new List<RecognitionToken>();

            for (int i = 0; i < RulesUnitys.Count; i++)
            {
                double match = RulesUnitys[i].RegexFuzzy.Match(c);
                if (match > MinMath)
                {
                    RulesUnitys[i].LastMatchValue = match;
                    returns.Add(RulesUnitys[i]);
                }
            }

            return returns;
        }
    }
}
