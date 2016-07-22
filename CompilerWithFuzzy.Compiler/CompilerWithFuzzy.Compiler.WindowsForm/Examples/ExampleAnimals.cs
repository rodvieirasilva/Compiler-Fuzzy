using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.Compiler.Syn.ParseTree;
using CompilerWithFuzzy.Compiler;
using CompilerWithFuzzy.GrammarFuzzy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilerWithFuzzy.Compiler.WindowsForm.Examples
{
    public static class ExampleAnimals
    {

        public static Dictionary<string, Func<Container, string>> DicCompileExample { get; set; }

        public static CompilerFuzzy Compiler { get; set; }

        static ExampleAnimals()
        {
            try
            {
                CompilerXmlParser parserXml = new CompilerXmlParser(".\\Examples\\ExampleAnimals1.xml");

                DicCompileExample = new Dictionary<string, Func<Container, string>>();

                //DicCompileExample.Add("Object", c => string.Format("com o objeto {0}", c["ball"].Value));

                //DicCompileExample.Add("Main",
                //        c => c.Count > 1 ? string.Format("Você mandou o {0} realizar a ação de {1}  com o objeto {2}",
                //            c["animal"].Value, c["action"].Value, c["object"].Value) : "");


                DicCompileExample.Add("A", c => string.Format("Você mandou o {0}", c["animal"].Value));

                DicCompileExample.Add("B", c => string.Format("realizar a ação de {0}", c["action"].Value));

                DicCompileExample.Add("C", c => string.Format("com o objeto {0}", c["object"].Value));

                DicCompileExample.Add("D", c => String.Format("{0} {1}", DicCompileExample["B"](c["b"]), DicCompileExample["C"](c["c"])));

                DicCompileExample.Add("Main", c => String.Format("{0} {1}", DicCompileExample["A"](c["a"]), DicCompileExample["D"](c["d"])));

                DicCompileExample.Add("Main1", c => String.Format("{0} {1}", DicCompileExample["E"](c["e"]), DicCompileExample["D"](c["d"])));



                parserXml.Compiler.DicCompile = DicCompileExample;

                parserXml.Compiler.Compile("dog get ball");






                ExampleAnimals.Compiler = parserXml.Compiler;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //            //LR(1)
            //            //0. S
            //            //0 → S$
            //            //1. S → V = E
            //            //2. S → E
            //            //3. E → V
            //            //4. V → x
            //            //5. V → ∗E
            //            List<RecognitionToken> recs = new List<RecognitionToken>();
            //            var dog = new RecognitionToken(1, "dog", "dog", "#7bf6b6");
            //            var cat = new RecognitionToken(2, "cat", "cat", "#7bf6b6");

            //            var get = new RecognitionToken(3, "get", "get", "#f28686");
            //            var sleep = new RecognitionToken(4, "sleep", "sleep", "#fbdb65");

            //            var ball = new RecognitionToken(5, "ball", "ball", "#fbdb65");


            //            recs.Add(dog);
            //            recs.Add(cat);
            //            recs.Add(get);
            //            recs.Add(sleep);
            //            recs.Add(ball);

            //            Grammar grammar = new Grammar();
            //            var catSymbol = new Symbol(1, "cat", true);
            //            catSymbol.SetCustomValue("RecToken", cat);

            //            var dogSymbol = new Symbol(2, "dog", true);
            //            dogSymbol.SetCustomValue("RecToken", dog);

            //            var getSymbol = new Symbol(3, "get", true);
            //            getSymbol.SetCustomValue("RecToken", get);

            ////rabbit
            ////parrot
            ////pick up
            ////to eat
            ////drop
            ////to sleep
            ////feed
            ////ball
            ////toy

            //            var sleepSymbol = new Symbol(4, "sleep", true);
            //            sleepSymbol.SetCustomValue("RecToken", sleep);

            //            var ballSymbol = new Symbol(5, "ball", true);
            //            ballSymbol.SetCustomValue("RecToken", ball);


            //            var s = new Symbol(6, "S", false);
            //            var animal = new Symbol(7, "Animal", false);
            //            var action = new Symbol(8, "Action", false);
            //            var obj = new Symbol(9, "Obj", false);

            //            grammar.Terminals.AddRange(new SymbolList(catSymbol, dogSymbol, getSymbol, sleepSymbol, ballSymbol));

            //            grammar.Variables.AddRange(new SymbolList(s, animal, action, obj));
            //            grammar.VariableStart = s;
            //            //grammar.AddRole(s, animal, action);
            //            grammar.AddRule(s, animal, action, obj);
            //            grammar.Rules[0].Pertinence = 1;
            //            grammar.AddRule(s, animal, action).Parent = grammar.Rules[0];
            //            grammar.Rules[1].Pertinence = 1;
            //            grammar.AddRule(s, action).Parent = grammar.Rules[0];
            //            grammar.Rules[2].Pertinence = 0.8;

            //            grammar.AddRule(animal, dogSymbol);
            //            grammar.AddRule(animal, catSymbol);

            //            grammar.AddRule(action, getSymbol);

            //            grammar.AddRule(obj, ballSymbol);

            //            //var ruleV = grammar.AddRole(V, x);
            //            // grammar.AddRole(V1, equal).Parent = ruleV;

            //            ExampleAnimals.Compiler = new Compiler(recs, grammar);

        }
    }
}
