using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.Compiler.Syn;
using CompilerWithFuzzy.Core;
using CompilerWithFuzzy.GrammarFuzzy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.Compiler
{
    [DataContract]
    [Serializable]
    public class CompilerXmlParser
    {
        [DataMember]
        public CompilerFuzzy Compiler { get; set; }

        List<RecognitionToken> RecsTokens { get; set; }
        Grammar Grammar { get; set; }


        [DataMember]
        public Dictionary<int, List<string>> NamesVars { get; set; }


        public CompilerXmlParser()
        { }
        public CompilerXmlParser(string filename)
        {

            NamesVars = new Dictionary<int, List<string>>();
            RecsTokens = new List<RecognitionToken>();
            Grammar = new Grammar();

            using (FileStream file = File.OpenRead(filename))
            {
                XDocument xdoc = XDocument.Load(file, LoadOptions.None);


                var settings = xdoc.Element("Compiler").Element("Settings");
                NormAbstract norm;
                switch (settings.Element("Norm").Value)
                {
                    case "MIN":
                        norm = new MinNorm();
                        break;
                    default:
                        norm = new MultiplyNorm();
                        break;
                }
                ConormAbstract conorm;
                switch (settings.Element("Conorm").Value)
                {
                    case "SUM":
                        conorm = new SumMinusProductConorm();
                        break;
                    default:
                        conorm = new MaxConorm();
                        break;
                }

                var recTokens = xdoc.Element("Compiler").Element("RecTokens").Elements("RecToken");
                int idTokens = 1;
                foreach (var token in recTokens)
                {

                    int id = idTokens++;
                    if (token.Attributes("id").Any() && !string.IsNullOrWhiteSpace(token.Attribute("id").Value))
                    {
                        id = Convert.ToInt32(token.Attribute("id").Value);
                    }

                    string name = token.Attribute("name").Value;
                    string fregex = token.Attribute("fregex").Value;

                    string hexColor = string.Empty;
                    if (token.Attributes("color").Any())
                    {
                        hexColor = token.Attribute("color").Value;
                    }


                    RecsTokens.Add(new RecognitionToken(id, name, fregex, hexColor, norm, conorm));
                }

                int idSymbols = 100;
                var symbolsXml = xdoc.Element("Compiler").Element("Grammar").Element("Symbols").Elements("Symbol");
                foreach (var symbolXml in symbolsXml)
                {
                    int id = idSymbols++;
                    if (symbolXml.Attributes("id").Any() && !string.IsNullOrWhiteSpace(symbolXml.Attribute("id").Value))
                    {
                        id = Convert.ToInt32(symbolXml.Attribute("id").Value);
                    }

                    string name = symbolXml.Attribute("name").Value;
                    bool terminal = false;

                    if (symbolXml.Attributes("terminal").Any())
                    {
                        terminal = Convert.ToBoolean(symbolXml.Attribute("terminal").Value);
                    }

                    string tempTokenReference = string.Empty;
                    int recTokenId = 0;

                    if (symbolXml.Attributes("recTokenId").Any())
                    {
                        tempTokenReference = symbolXml.Attribute("recTokenId").Value;
                        if (!string.IsNullOrWhiteSpace(tempTokenReference))
                            recTokenId = Convert.ToInt32(tempTokenReference);
                    }

                    if (string.IsNullOrWhiteSpace(tempTokenReference) && symbolXml.Attributes("recTokenName").Any())
                    {
                        tempTokenReference = symbolXml.Attribute("recTokenName").Value;
                    }


                    string temp = symbolXml.Attribute("charValue").Value;
                    if (string.IsNullOrWhiteSpace(temp))
                    {
                        temp = "\0";
                    }
                    char value = temp[0];
                    Symbol symbol = new Symbol(id, name, terminal, value);

                    if (terminal)
                    {
                        if (recTokenId > 0)
                        {
                            symbol.SetCustomValue("RecToken", RecsTokens.First(rt => rt.Id == recTokenId));
                        }
                        else if (!string.IsNullOrWhiteSpace(tempTokenReference))
                        {
                            symbol.SetCustomValue("RecToken", RecsTokens.First(rt => rt.Name == tempTokenReference));
                        }

                        Grammar.Terminals.Add(symbol);
                    }
                    else
                    {
                        bool variableInitial = false;
                        if (symbolXml.Attribute("variableInitial") != null)
                            variableInitial = Convert.ToBoolean(symbolXml.Attribute("variableInitial").Value);

                        if (variableInitial)
                        {
                            Grammar.VariableStart = symbol;
                        }
                        Grammar.Variables.Add(symbol);
                    }
                }

                int idRules = 100;
                var rulesXml = xdoc.Element("Compiler").Element("Grammar").Element("Rules").Elements("Rule");
                
                foreach (var ruleXml in rulesXml)
                {

                    bool gotoEmpty = false;
                    string sourceName = ruleXml.Attribute("sourceName").Value;

                    if (sourceName.Contains("normalAnnotation"))
                    {

                    }

                    double pertinence = Convert.ToDouble(ruleXml.Attribute("pertinence").Value.Replace(",", "."), CultureInfo.InvariantCulture);

                    SymbolList destinys = new SymbolList();

                    var destinysXml = ruleXml.Element("Destinys").Elements("Destiny");

                    List<string> nameVars = new List<string>();
                    foreach (var destinyXml in destinysXml)
                    {
                        string name = destinyXml.Attribute("name").Value;


                        if (name != "Empty")
                        {

                            var symbol = Grammar.Symbols.FirstOrDefault(s => s.Name == name);

                            if (symbol == null)
                            {
                                symbol = new Symbol(idSymbols++, name, false, '\0');
                                Grammar.Variables.Add(symbol);
                            }

                            destinys.Add(symbol);
                        }
                        else
                        {
                            gotoEmpty = destinysXml.Count() == 1;

                            destinys.Add(Symbol.EmptySymbol);

                            if (!Grammar.Terminals.Contains(Symbol.EmptySymbol))
                            {
                                Grammar.Terminals.Add(Symbol.EmptySymbol);
                            }

                        }
                        string namevar = name;
                        if (destinyXml.Attribute("var") != null && !string.IsNullOrWhiteSpace(destinyXml.Attribute("var").Value))
                        {
                            namevar = destinyXml.Attribute("var").Value.Trim();
                        }
                        nameVars.Add(namevar);
                    }

                    var sourceSymbol = Grammar.Variables.FirstOrDefault(s => s.Name == sourceName);
                    
                    if (sourceSymbol == null)
                    {
                        if (sourceSymbol == null)
                        {
                            sourceSymbol = new Symbol(idSymbols++, sourceName, false, '\0');
                        }
                        Grammar.Variables.Add(sourceSymbol);
                    }
                    sourceSymbol.GoToEmpty = gotoEmpty;
                    RuleProduction rule = new RuleProduction(sourceSymbol, destinys, pertinence);

                    rule.Description = ruleXml.Element("Description").Value;

                    foreach (var attr in ruleXml.Attributes())
                    {
                        if (attr.Name == "typeName")
                            rule.TypeName = attr.Value;
                        if (attr.Name == "id")
                        {
                            if (!string.IsNullOrWhiteSpace(attr.Value))
                            {
                                rule.Id = Convert.ToInt32(attr.Value);
                            }
                        }
                        if (attr.Name == "default")
                            rule.Default = Convert.ToBoolean(attr.Value);
                    }

                    if (rule.Id == 0)
                    {
                        rule.Id = idRules++;
                    }


                    string temp = ruleXml.Attribute("idRuleParent").Value;
                    int idRuleParent = 0;
                    if (!string.IsNullOrWhiteSpace(temp))
                        idRuleParent = Convert.ToInt32(temp);

                    if (idRuleParent > 0)
                    {
                        rule.Parent = Grammar.Rules.First(r => r.Id == idRuleParent);
                    }


                    //Adicionando Nome das variáveis
                    NamesVars.Add(rule.Id, nameVars);

                    Grammar.Rules.Add(rule);
                }





                this.Compiler = new CompilerFuzzy(RecsTokens, Grammar, NamesVars, null, norm, conorm);

                switch (settings.Element("Parser").Value)
                {
                    case "SyntacticAnalysisCYK":
                        this.Compiler.Syn = new SyntacticAnalysisCYK(Grammar);
                        break;
                    case "SyntacticAnalysisLR1":
                    default:
                        this.Compiler.Syn = new SyntacticAnalysisLR1(Grammar, norm, conorm);
                        break;
                }
                switch (settings.Element("Lexer").Value)
                {
                    case "FullLexicalAnalysis":
                        this.Compiler.Lex = new FullLexicalAnalysis(RecsTokens, norm, conorm);
                        break;
                    case "TokenizerLexicalAnalysis":
                    default:
                        this.Compiler.Lex = new TokenizerLexicalAnalysis(RecsTokens, norm, conorm);
                        break;
                }
            }
        }
    }
}
