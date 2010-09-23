namespace MessyLab.Platforms.Pico
{
	partial class ConsolePad
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsolePad));
			this.queryLabel = new System.Windows.Forms.Label();
			this.inTextBox = new System.Windows.Forms.TextBox();
			this.enterButton = new System.Windows.Forms.Button();
			this.inputPanel = new System.Windows.Forms.Panel();
			this.outTextBox = new System.Windows.Forms.TextBox();
			this.inputPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// queryLabel
			// 
			this.queryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.queryLabel.AutoSize = true;
			this.queryLabel.Location = new System.Drawing.Point(3, 6);
			this.queryLabel.Name = "queryLabel";
			this.queryLabel.Size = new System.Drawing.Size(0, 13);
			this.queryLabel.TabIndex = 0;
			// 
			// inTextBox
			// 
			this.inTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.inTextBox.Location = new System.Drawing.Point(3, 22);
			this.inTextBox.Name = "inTextBox";
			this.inTextBox.Size = new System.Drawing.Size(246, 22);
			this.inTextBox.TabIndex = 1;
			// 
			// enterButton
			// 
			this.enterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.enterButton.Image = ((System.Drawing.Image)(resources.GetObject("enterButton.Image")));
			this.enterButton.Location = new System.Drawing.Point(255, 22);
			this.enterButton.Name = "enterButton";
			this.enterButton.Size = new System.Drawing.Size(26, 23);
			this.enterButton.TabIndex = 2;
			this.enterButton.UseVisualStyleBackColor = true;
			this.enterButton.Click += new System.EventHandler(this.enterButton_Click);
			// 
			// inputPanel
			// 
			this.inputPanel.Controls.Add(this.enterButton);
			this.inputPanel.Controls.Add(this.inTextBox);
			this.inputPanel.Controls.Add(this.queryLabel);
			this.inputPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.inputPanel.Location = new System.Drawing.Point(0, 340);
			this.inputPanel.Name = "inputPanel";
			this.inputPanel.Size = new System.Drawing.Size(284, 48);
			this.inputPanel.TabIndex = 4;
			this.inputPanel.Visible = false;
			// 
			// outTextBox
			// 
			this.outTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.outTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outTextBox.Location = new System.Drawing.Point(0, 0);
			this.outTextBox.Multiline = true;
			this.outTextBox.Name = "outTextBox";
			this.outTextBox.ReadOnly = true;
			this.outTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.outTextBox.Size = new System.Drawing.Size(284, 340);
			this.outTextBox.TabIndex = 5;
			// 
			// ConsolePad
			// 
			this.AcceptButton = this.enterButton;
			this.AutoHidePortion = 300;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(284, 388);
			this.Controls.Add(this.outTextBox);
			this.Controls.Add(this.inputPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ConsolePad";
			this.TabText = "Console";
			this.Text = "Console";
			this.inputPanel.ResumeLayout(false);
			this.inputPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label queryLabel;
		private System.Windows.Forms.TextBox inTextBox;
		private System.Windows.Forms.Button enterButton;
		private System.Windows.Forms.Panel inputPanel;
		private System.Windows.Forms.TextBox outTextBox;
	}
}
