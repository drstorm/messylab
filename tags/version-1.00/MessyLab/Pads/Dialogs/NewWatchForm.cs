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
	/// New Watch dialog.
	/// </summary>
	public partial class NewWatchForm : Form
	{
		public NewWatchForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Sets a list of watch types.
		/// </summary>
		/// <param name="types">The list of type names to set.</param>
		public void SetTypes(IEnumerable<string> types)
		{
			typeComboBox.Items.Clear();
			if (types == null) return;

			foreach (var s in types)
			{
				typeComboBox.Items.Add(s);
			}
			if (typeComboBox.Items.Count > 0)
			{
				typeComboBox.SelectedIndex = 0;
			}
		}

		/// <summary>
		/// Selected watch name.
		/// </summary>
		public string WatchName { get; set; }

		/// <summary>
		/// Selected watch type.
		/// </summary>
		public string WatchType { get; set; }

		private void nameTextBox_TextChanged(object sender, EventArgs e)
		{
			okButton.Enabled = !string.IsNullOrEmpty(nameTextBox.Text);
			WatchName = nameTextBox.Text;
		}

		private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			WatchType = typeComboBox.Text;
		}
	}
}
