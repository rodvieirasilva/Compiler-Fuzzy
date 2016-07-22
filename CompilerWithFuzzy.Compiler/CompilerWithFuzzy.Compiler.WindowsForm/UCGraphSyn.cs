using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.GrammarFuzzy;
using CompilerWithFuzzy.Compiler.Lex;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using CompilerWithFuzzy.Compiler;
using CompilerWithFuzzy.Core;

namespace CompilerWithFuzzy.Compiler.WindowsForm
{
    public partial class UCGraphSyn : UserControl
    {
        public UCGraphSyn(CompilerFuzzy compile)
              : this(new Graph<Symbol, double>())
        {
            compile.OnCompile += OnCompile;
        }

        public UCGraphSyn(Graph<Symbol, double> g)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            Draw(g);
        }

        public void OnCompile(CompilerFuzzy compile)
        {
            if (compile.Syn.GraphsSyntactic != null && compile.Syn.GraphsSyntactic.Count > 0)
                Draw(compile.Syn.GraphsSyntactic[0]);


        }

        private TreeNode AddNode(Node<Symbol, double> node, TreeNode parent)
        {
            var text = String.Empty;
            var rule = node.GetCustomOject<RuleProduction>("Rule");
            if (node != null && rule != null)
            {
                text += rule.ToString();
            }
            if (node.Info != null)
            {
                var token = node.Info.GetCustomValue<Token>("Token");


                if (node != null && token != null)
                {
                    text += token.ToString() + " (" + token.Word + ")";
                }
            }
            var nodeUI = new TreeNode(text);
            if (parent == null)
            {
                treeViewSyn.Nodes.Add(nodeUI);
            }
            else
            {
                parent.Nodes.Add(nodeUI);
            }

            return nodeUI;
        }

        private void Draw(Graph<Symbol, double> g)
        {
            treeViewSyn.Nodes.Clear();
            if (g != null && g.Root != null)
            {
                Graph graph = new Graph("Graph");
                //graph.Attr.LayerDirection = LayerDirection.TB;
                Node<Symbol, double> node = g.Root;
                TreeNode treeNode;
                Stack<Node<Symbol, double>> stack = new Stack<Node<Symbol, double>>();
                Stack<TreeNode> stackTreeNode = new Stack<TreeNode>();
                stackTreeNode.Push(null);

                stack.Push(node);
                while (stack.Count > 0)
                {
                    node = stack.Pop();
                    treeNode = stackTreeNode.Pop();
                    var treeNodeNext = AddNode(node, treeNode);

                    string str = "Root";
                    if (node.Info != null)
                        str = node.Info.Name;
                    var no = graph.AddNode(node.Name);
                    no.Attr.Shape = Shape.Circle;
                    no.LabelText = str;

                    foreach (var transition in System.Linq.Enumerable.Reverse(node.Edges))
                    {
                        Edge arco = graph.AddEdge(node.Name, "", transition.Destiny.Name);

                        System.Drawing.Color c = Utils.GetColor(transition.Cost);
                        var color = new Microsoft.Msagl.Drawing.Color((byte)c.R, (byte)c.G, (byte)c.B);
                        arco.Attr.Color = color;
                        arco.Attr.AddStyle(Style.Bold);
                        arco.Attr.LineWidth = 5;
                        arco.LabelText = transition.Cost.ToString("n2");

                        stack.Push(transition.Destiny);
                        stackTreeNode.Push(treeNodeNext);
                    }
                }

                GViewer viewer = new GViewer();
                // viewer.CurrentLayoutMethod = LayoutMethod.Ranking;

                viewer.NavigationVisible = false;
                viewer.OutsideAreaBrush = Brushes.White;
                viewer.ToolBarIsVisible = false;
                viewer.Graph = graph;
                viewer.Dock = System.Windows.Forms.DockStyle.Fill;

                pnlGraph.Controls.Clear();
                pnlGraph.Controls.Add(viewer);
                treeViewSyn.ExpandAll();
            }

        }
    }
}
