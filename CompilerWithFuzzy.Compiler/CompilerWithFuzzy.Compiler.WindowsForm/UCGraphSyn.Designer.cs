namespace CompilerWithFuzzy.Compiler.WindowsForm
{
    partial class UCGraphSyn
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
            this.splitContainerSyn = new System.Windows.Forms.SplitContainer();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.treeViewSyn = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSyn)).BeginInit();
            this.splitContainerSyn.Panel1.SuspendLayout();
            this.splitContainerSyn.Panel2.SuspendLayout();
            this.splitContainerSyn.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerSyn
            // 
            this.splitContainerSyn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSyn.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSyn.Name = "splitContainerSyn";
            // 
            // splitContainerSyn.Panel1
            // 
            this.splitContainerSyn.Panel1.Controls.Add(this.treeViewSyn);
            // 
            // splitContainerSyn.Panel2
            // 
            this.splitContainerSyn.Panel2.Controls.Add(this.pnlGraph);
            this.splitContainerSyn.Size = new System.Drawing.Size(643, 333);
            this.splitContainerSyn.SplitterDistance = 205;
            this.splitContainerSyn.TabIndex = 2;
            // 
            // pnlGraph
            // 
            this.pnlGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraph.Location = new System.Drawing.Point(0, 0);
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Size = new System.Drawing.Size(434, 333);
            this.pnlGraph.TabIndex = 2;
            // 
            // treeViewSyn
            // 
            this.treeViewSyn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSyn.Location = new System.Drawing.Point(0, 0);
            this.treeViewSyn.Name = "treeViewSyn";
            this.treeViewSyn.Size = new System.Drawing.Size(205, 333);
            this.treeViewSyn.TabIndex = 0;
            // 
            // UCGraphSyn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerSyn);
            this.Name = "UCGraphSyn";
            this.Size = new System.Drawing.Size(643, 333);
            this.splitContainerSyn.Panel1.ResumeLayout(false);
            this.splitContainerSyn.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSyn)).EndInit();
            this.splitContainerSyn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerSyn;
        private System.Windows.Forms.TreeView treeViewSyn;
        private System.Windows.Forms.Panel pnlGraph;

    }
}
