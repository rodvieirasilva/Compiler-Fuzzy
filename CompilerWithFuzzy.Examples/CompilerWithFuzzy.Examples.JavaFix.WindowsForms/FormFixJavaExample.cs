using CompilerWithFuzzy.Compiler;
using CompilerWithFuzzy.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilerWithFuzzy.Examples.JavaFix.WindowsForms
{
    public partial class FormFixJavaExample : Form
    {

        private CompilerXmlParser cxp;
        public FormFixJavaExample()
        {
            InitializeComponent();


            DateTime dataInicio = DateTime.Now;
            this.richTextConsulta.Text = @"package Exercicio;  
                                        public class Execicio1  
                                          {  
                                          public void main()  
                                           {  
                                              System.out.print(" + "\"Aprenda java\"" + @");  
                                           }
                                            } ";

            cxp = new CompilerXmlParser(".\\ExampleJavaFIX.xml");

            Utils.SaveTime("FinalTempoCriarJAVA", dataInicio);
        }



        private void linkLabel_Click(object sender, EventArgs e)
        {

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    richTextBoxFix.Text = string.Empty;
                    cxp.Compiler.Compile(richTextConsulta.Text);
                    richTextBoxFix.Text = cxp.Compiler.CodeModifySource;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
