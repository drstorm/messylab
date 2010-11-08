namespace MessyLab
{
	partial class WatchPad
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatchPad));
			this.listView = new System.Windows.Forms.ListView();
			this.nameColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.valueColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.typeColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.addToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.valueColumnHeader,
            this.typeColumnHeader});
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.FullRowSelect = true;
			this.listView.Location = new System.Drawing.Point(0, 25);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(284, 364);
			this.listView.TabIndex = 8;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
			this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
			// 
			// nameColumnHeader
			// 
			this.nameColumnHeader.Text = "Name";
			this.nameColumnHeader.Width = 130;
			// 
			// valueColumnHeader
			// 
			this.valueColumnHeader.Text = "Value";
			this.valueColumnHeader.Width = 80;
			// 
			// typeColumnHeader
			// 
			this.typeColumnHeader.Text = "Type";
			this.typeColumnHeader.Width = 70;
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripButton,
            this.deleteToolStripButton});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(284, 25);
			this.toolStrip.TabIndex = 7;
			this.toolStrip.Text = "toolStrip1";
			// 
			// addToolStripButton
			// 
			this.addToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addToolStripButton.Image")));
			this.addToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.addToolStripButton.Name = "addToolStripButton";
			this.addToolStripButton.Size = new System.Drawing.Size(88, 22);
			this.addToolStripButton.Text = "New Watch";
			this.addToolStripButton.Click += new System.EventHandler(this.addToolStripButton_Click);
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
			// WatchPad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(284, 389);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.toolStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "WatchPad";
			this.TabText = "Watch";
			this.Text = "Watch";
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.ColumnHeader nameColumnHeader;
		private System.Windows.Forms.ColumnHeader valueColumnHeader;
		private System.Windows.Forms.ColumnHeader typeColumnHeader;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton addToolStripButton;
		private System.Windows.Forms.ToolStripButton deleteToolStripButton;
	}
}
