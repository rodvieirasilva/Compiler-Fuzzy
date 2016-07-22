namespace CompilerWithFuzzy.Compiler.WindowsForm
{
    partial class UCColor
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
            this.lblColor = new System.Windows.Forms.Label();
            this.lblDesricao = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblColor
            // 
            this.lblColor.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblColor.Location = new System.Drawing.Point(0, 0);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(23, 22);
            this.lblColor.TabIndex = 0;
            // 
            // lblDesricao
            // 
            this.lblDesricao.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDesricao.Location = new System.Drawing.Point(23, 0);
            this.lblDesricao.Name = "lblDesricao";
            this.lblDesricao.Size = new System.Drawing.Size(103, 22);
            this.lblDesricao.TabIndex = 1;
            this.lblDesricao.Text = "Descrição";
            this.lblDesricao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UCColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblDesricao);
            this.Controls.Add(this.lblColor);
            this.Name = "UCColor";
            this.Size = new System.Drawing.Size(140, 22);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Label lblDesricao;
    }
}
