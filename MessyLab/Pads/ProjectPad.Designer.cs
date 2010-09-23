namespace MessyLab
{
	partial class ProjectPad
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectPad));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.addNewFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addExistingFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.listView = new System.Windows.Forms.ListView();
			this.Main = new System.Windows.Forms.ColumnHeader();
			this.Filename = new System.Windows.Forms.ColumnHeader();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setAsMainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.removeToolStripButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(284, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip";
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewFileToolStripMenuItem,
            this.addExistingFileToolStripMenuItem});
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(58, 22);
			this.toolStripDropDownButton1.Text = "Add";
			// 
			// addNewFileToolStripMenuItem
			// 
			this.addNewFileToolStripMenuItem.Name = "addNewFileToolStripMenuItem";
			this.addNewFileToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.addNewFileToolStripMenuItem.Text = "Add New File...";
			this.addNewFileToolStripMenuItem.Click += new System.EventHandler(this.addNewFileToolStripMenuItem_Click);
			// 
			// addExistingFileToolStripMenuItem
			// 
			this.addExistingFileToolStripMenuItem.Name = "addExistingFileToolStripMenuItem";
			this.addExistingFileToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.addExistingFileToolStripMenuItem.Text = "Add Existing File...";
			this.addExistingFileToolStripMenuItem.Click += new System.EventHandler(this.addExistingFileToolStripMenuItem_Click);
			// 
			// removeToolStripButton
			// 
			this.removeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripButton.Image")));
			this.removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.removeToolStripButton.Name = "removeToolStripButton";
			this.removeToolStripButton.Size = new System.Drawing.Size(70, 22);
			this.removeToolStripButton.Text = "Remove";
			this.removeToolStripButton.Click += new System.EventHandler(this.removeToolStripButton_Click);
			// 
			// listView
			// 
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Main,
            this.Filename});
			this.listView.ContextMenuStrip = this.contextMenuStrip;
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.FullRowSelect = true;
			this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView.Location = new System.Drawing.Point(0, 25);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(284, 460);
			this.listView.TabIndex = 1;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
			this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
			this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
			// 
			// Main
			// 
			this.Main.Text = "Main";
			this.Main.Width = 39;
			// 
			// Filename
			// 
			this.Filename.Text = "Filename";
			this.Filename.Width = 207;
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.setAsMainToolStripMenuItem,
            this.toolStripMenuItem1,
            this.removeToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(135, 76);
			this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// setAsMainToolStripMenuItem
			// 
			this.setAsMainToolStripMenuItem.Name = "setAsMainToolStripMenuItem";
			this.setAsMainToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.setAsMainToolStripMenuItem.Text = "Set as Main";
			this.setAsMainToolStripMenuItem.Click += new System.EventHandler(this.setAsMainToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(131, 6);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripMenuItem.Image")));
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.removeToolStripMenuItem.Text = "Remove";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// ProjectPad
			// 
			this.AutoHidePortion = 300;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(284, 485);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.toolStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ProjectPad";
			this.TabText = "Project Explorer";
			this.Text = "Project Explorer";
			this.Load += new System.EventHandler(this.ProjectPad_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.ColumnHeader Main;
		private System.Windows.Forms.ColumnHeader Filename;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem addNewFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addExistingFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton removeToolStripButton;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setAsMainToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
	}
}
