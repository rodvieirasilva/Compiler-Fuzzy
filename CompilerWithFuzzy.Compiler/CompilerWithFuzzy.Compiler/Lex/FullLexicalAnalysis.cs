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
    public class FullLexicalAnalysis : AbstractLexicalAnalysis
    {
        public FullLexicalAnalysis()
           : base()
        {
        }
        public FullLexicalAnalysis(List<RecognitionToken> rules, NormAbstract norm, ConormAbstract conorm)
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
            //sourcecode = sourcecode.Replace("\t", "   ");
            GraphTokens = new Graph<Token, Token>();
            List<Token> lastTokens = new List<Token>();
            int lineActual = 0;
            int columnActual = 0;
            List<int> separators = new List<int>();
            Dictionary<int, List<Node<Token, Token>>> dicLastNodes = new Dictionary<int, List<Node<Token, Token>>>();

            List<Node<Token, Token>> lastNodes;
            dicLastNodes.Add(0, new List<Node<Token, Token>>());
            var root = GraphTokens.AddNode("Root");
            GraphTokens.Root = root;
            root.Info = new Token(0, 0, 0, null, "R");
            dicLastNodes[0].Add(root);
            for (int i = 0; i < sourcecode.Length; i++)
            {
                columnActual = i;
                List<RecognitionToken> unitys = FindRulesUnitys(sourcecode[i]);
                int tempSe = 0;
                lastNodes = new List<Node<Token, Token>>();
                foreach (var item in unitys)
                {
                    tempSe = i + 1;
                    if (!dicLastNodes.Keys.Contains(tempSe))
                    {
                        dicLastNodes.Add(tempSe, new List<Node<Token, Token>>());
                    }

                    var node = GraphTokens.AddNode(NextIdNode());
                    Token token = new Token(NextIdToken(), lineActual, columnActual, item, sourcecode[i]);
                    node.Info = token;
                    lastNodes.Add(node);
                    foreach (var last in dicLastNodes[i])
                    {
                        var edge = last.AddEdge(token, node, item.LastMatchValue);
                    }
                }
                dicLastNodes[tempSe].AddRange(lastNodes);


                lastNodes.Clear();

                List<int> keys = dicLastNodes.Keys.ToList();
                foreach (var index in keys)
                {
                    int count = i - index + 1;
                    if (count > 1)
                    {
                        tempSe = index + count;
                        string strActual = sourcecode.Substring(index, count);
                        if (!strActual.Contains(" "))
                        {
                            List<RecognitionToken> roles = FindRules(strActual);
                            foreach (var item in roles)
                            {
                                var node = GraphTokens.AddNode(NextIdNode());
                                Token token = new Token(NextIdToken(), lineActual, columnActual, item, strActual);
                                node.Info = token;

                                lastNodes.Add(node);

                                foreach (var last in dicLastNodes[index])
                                {
                                    var edge = last.AddEdge(token, node, item.LastMatchValue);
                                }

                            }
                            if (!dicLastNodes.Keys.Contains(tempSe))
                            {
                                dicLastNodes.Add(tempSe, new List<Node<Token, Token>>());
                            }
                            lastNodes = lastNodes.Where(l => !dicLastNodes[tempSe].Contains(l)).ToList();
                            dicLastNodes[tempSe].AddRange(lastNodes);
                        }
                    }
                }

                if (sourcecode[i] == '\n')
                {
                    lineActual++;
                    columnActual = 0;
                }
            }

            return GraphTokens;
        }

    }
}
