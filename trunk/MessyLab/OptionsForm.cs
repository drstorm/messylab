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
using System.IO;
using MessyLab.Properties;

namespace MessyLab
{
	/// <summary>
	/// The Options Dialog.
	/// </summary>
	public partial class OptionsForm : Form
	{
		public OptionsForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Displays a folder selection dialog.
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
		/// Displays a font selection dialog.
		/// </summary>
		private void chooseButton_Click(object sender, EventArgs e)
		{
			using (var d = new FontDialog())
			{
				d.FontMustExist = true;
				d.Font = Settings.Default.EditorFont;
				if (d.ShowDialog() == DialogResult.OK)
				{
					Settings.Default.EditorFont = d.Font;
					fontTextBox.Text = string.Format("{0}, {1}pt", d.Font.Name, d.Font.SizeInPoints);
				}
			}
		}

		private void OptionsForm_Shown(object sender, EventArgs e)
		{
			LoadSettings();
		}

		/// <summary>
		/// Saves the settings and closes the dialog.
		/// </summary>
		private void okButton_Click(object sender, EventArgs e)
		{
			SaveSettings();
			DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// Loads the settings to the form.
		/// </summary>
		private void LoadSettings()
		{
			locationTextBox.Text = Settings.Default.ProjectRoot;
			var font = Settings.Default.EditorFont;
			fontTextBox.Text = string.Format("{0}, {1}pt", font.Name, font.SizeInPoints);
		}

		/// <summary>
		/// Saves the settings from the form.
		/// </summary>
		private void SaveSettings()
		{
			if (!Directory.Exists(locationTextBox.Text))
			{
				MessageBox.Show("Specified project location does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				locationTextBox.SelectAll();
				locationTextBox.Focus();
				return;
			}
			Settings.Default.ProjectRoot = locationTextBox.Text;
			Settings.Default.Save();
		}

	}
}
