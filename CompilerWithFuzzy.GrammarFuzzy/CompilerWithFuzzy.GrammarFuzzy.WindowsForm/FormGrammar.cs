using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompilerWithFuzzy.GrammarFuzzy.WindowsForm;
using CompilerWithFuzzy.GrammarFuzzy.Automa;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;

namespace CompilerWithFuzzy.GrammarFuzzy.WindowsForm
{
   public partial class FormGrammar : Form
    {
        private Grammar g;
       public FormGrammar()
        {
            InitializeComponent();
            g = new Grammar();
            txtTerminais.Text = "ab";
            txtVariaveis.Text = "SA";
            txtRegras.Text = "S=AA|a\r\nA=SS|b";
        }

        List<Transition> transicoes;
        /// <summary>
        /// Desenha o autômato.
        /// </summary>
        private void Desenha(StackAutoma automato)
        {
            Graph grafoAutomato = new Graph("Autômato");
            transicoes = new List<Transition>();
            // Adiciona elementos com base nas transições
            foreach (KeyValuePair<string, State> estado in automato.States)
            {
                Node no = grafoAutomato.AddNode(estado.Key);
                no.Attr.Shape = Shape.Circle;
                // Faz marcações no grafo..
                if (estado.Value.Final)
                {
                    no.Attr.Shape = Shape.DoubleCircle;
                }
                if (estado.Value == automato.StartState)
                {
                    no.Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightGray;
                }
                transicoes.AddRange(estado.Value.Transitions);
            }
            foreach (Transition transicao in transicoes)
            {
                Edge arco = grafoAutomato.AddEdge(transicao.Source.Name, transicao.Destiny.Name);
                arco.LabelText = string.Format("({0}, {1}, {{{2}}})", transicao.Symbol, transicao.ConsumingStack[0], string.Join(",", transicao.PushStack.ToArray().Select(c => c.Name).ToArray()));
            }
            GViewer viewer = new GViewer();
            viewer.NavigationVisible = false;
            viewer.OutsideAreaBrush = Brushes.White;
            //viewer.RemoveToolbar();
            viewer.Graph = grafoAutomato;
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAutomato.Controls.Clear();
            pnlAutomato.Controls.Add(viewer);

        }


        private void btnGerar_Click(object sender, EventArgs e)
        {
            g = new Grammar();

            g.Variables.Clear();
            g.Variables.AddRange(txtVariaveis.Text.Select(c => new Symbol(-1, c, false)));
            g.Terminals.Clear();
            g.Terminals.AddRange(txtVariaveis.Text.Select(c => new Symbol(-1, c, true, c)));



            string[] regras = txtRegras.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < regras.Length; i++)
            {
                char variavel = regras[i].ToUpper()[0];

                string prod = regras[i].Substring(2).Trim();

                string[] prods = prod.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < prods.Length; j++)
                {
                    var symbols = new SymbolList();
                    symbols = symbols.AddRange(prods[j].Select(prd => new Symbol(-1, prd, prd.IsTerminal(), prd)));
                    g.AddRule(g.Variables.Find(variavel), symbols);
                }
            }
        }

        private void txtVariaveis_TextChanged(object sender, EventArgs e)
        {
            cmbInicial.Items.Clear();
            cmbInicial.Items.AddRange(txtVariaveis.Text.ToArray().Cast<Object>().ToArray());

            if (cmbInicial.Items.Count > 0)
            {
                cmbInicial.SelectedIndex = 0;
            }

        }

        private void txtRegras_KeyUp(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = false;
            if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete && char.IsLetterOrDigit((char)e.KeyCode))
            {
                string[] regras = txtRegras.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                int j = txtRegras.SelectionStart;
                txtRegras.Text = "";
                for (int i = 0; i < regras.Length; i++)
                {
                    if (regras[i].Length > 1)
                        regras[i] = Char.ToUpper(regras[i][0]) + regras[i].Substring(1);

                    txtRegras.Text += regras[i] + "\r\n";
                }
                txtRegras.Select(j + 1, 0);
            }

        }
        Simplification simplificao;
        private void btnSimplificar_Click(object sender, EventArgs e)
        {
            btnGerar_Click(null, null);
            simplificao = new Simplification(g);

            lbVazios.Items.Clear();
            lbVazios.Items.AddRange(simplificao.VariablesEmpty.Cast<Object>().ToArray());
            lbRegrasSemVazio.Items.Clear();
            lbRegrasSemVazio.Items.AddRange(simplificao.GrammarNoEmpty.Rules.ToArray());
            lbFecho.Items.Clear();
            lbFecho.Items.AddRange(simplificao.FastenersString.ToArray());

            lbProducoesFechos.Items.Clear();
            lbProducoesFechos.Items.AddRange(simplificao.GrammarNoUnitarianProductions.Rules.ToArray());

            lbVariaveisAcessamTerminais.Items.Clear();
            lbVariaveisAcessamTerminais.Items.AddRange(simplificao.VariablesCallTerminals.Cast<Object>().ToArray());

            lbVariaveisAcessiveis.Items.Clear();
            lbVariaveisAcessiveis.Items.AddRange(simplificao.AcessiblesVariables.Cast<Object>().ToArray());

            lbTerminaisAcessiveis.Items.Clear();
            lbTerminaisAcessiveis.Items.AddRange(simplificao.AcessiblesTerminals.Cast<Object>().ToArray());

            lbProducoesFinais.Items.Clear();
            lbProducoesFinais.Items.AddRange(simplificao.Simplified.Rules.ToArray());
        }

        private void btnChonsky_Click(object sender, EventArgs e)
        {
            btnSimplificar_Click(null, null);

            NormalFormChomsky fnc = new NormalFormChomsky(simplificao.Simplified);
            lbVariaveisChomsky.Items.Clear();
            lbVariaveisChomsky.Items.AddRange(fnc.Normalized.Variables.Cast<Object>().ToArray());

            lbTerminaisChomsky.Items.Clear();
            lbTerminaisChomsky.Items.AddRange(fnc.Normalized.Terminals.Cast<Object>().ToArray());

            lbProducoesChomsky.Items.Clear();
            lbProducoesChomsky.Items.AddRange(fnc.Normalized.Rules.ToArray());
        }
        NormalFormGreibach fng;
        private void btnGreibach_Click(object sender, EventArgs e)
        {
            btnChonsky_Click(null, null);

            fng = new NormalFormGreibach(simplificao.Simplified);
            lbVariaveisGreibach.Items.Clear();
            lbVariaveisGreibach.Items.AddRange(fng.Normalized.Variables.Cast<Object>().ToArray());

            lbTerminaisGreibach.Items.Clear();
            lbTerminaisGreibach.Items.AddRange(fng.Normalized.Variables.Cast<Object>().ToArray());

            lbProducoesGreibach.Items.Clear();
            lbProducoesGreibach.Items.AddRange(fng.Normalized.Rules.ToArray());

            lbNovosNomes.Items.Clear();
            lbNovosNomes.Items.AddRange(fng.GetNewsNames());

            lbArAr.Items.Clear();
            lbArAr.Items.AddRange(fng.GetRegrasArParaAr());

            lbSubstituiArAr.Items.Clear();
            lbSubstituiArAr.Items.AddRange(fng.GetRegrasSubstituiArParaAr());

            lbArMaiorAs.Items.Clear();
            lbArMaiorAs.Items.AddRange(fng.GetRegrasArParaMaiorAs());

            lbSubstituiArMaiorAs.Items.Clear();
            lbSubstituiArMaiorAs.Items.AddRange(fng.GetRegrasSubstituiArMaiorAs());

            lbProducoesNovosNomes.Items.Clear();
            lbProducoesNovosNomes.Items.AddRange(fng.GetProducoesNovosNomes());

        }

        private void btnAutomatoPilha_Click(object sender, EventArgs e)
        {
            btnGreibach_Click(null, null);
            StackAutoma automato = new StackAutoma(fng.Normalized);

            Desenha(automato);

            lbDefinicao.Items.Clear();
            lbDefinicao.Items.Add(string.Format("M=({{{0}}}, {{{1}}}, P, {{{2}}}, {{{3}}}, {{{4}}})",
                   string.Join(",", automato.Alphabet.Select(a => a.Name).ToArray()),
                   string.Join(",", automato.ObterEstados()),
                   automato.StartState.Name,
                   string.Join(",", automato.ObterEstadosFinais()),
                   string.Join(",", automato.StackAlphabet.Select(a => a + "").ToArray())));
            lbDefinicao.Items.Add("P={");
            foreach (Transition transicao in transicoes)
            {
                lbDefinicao.Items.Add(string.Format("   P({0},{1}, {2}) = {{({3}, {4})}}",
                        transicao.Source.Name,
                        transicao.Symbol,
                        transicao.ConsumingStack[0],
                        transicao.Destiny.Name,
                        string.Join(",", transicao.PushStack[0].Name.ToArray())));
            }
            lbDefinicao.Items.Add("}");
        }
    }
}
