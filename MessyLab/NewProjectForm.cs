#region License
/*
 * Copyright 2010 Miloš Anđelković
 *    
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MessyLab.Properties;
using System.IO;

namespace MessyLab
{
	/// <summary>
	/// New Project Dialog.
	/// </summary>
	public partial class NewProjectForm : Form
	{
		public NewProjectForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Selected new project Path.
		/// </summary>
		public string Path { get; protected set; }

		/// <summary>
		/// Selected platform.
		/// </summary>
		public string Platform { get; protected set; }

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			okButton.Enabled = listView.SelectedItems.Count == 1;
		}

		private void NewProjectForm_Shown(object sender, EventArgs e)
		{
			locationTextBox.Text = Settings.Default.ProjectRoot;
			listView.Items[0].Selected = true;
		}

		/// <summary>
		/// Displays the folder selection dialog.
		/// </summary>
		private void browseButton_Click(object sender, EventArgs e)
		{
			using (var d = new FolderBrowserDialog())
			{
				if (Directory.Exists(locationTextBox.Text))
				{ d.SelectedPath = locationTextBox.Text; }
				if (d.ShowDialog() == DialogResult.OK)
				{
					locationTextBox.Text = d.SelectedPath;
				}
			}
		}

		/// <summary>
		/// Checks the validity of the input and sets Path and Platform
		/// and closes the dialog if everything is OK.
		/// </summary>
		private void okButton_Click(object sender, EventArgs e)
		{
			Platform = listView.SelectedItems[0].Tag.ToString();
			string name = nameTextBox.Text;
			if (string.IsNullOrEmpty(name))
			{
				MessageBox.Show("No project name specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				nameTextBox.Focus();
				return;
			}
			try
			{
				if (System.IO.Path.GetExtension(name).ToLower() == ".mlp")
				{
					name = System.IO.Path.GetFileNameWithoutExtension(name);
				}
			}
			catch (ArgumentException)
			{
				MessageBox.Show("Invalid project name specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				nameTextBox.SelectAll();
				nameTextBox.Focus();
				return;
			}


			string dir = locationTextBox.Text;
			if (!Directory.Exists(dir))
			{
				MessageBox.Show("Specified project location does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				locationTextBox.SelectAll();
				locationTextBox.Focus();
				return;
			}
			if (createDirCheckBox.Checked)
			{
				dir = System.IO.Path.Combine(dir, name);
			}
			
			if (Directory.Exists(dir) && Directory.GetFiles(dir).Length > 0)
			{
				MessageBox.Show("Specified project location is not empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				nameTextBox.SelectAll();
				nameTextBox.Focus();
				return;
			}

			Path = System.IO.Path.Combine(dir, name + ".mlp");

			DialogResult = DialogResult.OK;
		}
	}
}
