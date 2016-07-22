namespace CompilerWithFuzzy.Compiler.WindowsForm
{
    partial class UCGraphSynToCode
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainerCodes = new System.Windows.Forms.SplitContainer();
            this.richTextBoxCodeIn = new System.Windows.Forms.RichTextBox();
            this.splitContainerResults = new System.Windows.Forms.SplitContainer();
            this.richTextBoxCodeOut = new System.Windows.Forms.RichTextBox();
            this.richTextBoxCompiled = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPertinenceLex = new System.Windows.Forms.Label();
            this.lblPertinenceSyn = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPertinenceTotal = new System.Windows.Forms.Label();
            this.lblPertinenceTotalDesc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCodes)).BeginInit();
            this.splitContainerCodes.Panel1.SuspendLayout();
            this.splitContainerCodes.Panel2.SuspendLayout();
            this.splitContainerCodes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerResults)).BeginInit();
            this.splitContainerResults.Panel1.SuspendLayout();
            this.splitContainerResults.Panel2.SuspendLayout();
            this.splitContainerResults.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerCodes
            // 
            this.splitContainerCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCodes.Location = new System.Drawing.Point(0, 0);
            this.splitContainerCodes.Name = "splitContainerCodes";
            // 
            // splitContainerCodes.Panel1
            // 
            this.splitContainerCodes.Panel1.Controls.Add(this.richTextBoxCodeIn);
            // 
            // splitContainerCodes.Panel2
            // 
            this.splitContainerCodes.Panel2.Controls.Add(this.splitContainerResults);
            this.splitContainerCodes.Size = new System.Drawing.Size(1008, 491);
            this.splitContainerCodes.SplitterDistance = 498;
            this.splitContainerCodes.TabIndex = 2;
            // 
            // richTextBoxCodeIn
            // 
            this.richTextBoxCodeIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxCodeIn.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxCodeIn.Name = "richTextBoxCodeIn";
            this.richTextBoxCodeIn.Size = new System.Drawing.Size(498, 491);
            this.richTextBoxCodeIn.TabIndex = 1;
            this.richTextBoxCodeIn.Text = "";
            this.richTextBoxCodeIn.TextChanged += new System.EventHandler(this.richTextBoxCodeIn_TextChanged);
            // 
            // splitContainerResults
            // 
            this.splitContainerResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerResults.Location = new System.Drawing.Point(0, 0);
            this.splitContainerResults.Name = "splitContainerResults";
            this.splitContainerResults.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerResults.Panel1
            // 
            this.splitContainerResults.Panel1.Controls.Add(this.richTextBoxCodeOut);
            // 
            // splitContainerResults.Panel2
            // 
            this.splitContainerResults.Panel2.Controls.Add(this.panel1);
            this.splitContainerResults.Panel2.Controls.Add(this.richTextBoxCompiled);
            this.splitContainerResults.Size = new System.Drawing.Size(506, 491);
            this.splitContainerResults.SplitterDistance = 223;
            this.splitContainerResults.TabIndex = 3;
            // 
            // richTextBoxCodeOut
            // 
            this.richTextBoxCodeOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxCodeOut.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxCodeOut.Name = "richTextBoxCodeOut";
            this.richTextBoxCodeOut.Size = new System.Drawing.Size(506, 223);
            this.richTextBoxCodeOut.TabIndex = 3;
            this.richTextBoxCodeOut.Text = "";
            // 
            // richTextBoxCompiled
            // 
            this.richTextBoxCompiled.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxCompiled.Location = new System.Drawing.Point(0, 30);
            this.richTextBoxCompiled.Name = "richTextBoxCompiled";
            this.richTextBoxCompiled.Size = new System.Drawing.Size(506, 234);
            this.richTextBoxCompiled.TabIndex = 4;
            this.richTextBoxCompiled.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblPertinenceTotal);
            this.panel1.Controls.Add(this.lblPertinenceTotalDesc);
            this.panel1.Controls.Add(this.lblPertinenceSyn);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblPertinenceLex);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(506, 24);
            this.panel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Pertinence Lex:";
            // 
            // lblPertinenceLex
            // 
            this.lblPertinenceLex.AutoSize = true;
            this.lblPertinenceLex.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPertinenceLex.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPertinenceLex.Location = new System.Drawing.Point(81, 0);
            this.lblPertinenceLex.Name = "lblPertinenceLex";
            this.lblPertinenceLex.Size = new System.Drawing.Size(40, 17);
            this.lblPertinenceLex.TabIndex = 7;
            this.lblPertinenceLex.Text = "0,00";
            // 
            // lblPertinenceSyn
            // 
            this.lblPertinenceSyn.AutoSize = true;
            this.lblPertinenceSyn.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPertinenceSyn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPertinenceSyn.Location = new System.Drawing.Point(203, 0);
            this.lblPertinenceSyn.Name = "lblPertinenceSyn";
            this.lblPertinenceSyn.Size = new System.Drawing.Size(40, 17);
            this.lblPertinenceSyn.TabIndex = 9;
            this.lblPertinenceSyn.Text = "0,00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(121, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Pertinence Syn:";
            // 
            // lblPertinenceTotal
            // 
            this.lblPertinenceTotal.AutoSize = true;
            this.lblPertinenceTotal.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPertinenceTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPertinenceTotal.Location = new System.Drawing.Point(331, 0);
            this.lblPertinenceTotal.Name = "lblPertinenceTotal";
            this.lblPertinenceTotal.Size = new System.Drawing.Size(40, 17);
            this.lblPertinenceTotal.TabIndex = 11;
            this.lblPertinenceTotal.Text = "0,00";
            // 
            // lblPertinenceTotalDesc
            // 
            this.lblPertinenceTotalDesc.AutoSize = true;
            this.lblPertinenceTotalDesc.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPertinenceTotalDesc.Location = new System.Drawing.Point(243, 0);
            this.lblPertinenceTotalDesc.Name = "lblPertinenceTotalDesc";
            this.lblPertinenceTotalDesc.Size = new System.Drawing.Size(88, 13);
            this.lblPertinenceTotalDesc.TabIndex = 10;
            this.lblPertinenceTotalDesc.Text = "Pertinence Total:";
            // 
            // UCGraphSynToCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerCodes);
            this.Name = "UCGraphSynToCode";
            this.Size = new System.Drawing.Size(1008, 491);
            this.splitContainerCodes.Panel1.ResumeLayout(false);
            this.splitContainerCodes.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCodes)).EndInit();
            this.splitContainerCodes.ResumeLayout(false);
            this.splitContainerResults.Panel1.ResumeLayout(false);
            this.splitContainerResults.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerResults)).EndInit();
            this.splitContainerResults.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerCodes;
        private System.Windows.Forms.RichTextBox richTextBoxCodeIn;
        private System.Windows.Forms.SplitContainer splitContainerResults;
        private System.Windows.Forms.RichTextBox richTextBoxCodeOut;
        private System.Windows.Forms.RichTextBox richTextBoxCompiled;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblPertinenceTotal;
        private System.Windows.Forms.Label lblPertinenceTotalDesc;
        private System.Windows.Forms.Label lblPertinenceSyn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPertinenceLex;
        private System.Windows.Forms.Label label1;
    }
}
