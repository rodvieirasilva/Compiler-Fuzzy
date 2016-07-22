namespace CompilerWithFuzzy.Compiler.WindowsForm
{
    partial class UCGraphLex
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
            this.flowLayoutPanelColor = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.listBoxPaths = new System.Windows.Forms.ListBox();
            this.flowLayoutPanelColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelColor
            // 
            this.flowLayoutPanelColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelColor.AutoScroll = true;
            this.flowLayoutPanelColor.Controls.Add(this.lblSubtitle);
            this.flowLayoutPanelColor.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelColor.Location = new System.Drawing.Point(419, 6);
            this.flowLayoutPanelColor.Name = "flowLayoutPanelColor";
            this.flowLayoutPanelColor.Size = new System.Drawing.Size(296, 351);
            this.flowLayoutPanelColor.TabIndex = 8;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Location = new System.Drawing.Point(3, 0);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(161, 20);
            this.lblSubtitle.TabIndex = 6;
            this.lblSubtitle.Text = "Colors";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlGraph
            // 
            this.pnlGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGraph.Location = new System.Drawing.Point(3, 3);
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Size = new System.Drawing.Size(410, 462);
            this.pnlGraph.TabIndex = 10;
            // 
            // listBoxPaths
            // 
            this.listBoxPaths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPaths.FormattingEnabled = true;
            this.listBoxPaths.HorizontalScrollbar = true;
            this.listBoxPaths.Location = new System.Drawing.Point(419, 363);
            this.listBoxPaths.Name = "listBoxPaths";
            this.listBoxPaths.Size = new System.Drawing.Size(293, 95);
            this.listBoxPaths.TabIndex = 11;
            this.listBoxPaths.SelectedIndexChanged += new System.EventHandler(this.listBoxPaths_SelectedIndexChanged);
            // 
            // UCGraphLex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBoxPaths);
            this.Controls.Add(this.flowLayoutPanelColor);
            this.Controls.Add(this.pnlGraph);
            this.Name = "UCGraphLex";
            this.Size = new System.Drawing.Size(715, 468);
            this.flowLayoutPanelColor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelColor;
        private System.Windows.Forms.Panel pnlGraph;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.ListBox listBoxPaths;
    }
}
