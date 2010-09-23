namespace MessyLab
{
	partial class BreakpointsPad
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BreakpointsPad));
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Memory Breakpoints", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Register Breakpoints", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("I/O Breakpoints", System.Windows.Forms.HorizontalAlignment.Left);
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.newToolStripSplitButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.memoryBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.registerBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.iOBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.listView = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripSplitButton,
            this.deleteToolStripButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(328, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// newToolStripSplitButton
			// 
			this.newToolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.memoryBreakpointToolStripMenuItem,
            this.registerBreakpointToolStripMenuItem,
            this.iOBreakpointToolStripMenuItem});
			this.newToolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripSplitButton.Image")));
			this.newToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripSplitButton.Name = "newToolStripSplitButton";
			this.newToolStripSplitButton.Size = new System.Drawing.Size(120, 22);
			this.newToolStripSplitButton.Text = "New Breakpoint";
			// 
			// memoryBreakpointToolStripMenuItem
			// 
			this.memoryBreakpointToolStripMenuItem.Name = "memoryBreakpointToolStripMenuItem";
			this.memoryBreakpointToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
			this.memoryBreakpointToolStripMenuItem.Text = "Memory Breakpoint";
			this.memoryBreakpointToolStripMenuItem.Click += new System.EventHandler(this.memoryBreakpointToolStripMenuItem_Click);
			// 
			// registerBreakpointToolStripMenuItem
			// 
			this.registerBreakpointToolStripMenuItem.Name = "registerBreakpointToolStripMenuItem";
			this.registerBreakpointToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
			this.registerBreakpointToolStripMenuItem.Text = "Register Breakpoint";
			this.registerBreakpointToolStripMenuItem.Click += new System.EventHandler(this.registerBreakpointToolStripMenuItem_Click);
			// 
			// iOBreakpointToolStripMenuItem
			// 
			this.iOBreakpointToolStripMenuItem.Name = "iOBreakpointToolStripMenuItem";
			this.iOBreakpointToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
			this.iOBreakpointToolStripMenuItem.Text = "I/O Breakpoint";
			this.iOBreakpointToolStripMenuItem.Click += new System.EventHandler(this.iOBreakpointToolStripMenuItem_Click);
			// 
			// deleteToolStripButton
			// 
			this.deleteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripButton.Image")));
			this.deleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.deleteToolStripButton.Name = "deleteToolStripButton";
			this.deleteToolStripButton.Size = new System.Drawing.Size(60, 22);
			this.deleteToolStripButton.Text = "Delete";
			this.deleteToolStripButton.Click += new System.EventHandler(this.deleteToolStripButton_Click);
			// 
			// listView
			// 
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.FullRowSelect = true;
			listViewGroup1.Header = "Memory Breakpoints";
			listViewGroup1.Name = "memory";
			listViewGroup2.Header = "Register Breakpoints";
			listViewGroup2.Name = "register";
			listViewGroup3.Header = "I/O Breakpoints";
			listViewGroup3.Name = "io";
			this.listView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(0, 25);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(328, 296);
			this.listView.TabIndex = 1;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Description";
			this.columnHeader1.Width = 200;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Flags";
			// 
			// BreakpointsPad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(328, 321);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.toolStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "BreakpointsPad";
			this.TabText = "Breakpoints";
			this.Text = "Breakpoints";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.ToolStripButton deleteToolStripButton;
		private System.Windows.Forms.ToolStripDropDownButton newToolStripSplitButton;
		private System.Windows.Forms.ToolStripMenuItem memoryBreakpointToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem registerBreakpointToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem iOBreakpointToolStripMenuItem;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
	}
}
