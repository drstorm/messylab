namespace MessyLab
{
	partial class NewMemoryBreakpointForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewMemoryBreakpointForm));
			this.label3 = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.readCheckBox = new System.Windows.Forms.CheckBox();
			this.writeCheckBox = new System.Windows.Forms.CheckBox();
			this.executeCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.addressNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.countNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.addressNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Location = new System.Drawing.Point(12, 175);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(260, 2);
			this.label3.TabIndex = 11;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(197, 188);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(116, 188);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 5;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// readCheckBox
			// 
			this.readCheckBox.AutoSize = true;
			this.readCheckBox.Checked = true;
			this.readCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.readCheckBox.Location = new System.Drawing.Point(81, 92);
			this.readCheckBox.Name = "readCheckBox";
			this.readCheckBox.Size = new System.Drawing.Size(52, 17);
			this.readCheckBox.TabIndex = 2;
			this.readCheckBox.Text = "Read";
			this.readCheckBox.UseVisualStyleBackColor = true;
			this.readCheckBox.CheckedChanged += new System.EventHandler(this.readCheckBox_CheckedChanged);
			// 
			// writeCheckBox
			// 
			this.writeCheckBox.AutoSize = true;
			this.writeCheckBox.Checked = true;
			this.writeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.writeCheckBox.Location = new System.Drawing.Point(81, 115);
			this.writeCheckBox.Name = "writeCheckBox";
			this.writeCheckBox.Size = new System.Drawing.Size(54, 17);
			this.writeCheckBox.TabIndex = 3;
			this.writeCheckBox.Text = "Write";
			this.writeCheckBox.UseVisualStyleBackColor = true;
			this.writeCheckBox.CheckedChanged += new System.EventHandler(this.readCheckBox_CheckedChanged);
			// 
			// executeCheckBox
			// 
			this.executeCheckBox.AutoSize = true;
			this.executeCheckBox.Location = new System.Drawing.Point(81, 138);
			this.executeCheckBox.Name = "executeCheckBox";
			this.executeCheckBox.Size = new System.Drawing.Size(65, 17);
			this.executeCheckBox.TabIndex = 4;
			this.executeCheckBox.Text = "Execute";
			this.executeCheckBox.UseVisualStyleBackColor = true;
			this.executeCheckBox.CheckedChanged += new System.EventHandler(this.readCheckBox_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 13);
			this.label1.TabIndex = 15;
			this.label1.Text = "Address:";
			// 
			// addressNumericUpDown
			// 
			this.addressNumericUpDown.Location = new System.Drawing.Point(81, 13);
			this.addressNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
			this.addressNumericUpDown.Minimum = new decimal(new int[] {
            0,
            -2147483648,
            0,
            -2147483648});
			this.addressNumericUpDown.Name = "addressNumericUpDown";
			this.addressNumericUpDown.Size = new System.Drawing.Size(191, 22);
			this.addressNumericUpDown.TabIndex = 0;
			// 
			// countNumericUpDown
			// 
			this.countNumericUpDown.Location = new System.Drawing.Point(81, 41);
			this.countNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.countNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.countNumericUpDown.Name = "countNumericUpDown";
			this.countNumericUpDown.Size = new System.Drawing.Size(191, 22);
			this.countNumericUpDown.TabIndex = 1;
			this.countNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 18;
			this.label2.Text = "Count:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 71);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 13);
			this.label4.TabIndex = 20;
			this.label4.Text = "Trigger on:";
			// 
			// NewMemoryBreakpointForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(284, 223);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.countNumericUpDown);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.addressNumericUpDown);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.executeCheckBox);
			this.Controls.Add(this.writeCheckBox);
			this.Controls.Add(this.readCheckBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewMemoryBreakpointForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Memory Breakpoint";
			((System.ComponentModel.ISupportInitialize)(this.addressNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.CheckBox readCheckBox;
		private System.Windows.Forms.CheckBox writeCheckBox;
		private System.Windows.Forms.CheckBox executeCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown addressNumericUpDown;
		private System.Windows.Forms.NumericUpDown countNumericUpDown;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;

	}
}