using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilerWithFuzzy.Compiler.WindowsForm
{
 public partial class UCColor : UserControl
    {
   public UCColor(string text, Color color)
        {
            InitializeComponent();

            lblColor.BackColor = color;
            lblDesricao.Text = text;
        }
    }
}
