namespace CompilerWithFuzzy.Compiler.WindowsForm
{
    partial class FrmCompilerWithFuzzy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSyn = new System.Windows.Forms.TabPage();
            this.tabControlSyn = new System.Windows.Forms.TabControl();
            this.tabPageCode = new System.Windows.Forms.TabPage();
            this.tabPageTree = new System.Windows.Forms.TabPage();
            this.tabPageAutoma = new System.Windows.Forms.TabPage();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.tabPageLex = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageSyn.SuspendLayout();
            this.tabControlSyn.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.60452F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.39548F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.965065F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98.03493F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(820, 458);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // tabControl
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl, 3);
            this.tabControl.Controls.Add(this.tabPageSyn);
            this.tabControl.Controls.Add(this.tabPageLex);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(3, 11);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(814, 444);
            this.tabControl.TabIndex = 7;
            // 
            // tabPageSyn
            // 
            this.tabPageSyn.Controls.Add(this.tabControlSyn);
            this.tabPageSyn.Location = new System.Drawing.Point(4, 22);
            this.tabPageSyn.Name = "tabPageSyn";
            this.tabPageSyn.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSyn.Size = new System.Drawing.Size(806, 418);
            this.tabPageSyn.TabIndex = 1;
            this.tabPageSyn.Text = "Syn";
            this.tabPageSyn.UseVisualStyleBackColor = true;
            // 
            // tabControlSyn
            // 
            this.tabControlSyn.Controls.Add(this.tabPageCode);
            this.tabControlSyn.Controls.Add(this.tabPageTree);
            this.tabControlSyn.Controls.Add(this.tabPageAutoma);
            this.tabControlSyn.Controls.Add(this.tabPageData);
            this.tabControlSyn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSyn.Location = new System.Drawing.Point(3, 3);
            this.tabControlSyn.Name = "tabControlSyn";
            this.tabControlSyn.SelectedIndex = 0;
            this.tabControlSyn.Size = new System.Drawing.Size(800, 412);
            this.tabControlSyn.TabIndex = 0;
            // 
            // tabPageCode
            // 
            this.tabPageCode.Location = new System.Drawing.Point(4, 22);
            this.tabPageCode.Name = "tabPageCode";
            this.tabPageCode.Size = new System.Drawing.Size(792, 386);
            this.tabPageCode.TabIndex = 3;
            this.tabPageCode.Text = "Code";
            this.tabPageCode.UseVisualStyleBackColor = true;
            // 
            // tabPageTree
            // 
            this.tabPageTree.Location = new System.Drawing.Point(4, 22);
            this.tabPageTree.Name = "tabPageTree";
            this.tabPageTree.Size = new System.Drawing.Size(792, 386);
            this.tabPageTree.TabIndex = 2;
            this.tabPageTree.Text = "Tree";
            this.tabPageTree.UseVisualStyleBackColor = true;
            // 
            // tabPageAutoma
            // 
            this.tabPageAutoma.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutoma.Name = "tabPageAutoma";
            this.tabPageAutoma.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutoma.Size = new System.Drawing.Size(792, 386);
            this.tabPageAutoma.TabIndex = 0;
            this.tabPageAutoma.Text = "Automa";
            this.tabPageAutoma.UseVisualStyleBackColor = true;
            // 
            // tabPageData
            // 
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageData.Size = new System.Drawing.Size(792, 386);
            this.tabPageData.TabIndex = 1;
            this.tabPageData.Text = "Table";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // tabPageLex
            // 
            this.tabPageLex.Location = new System.Drawing.Point(4, 22);
            this.tabPageLex.Name = "tabPageLex";
            this.tabPageLex.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLex.Size = new System.Drawing.Size(797, 418);
            this.tabPageLex.TabIndex = 0;
            this.tabPageLex.Text = "Lex";
            this.tabPageLex.UseVisualStyleBackColor = true;
            // 
            // FrmCompilerWithFuzzy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 458);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmCompilerWithFuzzy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CompilerWithFuzzy";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageSyn.ResumeLayout(false);
            this.tabControlSyn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSyn;
        private System.Windows.Forms.TabPage tabPageLex;
        private System.Windows.Forms.TabControl tabControlSyn;
        private System.Windows.Forms.TabPage tabPageAutoma;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.TabPage tabPageTree;
        private System.Windows.Forms.TabPage tabPageCode;

    }
}

