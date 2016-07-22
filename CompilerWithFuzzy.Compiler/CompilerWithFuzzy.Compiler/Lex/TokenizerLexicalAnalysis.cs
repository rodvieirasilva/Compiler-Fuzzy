using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.Compiler.Lex.Base;
using CompilerWithFuzzy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace CompilerWithFuzzy.Compiler.Lex
{
    [DataContract]
    [Serializable]
    public class TokenizerLexicalAnalysis : AbstractLexicalAnalysis
    {

        public TokenizerLexicalAnalysis() : base()
        { }

        public TokenizerLexicalAnalysis(List<RecognitionToken> rules, NormAbstract norm, ConormAbstract conorm)
            : base(rules, norm, conorm)
        {
        }

        /// <summary>
        /// 1. Create Structs
        /// 2. Separators unitys
        /// 2.1   
        /// </summary>
        /// <param name="sourcecode"></param>
        /// <returns></returns>
        public override Graph<Token, Token> Analysis(string sourcecode)
        {
            if (string.IsNullOrWhiteSpace(sourcecode))
                sourcecode = string.Empty;

            sourcecode = sourcecode.Replace("\r", "");
            List<string> tokens = Tokenizer(sourcecode);

            GraphTokens = new Graph<Token, Token>();
            List<Token> lastTokens = new List<Token>();
            int lineActual = 0;
            int columnActual = 0;
            var root = GraphTokens.AddNode("Root");
            GraphTokens.Root = root;
            root.Info = new Token(0, 0, 0, null, "R");
            Node<Token, Token> lastNode = root;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (i == 23)
                {
                }
                columnActual = i;

                RecognitionToken rule = FindRules(tokens[i]).Max();
                if (rule != null)
                {
                    var node = GraphTokens.AddNode(NextIdNode());
                    Token token = new Token(NextIdToken(), lineActual, columnActual, rule, tokens[i]);
                    node.Info = token;
                    var edge = lastNode.AddEdge(token, node, rule.LastMatchValue);
                    lastNode = node;
                }
            }
            return GraphTokens;
        }


        public List<string> Tokenizer(string source)
        {
            source = source.Replace("\r", " ").Replace("\t", " ").Replace("  ", " ").Replace("\n", " ");
            List<char> unitarys = RulesUnitys.Select(r => r.RegexFuzzy.RegexFuzzy[0]).ToList();
            unitarys.Add(' ');
            unitarys.Add('"');


            List<string> returns = new List<string>();
            int iPrev = 0;
            for (int i = 0; i < source.Length; i++)
            {
                bool final = i == source.Length - 1;
                if (unitarys.Contains(source[i]))
                {
                    if (source[i] == '\'' || source[i] == '\"')
                    {
                        char indexStr = source[i];

                        int termStr = source.IndexOf(indexStr, i + 1);
                        //returns.Add("'");
                        returns.Add(indexStr + source.Substring(i + 1, termStr - i - 1) + indexStr);
                        //returns.Add("'");
                        i = termStr + 1;
                        iPrev = i;
                    }
                    else
                    {
                        int temp = i - iPrev;

                        if (temp > 0 && source.Substring(iPrev, temp).Trim().Length > 0)
                            returns.Add(source.Substring(iPrev, temp));
                        if (source[i] != ' ')
                            returns.Add(source.Substring(i, 1));
                        iPrev = i + 1;
                    }
                }

                if (final)
                {
                    int temp = source.Length - iPrev;
                    if (temp > 0 && source.Substring(iPrev, temp).Trim().Length > 0)
                        returns.Add(source.Substring(iPrev, temp));
                }

            }

            return returns;
        }
    }
}
