namespace CompilerWithFuzzy.AutomaFuzzy.WindowsForm
{
    partial class FrmAutomaFuzzy
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
            this.pnlAutoma = new System.Windows.Forms.Panel();
            this.txtExpression = new System.Windows.Forms.TextBox();
            this.txtValidate = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbPass = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFinalPertinence = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAutoma
            // 
            this.pnlAutoma.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.pnlAutoma, 2);
            this.pnlAutoma.Location = new System.Drawing.Point(3, 48);
            this.pnlAutoma.Name = "pnlAutoma";
            this.pnlAutoma.Size = new System.Drawing.Size(676, 514);
            this.pnlAutoma.TabIndex = 1;
            // 
            // txtExpression
            // 
            this.txtExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExpression.Location = new System.Drawing.Point(3, 23);
            this.txtExpression.Name = "txtExpression";
            this.txtExpression.Size = new System.Drawing.Size(335, 20);
            this.txtExpression.TabIndex = 3;
            this.txtExpression.TextChanged += new System.EventHandler(this.txtExpression_TextChanged);
            // 
            // txtValidate
            // 
            this.txtValidate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValidate.Location = new System.Drawing.Point(344, 23);
            this.txtValidate.Name = "txtValidate";
            this.txtValidate.Size = new System.Drawing.Size(335, 20);
            this.txtValidate.TabIndex = 4;
            this.txtValidate.TextChanged += new System.EventHandler(this.txtValidate_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 377F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlAutoma, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbPass, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtValidate, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtExpression, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFinalPertinence, 2, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1059, 565);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(685, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(371, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Pertinence Final";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(344, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(335, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Word to validate";
            // 
            // lbPass
            // 
            this.lbPass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPass.FormattingEnabled = true;
            this.lbPass.Location = new System.Drawing.Point(685, 48);
            this.lbPass.Name = "lbPass";
            this.lbPass.Size = new System.Drawing.Size(371, 511);
            this.lbPass.TabIndex = 2;
            this.lbPass.SelectedIndexChanged += new System.EventHandler(this.lbPass_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Expression Fuzzy";
            // 
            // lblFinalPertinence
            // 
            this.lblFinalPertinence.AutoSize = true;
            this.lblFinalPertinence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinalPertinence.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalPertinence.Location = new System.Drawing.Point(685, 20);
            this.lblFinalPertinence.Name = "lblFinalPertinence";
            this.lblFinalPertinence.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFinalPertinence.Size = new System.Drawing.Size(371, 25);
            this.lblFinalPertinence.TabIndex = 8;
            this.lblFinalPertinence.Text = "0";
            // 
            // FrmAutomaFuzzy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 565);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmAutomaFuzzy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutomaFuzzy";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlAutoma;
        private System.Windows.Forms.TextBox txtExpression;
        private System.Windows.Forms.TextBox txtValidate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox lbPass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFinalPertinence;
    }
}

