namespace MessyLab
{
	partial class FindAndReplaceForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindAndReplaceForm));
			this.findWhatlabel = new System.Windows.Forms.Label();
			this.replaceWithLabel = new System.Windows.Forms.Label();
			this.findWhatTextBox = new System.Windows.Forms.TextBox();
			this.replaceWithTextBox = new System.Windows.Forms.TextBox();
			this.findNextButton = new System.Windows.Forms.Button();
			this.replaceButton = new System.Windows.Forms.Button();
			this.replaceAllButton = new System.Windows.Forms.Button();
			this.matchWholeWordCheckBox = new System.Windows.Forms.CheckBox();
			this.matchCaseCheckBox = new System.Windows.Forms.CheckBox();
			this.highlightAllButton = new System.Windows.Forms.Button();
			this.findPreviousButton = new System.Windows.Forms.Button();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.findToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.replaceToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.toolStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// findWhatlabel
			// 
			this.findWhatlabel.AutoSize = true;
			this.findWhatlabel.Location = new System.Drawing.Point(3, 0);
			this.findWhatlabel.Name = "findWhatlabel";
			this.findWhatlabel.Size = new System.Drawing.Size(56, 13);
			this.findWhatlabel.TabIndex = 0;
			this.findWhatlabel.Text = "Fi&nd what:";
			// 
			// replaceWithLabel
			// 
			this.replaceWithLabel.AutoSize = true;
			this.replaceWithLabel.Location = new System.Drawing.Point(3, 39);
			this.replaceWithLabel.Name = "replaceWithLabel";
			this.replaceWithLabel.Size = new System.Drawing.Size(72, 13);
			this.replaceWithLabel.TabIndex = 2;
			this.replaceWithLabel.Text = "Re&place with:";
			// 
			// findWhatTextBox
			// 
			this.findWhatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.findWhatTextBox, 2);
			this.findWhatTextBox.Location = new System.Drawing.Point(3, 16);
			this.findWhatTextBox.Name = "findWhatTextBox";
			this.findWhatTextBox.Size = new System.Drawing.Size(234, 20);
			this.findWhatTextBox.TabIndex = 1;
			this.findWhatTextBox.TextChanged += new System.EventHandler(this.findWhatTextBox_TextChanged);
			// 
			// replaceWithTextBox
			// 
			this.replaceWithTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.replaceWithTextBox, 2);
			this.replaceWithTextBox.Location = new System.Drawing.Point(3, 55);
			this.replaceWithTextBox.Name = "replaceWithTextBox";
			this.replaceWithTextBox.Size = new System.Drawing.Size(234, 20);
			this.replaceWithTextBox.TabIndex = 3;
			// 
			// findNextButton
			// 
			this.findNextButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.findNextButton.Location = new System.Drawing.Point(123, 151);
			this.findNextButton.Name = "findNextButton";
			this.findNextButton.Size = new System.Drawing.Size(114, 23);
			this.findNextButton.TabIndex = 6;
			this.findNextButton.Text = "&Find next";
			this.findNextButton.UseVisualStyleBackColor = true;
			this.findNextButton.Click += new System.EventHandler(this.findNextButton_Click);
			// 
			// replaceButton
			// 
			this.replaceButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.replaceButton.Location = new System.Drawing.Point(3, 209);
			this.replaceButton.Name = "replaceButton";
			this.replaceButton.Size = new System.Drawing.Size(114, 23);
			this.replaceButton.TabIndex = 7;
			this.replaceButton.Text = "&Replace";
			this.replaceButton.UseVisualStyleBackColor = true;
			this.replaceButton.Click += new System.EventHandler(this.replaceButton_Click);
			// 
			// replaceAllButton
			// 
			this.replaceAllButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.replaceAllButton.Location = new System.Drawing.Point(123, 209);
			this.replaceAllButton.Name = "replaceAllButton";
			this.replaceAllButton.Size = new System.Drawing.Size(114, 23);
			this.replaceAllButton.TabIndex = 9;
			this.replaceAllButton.Text = "Replace &All";
			this.replaceAllButton.UseVisualStyleBackColor = true;
			this.replaceAllButton.Click += new System.EventHandler(this.replaceAllButton_Click);
			// 
			// matchWholeWordCheckBox
			// 
			this.matchWholeWordCheckBox.AutoSize = true;
			this.matchWholeWordCheckBox.Location = new System.Drawing.Point(6, 42);
			this.matchWholeWordCheckBox.Name = "matchWholeWordCheckBox";
			this.matchWholeWordCheckBox.Size = new System.Drawing.Size(113, 17);
			this.matchWholeWordCheckBox.TabIndex = 5;
			this.matchWholeWordCheckBox.Text = "Match &whole word";
			this.matchWholeWordCheckBox.UseVisualStyleBackColor = true;
			// 
			// matchCaseCheckBox
			// 
			this.matchCaseCheckBox.AutoSize = true;
			this.matchCaseCheckBox.Location = new System.Drawing.Point(6, 19);
			this.matchCaseCheckBox.Name = "matchCaseCheckBox";
			this.matchCaseCheckBox.Size = new System.Drawing.Size(82, 17);
			this.matchCaseCheckBox.TabIndex = 4;
			this.matchCaseCheckBox.Text = "Match &case";
			this.matchCaseCheckBox.UseVisualStyleBackColor = true;
			// 
			// highlightAllButton
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.highlightAllButton, 2);
			this.highlightAllButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.highlightAllButton.Location = new System.Drawing.Point(3, 180);
			this.highlightAllButton.Name = "highlightAllButton";
			this.highlightAllButton.Size = new System.Drawing.Size(234, 23);
			this.highlightAllButton.TabIndex = 8;
			this.highlightAllButton.Text = "Find and Highlight &all";
			this.highlightAllButton.UseVisualStyleBackColor = true;
			this.highlightAllButton.Visible = false;
			this.highlightAllButton.Click += new System.EventHandler(this.highlightAllButton_Click);
			// 
			// findPreviousButton
			// 
			this.findPreviousButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.findPreviousButton.Location = new System.Drawing.Point(3, 151);
			this.findPreviousButton.Name = "findPreviousButton";
			this.findPreviousButton.Size = new System.Drawing.Size(114, 23);
			this.findPreviousButton.TabIndex = 6;
			this.findPreviousButton.Text = "Find pre&vious";
			this.findPreviousButton.UseVisualStyleBackColor = true;
			this.findPreviousButton.Click += new System.EventHandler(this.findPreviousButton_Click);
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripButton,
            this.replaceToolStripButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(240, 25);
			this.toolStrip1.TabIndex = 10;
			this.toolStrip1.Text = "toolStrip";
			// 
			// findToolStripButton
			// 
			this.findToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("findToolStripButton.Image")));
			this.findToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.findToolStripButton.Name = "findToolStripButton";
			this.findToolStripButton.Size = new System.Drawing.Size(50, 22);
			this.findToolStripButton.Text = "Find";
			this.findToolStripButton.Click += new System.EventHandler(this.findToolStripButton_Click);
			// 
			// replaceToolStripButton
			// 
			this.replaceToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("replaceToolStripButton.Image")));
			this.replaceToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.replaceToolStripButton.Name = "replaceToolStripButton";
			this.replaceToolStripButton.Size = new System.Drawing.Size(68, 22);
			this.replaceToolStripButton.Text = "Replace";
			this.replaceToolStripButton.Click += new System.EventHandler(this.replaceToolStripButton_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
			this.groupBox1.Controls.Add(this.matchCaseCheckBox);
			this.groupBox1.Controls.Add(this.matchWholeWordCheckBox);
			this.groupBox1.Location = new System.Drawing.Point(3, 81);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(234, 64);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Find options";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
			this.tableLayoutPanel1.Controls.Add(this.findWhatlabel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.highlightAllButton, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.replaceAllButton, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.findWhatTextBox, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.replaceButton, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.replaceWithLabel, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.replaceWithTextBox, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.findNextButton, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.findPreviousButton, 0, 5);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 8;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(240, 235);
			this.tableLayoutPanel1.TabIndex = 12;
			// 
			// FindAndReplaceForm
			// 
			this.AcceptButton = this.replaceButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(240, 260);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.toolStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindAndReplaceForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Find and replace";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindAndReplaceForm_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindAndReplaceForm_KeyDown);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label findWhatlabel;
		private System.Windows.Forms.Label replaceWithLabel;
		private System.Windows.Forms.TextBox findWhatTextBox;
		private System.Windows.Forms.TextBox replaceWithTextBox;
		private System.Windows.Forms.Button findNextButton;
		private System.Windows.Forms.Button replaceButton;
		private System.Windows.Forms.Button replaceAllButton;
		private System.Windows.Forms.CheckBox matchWholeWordCheckBox;
		private System.Windows.Forms.CheckBox matchCaseCheckBox;
		private System.Windows.Forms.Button highlightAllButton;
		private System.Windows.Forms.Button findPreviousButton;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton findToolStripButton;
		private System.Windows.Forms.ToolStripButton replaceToolStripButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}