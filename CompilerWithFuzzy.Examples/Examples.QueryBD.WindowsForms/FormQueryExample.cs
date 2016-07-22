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

namespace CompilerWithFuzzy.Examples.QueryBD.WindowsForms
{
    public partial class FormQueryExample : Form
    {
        private CompilerXmlParser cxp;
        private static Dictionary<string, Func<CompilerWithFuzzy.Compiler.Syn.ParseTree.Container, string>> DicCompileExample { get; set; }
        public FormQueryExample()
        {
            InitializeComponent();

            linkLabel.Visible = false;
            this.richTextConsulta.Text = "Cliente sendo 1 maior que Codigo";

            cxp = new CompilerXmlParser(".\\ExampleSQL1.xml");
        }

        public string CompilarCampo(CompilerWithFuzzy.Compiler.Syn.ParseTree.Container c)
        {
            return c["nomecampo"].Value.ToLower() == "codigo" ? "ID as Codigo" : c["nomecampo"].Value;
        }

        public string CompilarCampoSemAlias(CompilerWithFuzzy.Compiler.Syn.ParseTree.Container c)
        {
            return c["nomecampo"].Value.ToLower() == "codigo" ? "ID" : c["nomecampo"].Value;
        }

        public string CompilarInitial(CompilerWithFuzzy.Compiler.Syn.ParseTree.Container c)
        {

            if (c.ContainsKey("campo") && c.ContainsKey("tabela") && c.ContainsKey("condicao"))
            {
                return string.Format("SELECT {0} FROM {1} {2}",
                      DicCompileExample[c["campo"].TypeName](c["campo"]),
                      DicCompileExample[c["tabela"].TypeName](c["tabela"]), c.ContainsKey("condicao") ?
                      DicCompileExample[c["condicao"].TypeName](c["condicao"]) : string.Empty);
            }
            else if (c.ContainsKey("tabela") && c.ContainsKey("condicao"))
            {
                return string.Format("SELECT {0} FROM {1} {2}",
                      "*",
                      DicCompileExample[c["tabela"].TypeName](c["tabela"]), c.ContainsKey("condicao") ?
                      DicCompileExample[c["condicao"].TypeName](c["condicao"]) : string.Empty);
            }
            else
            {
                return "SELECT * FROM " + DicCompileExample[c["tabela"].TypeName](c["tabela"]);
            }
        }


        public string Condicao(CompilerWithFuzzy.Compiler.Syn.ParseTree.Container c)
        {
            return string.Format("WHERE {0} > {1}",
                  CompilarCampoSemAlias(c["campo"]),
                  c["numero"].Value);
        }

        private void linkLabel_Click(object sender, EventArgs e)
        {
            richTextConsulta.Text = linkLabel.Text;
            linkLabel.Visible = false;
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DicCompileExample = new Dictionary<string, Func<CompilerWithFuzzy.Compiler.Syn.ParseTree.Container, string>>();

                DicCompileExample.Add("Tabela", c => string.Format("{0}", c["nometabela"].Value));

                DicCompileExample.Add("Campo", CompilarCampo);

                DicCompileExample.Add("Initial", CompilarInitial);

                DicCompileExample.Add("Condicao", Condicao);

                cxp.Compiler.DicCompile = DicCompileExample;


                try
                {
                    cxp.Compiler.Compile(richTextConsulta.Text);

                    if (!string.IsNullOrEmpty(cxp.Compiler.CodeCompiled))
                    {
                        linkLabel.Text = "Correto: " + cxp.Compiler.CodeModifySource;

                        if (cxp.Compiler.CodeModifySource.ToLower() != richTextConsulta.Text.ToLower())
                        {
                            linkLabel.Visible = true;
                        }
                        else
                        {
                            linkLabel.Visible = false;
                        }


                        try
                        {
                            using (System.Data.SQLite.SQLiteConnection sql = new System.Data.SQLite.SQLiteConnection("data source=Banco; Version=3;"))
                            {
                                sql.Open();
                                using (var command = sql.CreateCommand())
                                {
                                    command.CommandText = cxp.Compiler.CodeCompiled;
                                    using (var reader = command.ExecuteReader())
                                    {
                                        DataTable dt = new DataTable();
                                        dt.Load(reader);

                                        dgvResultado.DataSource = dt;
                                        dgvResultado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


                                        lblMensagem.Text = "Consulta realizada com sucesso!";
                                        lblMensagem.ForeColor = Color.Green;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMensagem.Text = "Erro ao executar consulta! Msg: " + ex.ToString();
                            lblMensagem.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Verifique sua consulta!";
                        lblMensagem.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Verifique sua consulta!";
                    lblMensagem.ForeColor = Color.Red;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
