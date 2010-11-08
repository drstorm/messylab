namespace MessyLab.Platforms.Pico
{
	partial class PicoPropertiesForm
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
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.hexCheckBox = new System.Windows.Forms.CheckBox();
			this.txtCheckBox = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.autoAssembleCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(252, 164);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(171, 164);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Location = new System.Drawing.Point(12, 153);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(315, 2);
			this.label3.TabIndex = 10;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Build";
			// 
			// hexCheckBox
			// 
			this.hexCheckBox.AutoSize = true;
			this.hexCheckBox.Location = new System.Drawing.Point(29, 37);
			this.hexCheckBox.Name = "hexCheckBox";
			this.hexCheckBox.Size = new System.Drawing.Size(205, 17);
			this.hexCheckBox.TabIndex = 0;
			this.hexCheckBox.Text = "Generate pCAS compatible HEX file";
			this.hexCheckBox.UseVisualStyleBackColor = true;
			// 
			// txtCheckBox
			// 
			this.txtCheckBox.AutoSize = true;
			this.txtCheckBox.Location = new System.Drawing.Point(29, 60);
			this.txtCheckBox.Name = "txtCheckBox";
			this.txtCheckBox.Size = new System.Drawing.Size(271, 17);
			this.txtCheckBox.TabIndex = 1;
			this.txtCheckBox.Text = "Generate textual representation of the program";
			this.txtCheckBox.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 91);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(27, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "Edit";
			// 
			// autoAssembleCheckBox
			// 
			this.autoAssembleCheckBox.AutoSize = true;
			this.autoAssembleCheckBox.Location = new System.Drawing.Point(29, 119);
			this.autoAssembleCheckBox.Name = "autoAssembleCheckBox";
			this.autoAssembleCheckBox.Size = new System.Drawing.Size(157, 17);
			this.autoAssembleCheckBox.TabIndex = 2;
			this.autoAssembleCheckBox.Text = "Auto-assemble by default";
			this.autoAssembleCheckBox.UseVisualStyleBackColor = true;
			// 
			// PicoPropertiesForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(339, 199);
			this.Controls.Add(this.autoAssembleCheckBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtCheckBox);
			this.Controls.Add(this.hexCheckBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.label3);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PicoPropertiesForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Properties";
			this.Shown += new System.EventHandler(this.PicoPropertiesForm_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox hexCheckBox;
		private System.Windows.Forms.CheckBox txtCheckBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox autoAssembleCheckBox;
	}
}