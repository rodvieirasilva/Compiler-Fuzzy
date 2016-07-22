using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompilerWithFuzzy.GrammarFuzzy;
using CompilerWithFuzzy.Compiler;
using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.Core;

namespace CompilerWithFuzzy.Compiler.WindowsForm
{
public partial class UCGraphSynToCode : UserControl
    {
        private CompilerFuzzy comp;


        public UCGraphSynToCode(CompilerFuzzy comp)
        {
            this.comp = comp;
            InitializeComponent();
            //this.richTextBoxCodeIn.Text = "get dog ball";////"x=x";"dog get ball";
            this.richTextBoxCodeIn.Text = "dog get ball";////"x=x";"dog get ball";
            //this.richTextBoxCodeIn.Text = "can people";////"x=x";"dog get ball";

            this.Dock = DockStyle.Fill;
            // try
            {
                richTextBoxCodeIn_TextChanged(null, null);
                
            }
            //catch (Exception ex)
            { }
        }

       

        private void richTextBoxCodeIn_TextChanged(object sender, EventArgs e)
        {
            //try
            // {
            richTextBoxCodeOut.Clear();
            comp.Compile(richTextBoxCodeIn.Text);

            if (comp.Syn.GraphsSyntactic != null && comp.Syn.GraphsSyntactic.Count > 0)
            {
                richTextBoxCodeOut.Text = comp.CodeModifySource;

                richTextBoxCompiled.Text = comp.CodeCompiled;


                lblPertinenceLex.Text = comp.PertinenceLex.ToString();
                lblPertinenceLex.ForeColor = Utils.GetColor(comp.PertinenceLex);
                lblPertinenceSyn.Text = comp.PertinenceSyn.ToString();
                lblPertinenceSyn.ForeColor = Utils.GetColor(comp.PertinenceSyn);
                lblPertinenceTotal.Text = comp.PertinenceTotal.ToString();
                lblPertinenceTotal.ForeColor = Utils.GetColor(comp.PertinenceTotal);
            }


            // }
            // catch (Exception ex)
            // { }

        }
    }
}
