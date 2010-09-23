namespace MessyLab.Platforms.Pico
{
	partial class PicoTextEditorForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PicoTextEditorForm));
			this.infoTimer = new System.Windows.Forms.Timer(this.components);
			this.errorPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// infoTextEditorControl
			// 
			this.infoTextEditorControl.IsReadOnly = true;
			this.infoTextEditorControl.Size = new System.Drawing.Size(175, 603);
			// 
			// textEditorControl
			// 
			this.textEditorControl.Location = new System.Drawing.Point(175, 0);
			this.textEditorControl.Size = new System.Drawing.Size(670, 603);
			// 
			// infoTimer
			// 
			this.infoTimer.Interval = 500;
			this.infoTimer.Tick += new System.EventHandler(this.infoTimer_Tick);
			// 
			// errorPanel
			// 
			this.errorPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("errorPanel.BackgroundImage")));
			this.errorPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.errorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.errorPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.errorPanel.Location = new System.Drawing.Point(12, 12);
			this.errorPanel.Name = "errorPanel";
			this.errorPanel.Size = new System.Drawing.Size(24, 24);
			this.errorPanel.TabIndex = 7;
			this.errorPanel.Visible = false;
			this.errorPanel.Click += new System.EventHandler(this.errorPanel_Click);
			// 
			// PicoTextEditorForm
			// 
			this.Breakpoints = ((System.Collections.Generic.IEnumerable<int>)(resources.GetObject("$this.Breakpoints")));
			this.ClientSize = new System.Drawing.Size(845, 625);
			this.Controls.Add(this.errorPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PicoTextEditorForm";
			this.Controls.SetChildIndex(this.infoTextEditorControl, 0);
			this.Controls.SetChildIndex(this.textEditorControl, 0);
			this.Controls.SetChildIndex(this.errorPanel, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer infoTimer;
		protected System.Windows.Forms.Panel errorPanel;
	}
}
