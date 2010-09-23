namespace MessyLab
{
	partial class NewRegisterBreakpointForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRegisterBreakpointForm));
			this.label3 = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.nameComboBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.writeCheckBox = new System.Windows.Forms.CheckBox();
			this.readCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Location = new System.Drawing.Point(12, 127);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(260, 2);
			this.label3.TabIndex = 14;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(197, 140);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Enabled = false;
			this.okButton.Location = new System.Drawing.Point(116, 140);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// nameComboBox
			// 
			this.nameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.nameComboBox.FormattingEnabled = true;
			this.nameComboBox.Location = new System.Drawing.Point(81, 12);
			this.nameComboBox.Name = "nameComboBox";
			this.nameComboBox.Size = new System.Drawing.Size(191, 21);
			this.nameComboBox.TabIndex = 0;
			this.nameComboBox.SelectedIndexChanged += new System.EventHandler(this.nameComboBox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "Register:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 13);
			this.label4.TabIndex = 23;
			this.label4.Text = "Trigger on:";
			// 
			// writeCheckBox
			// 
			this.writeCheckBox.AutoSize = true;
			this.writeCheckBox.Location = new System.Drawing.Point(81, 86);
			this.writeCheckBox.Name = "writeCheckBox";
			this.writeCheckBox.Size = new System.Drawing.Size(54, 17);
			this.writeCheckBox.TabIndex = 2;
			this.writeCheckBox.Text = "Write";
			this.writeCheckBox.UseVisualStyleBackColor = true;
			this.writeCheckBox.CheckedChanged += new System.EventHandler(this.readCheckBox_CheckedChanged);
			// 
			// readCheckBox
			// 
			this.readCheckBox.AutoSize = true;
			this.readCheckBox.Location = new System.Drawing.Point(81, 63);
			this.readCheckBox.Name = "readCheckBox";
			this.readCheckBox.Size = new System.Drawing.Size(52, 17);
			this.readCheckBox.TabIndex = 1;
			this.readCheckBox.Text = "Read";
			this.readCheckBox.UseVisualStyleBackColor = true;
			this.readCheckBox.CheckedChanged += new System.EventHandler(this.readCheckBox_CheckedChanged);
			// 
			// NewRegisterBreakpointForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(284, 175);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.writeCheckBox);
			this.Controls.Add(this.readCheckBox);
			this.Controls.Add(this.nameComboBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewRegisterBreakpointForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Register Breakpoint";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.ComboBox nameComboBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox writeCheckBox;
		private System.Windows.Forms.CheckBox readCheckBox;
	}
}