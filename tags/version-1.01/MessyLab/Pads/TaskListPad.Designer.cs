namespace MessyLab
{
	partial class TaskListPad
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskListPad));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.MouseEnter += new System.EventHandler(this.TaskListPad_Enter);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "Task.png");
			// 
			// TaskListPad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(875, 197);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TaskListPad";
			this.TabText = "Task List";
			this.Text = "Task List";
			this.Shown += new System.EventHandler(this.TaskListPad_Shown);
			this.Enter += new System.EventHandler(this.TaskListPad_Enter);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ImageList imageList;
	}
}
