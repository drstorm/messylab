namespace MessyLab
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.okButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.listBox = new System.Windows.Forms.ListBox();
			this.homeLinkLabel = new System.Windows.Forms.LinkLabel();
			this.label5 = new System.Windows.Forms.Label();
			this.licenseLinkLabel = new System.Windows.Forms.LinkLabel();
			this.infoLinkLabel = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(331, 469);
			this.okButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(87, 30);
			this.okButton.TabIndex = 0;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(14, 204);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Messy Lab";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(14, 221);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "Version 1.01";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(14, 238);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(154, 17);
			this.label3.TabIndex = 3;
			this.label3.Text = "© 2010 Miloš Anđelković";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(14, 255);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(117, 17);
			this.label4.TabIndex = 4;
			this.label4.Text = "All rights reserved.";
			// 
			// listBox
			// 
			this.listBox.FormattingEnabled = true;
			this.listBox.ItemHeight = 17;
			this.listBox.Location = new System.Drawing.Point(240, 204);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(178, 242);
			this.listBox.TabIndex = 4;
			// 
			// homeLinkLabel
			// 
			this.homeLinkLabel.AutoSize = true;
			this.homeLinkLabel.BackColor = System.Drawing.Color.Transparent;
			this.homeLinkLabel.Location = new System.Drawing.Point(14, 272);
			this.homeLinkLabel.Name = "homeLinkLabel";
			this.homeLinkLabel.Size = new System.Drawing.Size(120, 17);
			this.homeLinkLabel.TabIndex = 1;
			this.homeLinkLabel.TabStop = true;
			this.homeLinkLabel.Text = "www.messylab.com";
			this.homeLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.homeLinkLabel_LinkClicked);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(14, 368);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(213, 65);
			this.label5.TabIndex = 7;
			this.label5.Text = "This software is distributed under the\r\nterms of the Apache License Version 2.0.\r" +
				"\n\r\nA copy of the license is available at the\r\nfollowing address:";
			// 
			// licenseLinkLabel
			// 
			this.licenseLinkLabel.AutoSize = true;
			this.licenseLinkLabel.BackColor = System.Drawing.Color.Transparent;
			this.licenseLinkLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.licenseLinkLabel.Location = new System.Drawing.Point(14, 433);
			this.licenseLinkLabel.Name = "licenseLinkLabel";
			this.licenseLinkLabel.Size = new System.Drawing.Size(203, 13);
			this.licenseLinkLabel.TabIndex = 3;
			this.licenseLinkLabel.TabStop = true;
			this.licenseLinkLabel.Text = "www.apache.org/licenses/LICENSE-2.0";
			this.licenseLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.licenseLinkLabel_LinkClicked);
			// 
			// infoLinkLabel
			// 
			this.infoLinkLabel.AutoSize = true;
			this.infoLinkLabel.BackColor = System.Drawing.Color.Transparent;
			this.infoLinkLabel.Location = new System.Drawing.Point(14, 319);
			this.infoLinkLabel.Name = "infoLinkLabel";
			this.infoLinkLabel.Size = new System.Drawing.Size(138, 17);
			this.infoLinkLabel.TabIndex = 2;
			this.infoLinkLabel.TabStop = true;
			this.infoLinkLabel.Text = "Additional Information";
			this.infoLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.infoLinkLabel_LinkClicked);
			// 
			// AboutForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.CancelButton = this.okButton;
			this.ClientSize = new System.Drawing.Size(474, 512);
			this.Controls.Add(this.infoLinkLabel);
			this.Controls.Add(this.licenseLinkLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.homeLinkLabel);
			this.Controls.Add(this.listBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.okButton);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About Messy Lab";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox listBox;
		private System.Windows.Forms.LinkLabel homeLinkLabel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.LinkLabel licenseLinkLabel;
		private System.Windows.Forms.LinkLabel infoLinkLabel;
	}
}