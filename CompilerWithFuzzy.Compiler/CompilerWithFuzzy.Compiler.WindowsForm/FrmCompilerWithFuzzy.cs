using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.Compiler;
using CompilerWithFuzzy.Compiler.Lex.Base;
using CompilerWithFuzzy.Compiler.Syn;
using CompilerWithFuzzy.Compiler.WindowsForm.Examples;
using CompilerWithFuzzy.GrammarFuzzy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompilerWithFuzzy.Core;

namespace CompilerWithFuzzy.Compiler.WindowsForm
{
    public partial class FrmCompilerWithFuzzy : Form
    {
        private NormAbstract norm = new MultiplyNorm();
        private ConormAbstract conorm = new MaxConorm();
        // AbstractLexicalAnalysis lex;
       public FrmCompilerWithFuzzy()
        {
            InitializeComponent();

           

            var r1 = new RecognitionToken(1, "space", " ", "#7bf6b6", norm, conorm);
            var r2 = new RecognitionToken(2, "type", "int", "#f28686", norm, conorm);
            var r3 = new RecognitionToken(2, "var", "a", "#fbdb65", norm, conorm);
            var r4 = new RecognitionToken(2, "final", ";", "#dcc6ad", norm, conorm);

            List<RecognitionToken> rules = new List<RecognitionToken>();
            rules.Add(r1);
            rules.Add(r2);
            rules.Add(r3);
            rules.Add(r4);
            //    lex = new AbstractLexicalAnalysis(rules);



            btnSyn_Click(null, null);
            //btnLexicalAnalysis_Click(null, null);
        }

        private void btnLexicalAnalysis_Click(object sender, EventArgs e)
        {

        }


        private Grammar grammar1()
        {
            Grammar grammar = new Grammar();
            //var sc = new Symbol(3, "c", true, 'c') { CustomValue = new Token(1, 0, 0, null, 'c') };
            //var sd = new Symbol(4, "d", true, 'd') { CustomValue = new Token(2, 0, 0, null, 'd') };
            //grammar.Terminals.AddRange(new SymbolList(sc, sd));
            //var sS = new Symbol(1, "S", false);
            //var sC = new Symbol(2, "C", false);
            //grammar.Variables.AddRange(new SymbolList(sS, sC));
            //grammar.VariableStart = grammar.Variables[0];
            //grammar.AddRole(sS, sC, sC);
            //grammar.Rules[0].Pertinence = 1;

            //grammar.AddRole(sC, sc, sC);
            //grammar.Rules[1].Pertinence = 1;

            //grammar.AddRole(sC, sd);
            //grammar.Rules[2].Pertinence = 1;
            return grammar;
        }

        private Grammar grammar2()
        {
            Grammar grammar = new Grammar();
            //var a = new Symbol(1, "a", true, 'a') { CustomValue = new Token(1, 0, 0, null, 'a') };
            //var left = new Symbol(2, "(", true, '(') { CustomValue = new Token(2, 0, 0, null, '(') };
            //var right = new Symbol(3, ")", true, ')') { CustomValue = new Token(3, 0, 0, null, ')') };
            //var A = new Symbol(4, "A", false);

            //grammar.Terminals.AddRange(new SymbolList(left));
            //grammar.Terminals.AddRange(new SymbolList(a));
            //grammar.Terminals.AddRange(new SymbolList(right));

            //grammar.Variables.AddRange(new SymbolList(A));
            //grammar.VariableStart = A;
            //grammar.AddRole(A, left, A, right);
            //grammar.AddRole(A, a);
            return grammar;
        }

        private Grammar grammar3()
        {
            Grammar grammar = new Grammar();
            //var a = new Symbol(1, "a", true, 'a') { CustomValue = new Token(1, 0, 0, null, 'a') };
            //var left = new Symbol(2, "(", true, '(') { CustomValue = new Token(2, 0, 0, null, '(') };
            //var right = new Symbol(3, ")", true, ')') { CustomValue = new Token(3, 0, 0, null, ')') };
            //var A = new Symbol(4, "A", false);

            //grammar.Terminals.AddRange(new SymbolList(left));
            //grammar.Terminals.AddRange(new SymbolList(a));
            //grammar.Terminals.AddRange(new SymbolList(right));

            //grammar.Variables.AddRange(new SymbolList(A));
            //grammar.VariableStart = A;
            //grammar.AddRole(A, left, A, right);
            //grammar.AddRole(A, a);
            return grammar;
        }

        private CompilerFuzzy Compiler4()
        {

            //LR(1)
            //0. S
            //0 → S$
            //1. S → V = E
            //2. S → E
            //3. E → V
            //4. V → x
            //5. V → ∗E
            List<RecognitionToken> recs = new List<RecognitionToken>();
            var requal = new RecognitionToken(1, "equal", "=", "#7bf6b6", norm, conorm);
            var rast = new RecognitionToken(2, "ast", "*", "#f28686", norm, conorm);
            var rx = new RecognitionToken(3, "x", "x", "#fbdb65", norm, conorm);
            recs.Add(requal);
            recs.Add(rast);
            recs.Add(rx);

            Grammar grammar = new Grammar();
            var x = new Symbol(1, "x", true, 'x');
            x.SetCustomValue("RecToken", rx);

            var ast = new Symbol(2, "*", true, '*');
            ast.SetCustomValue("RecToken", rast);

            var equal = new Symbol(3, "=", true, '=');
            equal.SetCustomValue("RecToken", requal);


            var S = new Symbol(4, "S", false);
            var V = new Symbol(5, "V", false);
            var V1 = new Symbol(7, "V1", false);
            var E = new Symbol(6, "E", false);

            grammar.Terminals.AddRange(new SymbolList(x, ast, equal));

            grammar.Variables.AddRange(new SymbolList(S, V, E, V1));
            grammar.VariableStart = S;
            grammar.AddRule(S, V, equal, E);
            grammar.AddRule(S, E);
            grammar.AddRule(E, V);
            grammar.AddRule(E, V1).Pertinence = 0.8;
            var ruleV = grammar.AddRule(V, x);
            grammar.AddRule(V1, equal).Parent = ruleV;
            grammar.AddRule(V, ast, E);

            return new CompilerFuzzy(recs, grammar, null, null, norm, conorm);
        }

        private void btnSyn_Click(object sender, EventArgs e)
        {
            var compile = //Compiler4();//
                ExampleAnimals.Compiler;
            tabPageAutoma.Controls.Clear();
            tabPageAutoma.Controls.Add(new UCAutomaSyn(compile));

            tabPageData.Controls.Clear();
            tabPageData.Controls.Add(new UCTableSyn(compile));

            tabPageLex.Controls.Clear();
            tabPageLex.Controls.Add(new UCGraphLex(compile));

            tabPageTree.Controls.Clear();
            tabPageTree.Controls.Add(new UCGraphSyn(compile));

            tabPageCode.Controls.Clear();
            tabPageCode.Controls.Add(new UCGraphSynToCode(compile));
            //sa.Validate(sa.Grammar.Terminals[0]);
            //sa.Validate(sa.Grammar.Terminals[0], sa.Grammar.Terminals[2], sa.Grammar.Terminals[1]);
        }



    }
}
