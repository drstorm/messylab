namespace MessyLab
{
	partial class ListPad
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
			this.listView = new System.Windows.Forms.ListView();
			this.imageColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.nColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.descriptionColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.fileColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.lineColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.columnColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.imageColumnHeader,
            this.nColumnHeader,
            this.descriptionColumnHeader,
            this.fileColumnHeader,
            this.lineColumnHeader,
            this.columnColumnHeader});
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(875, 197);
			this.listView.TabIndex = 4;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.Resize += new System.EventHandler(this.listView_Resize);
			this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
			// 
			// imageColumnHeader
			// 
			this.imageColumnHeader.Text = "";
			this.imageColumnHeader.Width = 20;
			// 
			// nColumnHeader
			// 
			this.nColumnHeader.Text = "";
			this.nColumnHeader.Width = 30;
			// 
			// descriptionColumnHeader
			// 
			this.descriptionColumnHeader.Text = "Description";
			this.descriptionColumnHeader.Width = 600;
			// 
			// fileColumnHeader
			// 
			this.fileColumnHeader.Text = "File";
			this.fileColumnHeader.Width = 100;
			// 
			// lineColumnHeader
			// 
			this.lineColumnHeader.Text = "Line";
			// 
			// columnColumnHeader
			// 
			this.columnColumnHeader.Text = "Column";
			// 
			// ListPad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(875, 197);
			this.Controls.Add(this.listView);
			this.Name = "ListPad";
			this.DockStateChanged += new System.EventHandler(this.ListPad_DockStateChanged);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ColumnHeader imageColumnHeader;
		private System.Windows.Forms.ColumnHeader descriptionColumnHeader;
		private System.Windows.Forms.ColumnHeader fileColumnHeader;
		private System.Windows.Forms.ColumnHeader lineColumnHeader;
		private System.Windows.Forms.ColumnHeader columnColumnHeader;
		protected System.Windows.Forms.ListView listView;
		private System.Windows.Forms.ColumnHeader nColumnHeader;
	}
}
