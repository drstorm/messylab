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

namespace MessyLab
{
	/// <summary>
	/// New Project Item Dialog.
	/// </summary>
	public partial class NewProjectItemForm : Form
	{
		public NewProjectItemForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Chosen Item Name.
		/// </summary>
		public string ItemName { get; set; }
		/// <summary>
		/// Chosen Item Type.
		/// </summary>
		public string ItemType { get; set; }

		/// <summary>
		/// Sets a list of valid item types.
		/// </summary>
		/// <param name="types"></param>
		public void SetTypes(IEnumerable<string> types)
		{
			listView.Clear();
			foreach (var s in types)
			{
				var item = new ListViewItem(s);
				listView.Items.Add(item);
			}
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			okButton.Enabled = listView.SelectedItems.Count == 1;
		}

		private void NewProjectItemForm_Shown(object sender, EventArgs e)
		{
			if (listView.Items.Count > 0)
			{
				listView.Items[0].Selected = true;
			}
		}

		/// <summary>
		/// Sets ItemName and ItemType and closes the form.
		/// </summary>
		private void okButton_Click(object sender, EventArgs e)
		{
			ItemType = listView.SelectedItems[0].Text;
			string name = nameTextBox.Text;
			if (string.IsNullOrEmpty(name))
			{
				MessageBox.Show("No item name specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				nameTextBox.Focus();
				return;
			}
			try
			{
				name = System.IO.Path.GetFileNameWithoutExtension(name);
			}
			catch (ArgumentException)
			{
				MessageBox.Show("Invalid item name specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				nameTextBox.SelectAll();
				nameTextBox.Focus();
				return;
			}

			ItemName = name;

			DialogResult = DialogResult.OK;
		}
	}
}
