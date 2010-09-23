namespace MessyLab
{
	partial class TextEditorForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextEditorForm));
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toggleBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.statusToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.lineToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.colToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.insToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.infoTextEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
			this.textEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
			this.tabContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.tabContextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.toggleBreakpointToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(172, 98);
			this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
			this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.cutToolStripMenuItem.Text = "Cu&t";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.copyToolStripMenuItem.Text = "&Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
			this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.pasteToolStripMenuItem.Text = "&Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(168, 6);
			// 
			// toggleBreakpointToolStripMenuItem
			// 
			this.toggleBreakpointToolStripMenuItem.Name = "toggleBreakpointToolStripMenuItem";
			this.toggleBreakpointToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.toggleBreakpointToolStripMenuItem.Text = "Toggle Breakpoint";
			this.toggleBreakpointToolStripMenuItem.Click += new System.EventHandler(this.toggleBreakpointToolStripMenuItem_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripStatusLabel,
            this.lineToolStripStatusLabel,
            this.colToolStripStatusLabel,
            this.insToolStripStatusLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 603);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
			this.statusStrip.Size = new System.Drawing.Size(845, 22);
			this.statusStrip.SizingGrip = false;
			this.statusStrip.TabIndex = 4;
			// 
			// statusToolStripStatusLabel
			// 
			this.statusToolStripStatusLabel.Name = "statusToolStripStatusLabel";
			this.statusToolStripStatusLabel.Size = new System.Drawing.Size(638, 17);
			this.statusToolStripStatusLabel.Spring = true;
			// 
			// lineToolStripStatusLabel
			// 
			this.lineToolStripStatusLabel.AutoSize = false;
			this.lineToolStripStatusLabel.Name = "lineToolStripStatusLabel";
			this.lineToolStripStatusLabel.Size = new System.Drawing.Size(64, 17);
			this.lineToolStripStatusLabel.Text = "Ln 1";
			this.lineToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// colToolStripStatusLabel
			// 
			this.colToolStripStatusLabel.AutoSize = false;
			this.colToolStripStatusLabel.Name = "colToolStripStatusLabel";
			this.colToolStripStatusLabel.Size = new System.Drawing.Size(64, 17);
			this.colToolStripStatusLabel.Text = "Col 1";
			this.colToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// insToolStripStatusLabel
			// 
			this.insToolStripStatusLabel.AutoSize = false;
			this.insToolStripStatusLabel.Name = "insToolStripStatusLabel";
			this.insToolStripStatusLabel.Size = new System.Drawing.Size(64, 17);
			this.insToolStripStatusLabel.Text = "INS";
			this.insToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// infoTextEditorControl
			// 
			this.infoTextEditorControl.Dock = System.Windows.Forms.DockStyle.Left;
			this.infoTextEditorControl.IsReadOnly = false;
			this.infoTextEditorControl.Location = new System.Drawing.Point(0, 0);
			this.infoTextEditorControl.Name = "infoTextEditorControl";
			this.infoTextEditorControl.ShowLineNumbers = false;
			this.infoTextEditorControl.Size = new System.Drawing.Size(249, 603);
			this.infoTextEditorControl.TabIndex = 5;
			this.infoTextEditorControl.TabStop = false;
			this.infoTextEditorControl.Visible = false;
			// 
			// textEditorControl
			// 
			this.textEditorControl.ContextMenuStrip = this.contextMenuStrip;
			this.textEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditorControl.IsIconBarVisible = true;
			this.textEditorControl.IsReadOnly = false;
			this.textEditorControl.Location = new System.Drawing.Point(249, 0);
			this.textEditorControl.Name = "textEditorControl";
			this.textEditorControl.Size = new System.Drawing.Size(596, 603);
			this.textEditorControl.TabIndex = 6;
			// 
			// tabContextMenuStrip
			// 
			this.tabContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.printToolStripMenuItem});
			this.tabContextMenuStrip.Name = "tabContextMenuStrip";
			this.tabContextMenuStrip.Size = new System.Drawing.Size(104, 76);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.closeToolStripMenuItem.Text = "&Close";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
			this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// printToolStripMenuItem
			// 
			this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
			this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			this.printToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.printToolStripMenuItem.Text = "&Print";
			this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
			// 
			// TextEditorForm
			// 
			this.ClientSize = new System.Drawing.Size(845, 625);
			this.Controls.Add(this.textEditorControl);
			this.Controls.Add(this.infoTextEditorControl);
			this.Controls.Add(this.statusStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TextEditorForm";
			this.TabPageContextMenuStrip = this.tabContextMenuStrip;
			this.Deactivate += new System.EventHandler(this.TextEditorForm_Deactivate);
			this.Load += new System.EventHandler(this.TextEditorForm_Load);
			this.Activated += new System.EventHandler(this.TextEditorForm_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextEditorForm_FormClosing);
			this.contextMenuStrip.ResumeLayout(false);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.tabContextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem toggleBreakpointToolStripMenuItem;
		protected System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel lineToolStripStatusLabel;
		private System.Windows.Forms.ToolStripStatusLabel colToolStripStatusLabel;
		private System.Windows.Forms.ToolStripStatusLabel insToolStripStatusLabel;
		protected ICSharpCode.TextEditor.TextEditorControl infoTextEditorControl;
		protected ICSharpCode.TextEditor.TextEditorControl textEditorControl;
		private System.Windows.Forms.ToolStripStatusLabel statusToolStripStatusLabel;
		private System.Windows.Forms.ContextMenuStrip tabContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
	}
}
