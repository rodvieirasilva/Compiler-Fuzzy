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

namespace CompilerWithFuzzy.Compiler.WindowsForm
{
    public partial class UCTableSyn : UserControl
    {
        private SyntacticAnalysisAbstract syn;


        public UCTableSyn(CompilerFuzzy compile)
            : this(compile.Syn)
        {
            compile.OnCompile += OnCompile;
        }

        public UCTableSyn(SyntacticAnalysisAbstract syn)
        {
            InitializeComponent();
            this.syn = syn;
            Atualizar();

        }

        public void OnCompile(CompilerFuzzy compile)
        {
            syn = compile.Syn;
            Atualizar();
        }
        public void Atualizar()
        {
            if (syn != null)
            {
                this.dataGridView.DataSource = syn.GetDataTable();
                this.Dock = DockStyle.Fill;
                foreach (var col in this.dataGridView.Columns)
                {
                    (col as DataGridViewColumn).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }
        }
    }
}
