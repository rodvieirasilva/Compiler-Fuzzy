using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompilerWithFuzzy.Compiler.Syn;
using CompilerWithFuzzy.GrammarFuzzy;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using CompilerWithFuzzy.Core;
using CompilerWithFuzzy.Compiler;

namespace CompilerWithFuzzy.Compiler.WindowsForm
{
    public partial class UCAutomaSyn : UserControl
    {

        public UCAutomaSyn(CompilerFuzzy compile) : this(compile.Syn)
        {
            compile.OnCompile += OnCompile;
        }
        public void OnCompile(CompilerFuzzy compile)
        {
            if (compile.Syn is SyntacticAnalysisLR1)
                DrawAutoma((compile.Syn as SyntacticAnalysisLR1).Automa);
        }

        public UCAutomaSyn(SyntacticAnalysisAbstract syn)
        {
            InitializeComponent();
            if (syn is SyntacticAnalysisLR1)
                DrawAutoma(((syn as SyntacticAnalysisLR1)).Automa);
            this.Dock = DockStyle.Fill;
        }

        private void DrawAutoma(AutomaFuzzy.Automa<Symbol> automma)
        {
            Graph graph = new Graph("Automa");
            //graph.GraphAttr.Backgroundcolor = Microsoft.Glee.Drawing.Color.Black;
            for (int i = 0; i < automma.States.Count; i++)
            {
                string str = automma.States[i].ToString();

                Node no = graph.AddNode(automma.States[i].Name);
                no.Attr.Shape = Shape.Box;
                no.LabelText = str;
            }

            foreach (var transition in automma.Transitions)
            {
                string symbol = ((CompilerWithFuzzy.AutomaFuzzy.Rules.SimpleIncludeRule<Symbol>)transition.Rule).Symbol.ToString();
                string label =
                    ((CompilerWithFuzzy.AutomaFuzzy.Rules.SimpleIncludeRule<Symbol>)transition.Rule).Symbol.ToString() +
                            " - " + transition.Rule.Pertinence.ToString();
                label = symbol;

                Edge arco = graph.AddEdge(transition.From.Name, label
                         , transition.To.Name);
                System.Drawing.Color c = Utils.GetColor(transition.Rule.Pertinence);
                var color = new Microsoft.Msagl.Drawing.Color((byte)c.R, (byte)c.G, (byte)c.B);
                arco.Attr.Color = color;
                //arco.Attr.Fontcolor = color;
                arco.Attr.AddStyle(Style.Bold);
                arco.Attr.LineWidth = 5;
            }

            GViewer viewer = new GViewer();
            viewer.NavigationVisible = false;
            viewer.OutsideAreaBrush = Brushes.White;
            viewer.ToolBarIsVisible = false;
            viewer.Graph = graph;
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAutoma.Controls.Clear();
            pnlAutoma.Controls.Add(viewer);

        }
    }
}
