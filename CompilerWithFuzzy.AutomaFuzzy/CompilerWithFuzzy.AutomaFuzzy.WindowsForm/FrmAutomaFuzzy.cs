using CompilerWithFuzzy.Core;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilerWithFuzzy.AutomaFuzzy.WindowsForm
{
    public partial class FrmAutomaFuzzy : Form
    {
        private RecognitionFuzzy regexFuzzy;

        const char SYMBOLEMPTY = '@';
       public FrmAutomaFuzzy()
        {

            RecognitionFuzzy regexFuzzy = new RecognitionFuzzy("[0-9]+",
                    new MinNorm(), new MaxConorm(), 0.3);
            double pertinence = regexFuzzy.Match("123456a");


            InitializeComponent();
        }

        private void Draw(Automa<char> automa, int indexTable = -1)
        {
            Graph graphAutoma = new Graph("Automa");

            for (int i = 0; i < automa.States.Count; i++)
            {
                State<char> state = automa.States[i];
                Node no = graphAutoma.AddNode(state.Name);
                no.Attr.Shape = Shape.Circle;

                if (state.PertinenceFinal > 0)
                {
                    no.Attr.Shape = Shape.DoubleCircle;
                }
                if (indexTable > -1)
                {
                    System.Drawing.Color c = Utils.GetColor(regexFuzzy.TableAutomaProcessing[indexTable][i]);
                    no.Attr.FillColor = new Microsoft.Msagl.Drawing.Color((byte)c.R, (byte)c.G, (byte)c.B);
                }
                else if (state.PertinenceInitial > 0)
                {
                    no.Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightGray;
                }
            }
            foreach (var transition in automa.Transitions)
            {
                Edge arco = graphAutoma.AddEdge(transition.From.Name, transition.To.Name);
                arco.LabelText = transition.ToString();
            }
            GViewer viewer = new GViewer();
            viewer.NavigationVisible = false;
            viewer.OutsideAreaBrush = Brushes.White;
            viewer.ToolBarIsVisible = false;
            viewer.Graph = graphAutoma;
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAutoma.Controls.Clear();
            pnlAutoma.Controls.Add(viewer);

        }

        private void txtExpression_TextChanged(object sender, EventArgs e)
        {
            try
            {

                regexFuzzy = new RecognitionFuzzy(txtExpression.Text, new MinNorm(), new MaxConorm(), 0.9);
                Draw(regexFuzzy.Automa);
                PrintMatch();
            }
            catch (Exception ex)
            { }
        }

        private void PrintMatch()
        {
            double pertinence = regexFuzzy.Match(txtValidate.Text);
            lblFinalPertinence.Text = pertinence.ToString();
            lblFinalPertinence.BackColor = Utils.GetColor(pertinence);
            lbPass.Items.Clear();
            for (int i = 0; i < regexFuzzy.TableAutomaProcessing.Count; i++)
            {
                lbPass.Items.Add(string.Format("{0} -> {1}", i, string.Join("  ", regexFuzzy.TableAutomaProcessing[i].Select(d => d.ToString()))));
            }

        }

        private void txtValidate_TextChanged(object sender, EventArgs e)
        {
            PrintMatch();
            Draw(regexFuzzy.Automa);
        }

        private void lbPass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Draw(regexFuzzy.Automa, lbPass.SelectedIndex);
        }
    }
}
