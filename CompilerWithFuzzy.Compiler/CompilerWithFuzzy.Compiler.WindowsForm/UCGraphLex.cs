using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.Compiler.DataStruct;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using CompilerWithFuzzy.Compiler.Lex.Base;
using CompilerWithFuzzy.Core;

namespace CompilerWithFuzzy.Compiler.WindowsForm
{
public partial class UCGraphLex : UserControl
    {

        private List<GraphPath<Token, Token>> Paths;

        private Graph<Token, Token> graphToken;

        private AbstractLexicalAnalysis lex;

    public UCGraphLex(CompilerFuzzy compile)
            : this(compile.Lex)
        {
            compile.OnCompile += OnCompile;
        }
       public UCGraphLex(AbstractLexicalAnalysis lex)
        {
            InitializeComponent();
            this.lex = lex;
            Atualizar();
        }
        private void Atualizar()
        {
            NormAbstract norm = new MultiplyNorm();
            flowLayoutPanelColor.Controls.Clear();
            if (lex.GraphTokens != null)
            {
                foreach (var rule in lex.Rules)
                {
                    flowLayoutPanelColor.Controls.Add(new UCColor(rule.Name, rule.Color));
                }
                this.Dock = DockStyle.Fill;
                graphToken = lex.GraphTokens;
                Draw(null);
                Paths = lex.GraphTokens.AllPaths(norm).OrderByDescending(p => p.Cost).ToList();
                FillList();
            }
        }

      public void OnCompile(CompilerFuzzy compile)
        {
            this.lex = compile.Lex;
            Atualizar();
        }



        private void FillList()
        {
            listBoxPaths.Items.Clear();
            foreach (var item in Paths)
            {
                listBoxPaths.Items.Add(item.ToString());
            }
        }

        private void Draw(GraphPath<Token, Token> path)
        {

            Graph graph = new Graph("Graph");
            for (int i = 0; i < graphToken.Nodes.Count; i++)
            {
                string str = "Root";
                if (graphToken.Nodes[i].Info != null)
                {
                    str = graphToken.Nodes[i].Info.ToString() + " - " + graphToken.Nodes[i].Id.ToString();
                }

                Node no = graph.AddNode(graphToken.Nodes[i].Name);
                no.Attr.Shape = Shape.Circle;
                no.LabelText = str;
                if (graphToken.Nodes[i].Info != null && graphToken.Nodes[i].Info.RecToken != null)
                {
                    var color = graphToken.Nodes[i].Info.RecToken.Color;
                    no.Attr.FillColor = new Microsoft.Msagl.Drawing.Color((byte)color.R, (byte)color.G, (byte)color.B);
                }

                foreach (var transition in graphToken.Nodes[i].Edges)
                {
                    Edge arco = graph.AddEdge(graphToken.Nodes[i].Name, transition.Cost.ToString("n2"), transition.Destiny.Name);
                    System.Drawing.Color c = Utils.GetColor(transition.Cost);
                    var color = new Microsoft.Msagl.Drawing.Color((byte)c.R, (byte)c.G, (byte)c.B);

                    if (path != null)
                    {
                        if (path.Nodes.Contains(transition.Destiny) && path.Nodes.Contains(graphToken.Nodes[i]))
                        {
                            arco.Attr.Color = color;
                        }
                        else
                        {
                            arco.Attr.Color = Microsoft.Msagl.Drawing.Color.Gray;

                        }
                    }
                    else
                    {
                        arco.Attr.Color = color;
                    }

                    arco.Attr.AddStyle(Style.Bold);
                    arco.Attr.LineWidth = 5;
                }
            }

            GViewer viewer = new GViewer();
            viewer.NavigationVisible = false;
            viewer.OutsideAreaBrush = Brushes.White;
            viewer.ToolBarIsVisible = false;
            viewer.Graph = graph;
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGraph.Controls.Clear();
            pnlGraph.Controls.Add(viewer);


        }

        private void listBoxPaths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPaths.SelectedIndex > -1)
                Draw(Paths[listBoxPaths.SelectedIndex]);
        }
    }
}
