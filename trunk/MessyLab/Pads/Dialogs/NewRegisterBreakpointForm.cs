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
using MessyLab.Platforms;

namespace MessyLab
{
	/// <summary>
	/// New Register Breakpoint dialog.
	/// </summary>
	public partial class NewRegisterBreakpointForm : Form, INewBreakpointDialog
	{
		public NewRegisterBreakpointForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Sets the list of available registers.
		/// </summary>
		/// <param name="registerNames">The list of register names.</param>
		public void SetRegisterNames(IEnumerable<string> registerNames)
		{
			nameComboBox.Items.Clear();
			if (registerNames == null) return;

			foreach (var s in registerNames)
			{
				nameComboBox.Items.Add(s);
			}
			if (nameComboBox.Items.Count > 0)
			{
				nameComboBox.SelectedIndex = 0;
			}
		}

		/// <summary>
		/// Sets On Read behavior.
		/// </summary>
		/// <param name="onRead">A value indicating whether to break on read by default.</param>
		/// <param name="onReadEnabled">A value indicating whether the checkbox is enabled.</param>
		public void SetOnRead(bool onRead, bool onReadEnabled)
		{
			readCheckBox.Checked = onRead;
			readCheckBox.Enabled = onReadEnabled;
		}

		/// <summary>
		/// Sets On Read behavior.
		/// </summary>
		/// <param name="onWrite">A value indicating whether to break on write by default.</param>
		/// <param name="onWriteEnabled">A value indicating whether the checkbox is enabled.</param>
		public void SetOnWrite(bool onWrite, bool onWriteEnabled)
		{
			writeCheckBox.Checked = onWrite;
			writeCheckBox.Enabled = onWriteEnabled;
		}

		/// <summary>
		/// Shows the dialog.
		/// </summary>
		/// <param name="breakpoint">The new breakpoint definition as specified by the user.</param>
		/// <param name="controller">The parent breakpoint controller.</param>
		/// <returns>A value indicating success.</returns>
		public bool ShowNewBreakpointDialog(out BreakpointController.BreakpointDefinition breakpoint, BreakpointController controller)
		{
			if (nameComboBox.Items.Count > 0) nameComboBox.SelectedIndex = 0;
			if (ShowDialog() == DialogResult.OK)
			{
				var bp = new BreakpointController.RegisterBreakpointDefinition(controller)
				{
					Name = nameComboBox.Text,
					OnRead = readCheckBox.Checked,
					OnWrite = writeCheckBox.Checked,
				};
				breakpoint = bp;
				return true;
			}
			else
			{
				breakpoint = null;
				return false;
			}
		}

		/// <summary>
		/// Enables or disables the OK button.
		/// </summary>
		private void SetOKButtonEnabled()
		{
			bool enable = !string.IsNullOrEmpty(nameComboBox.Text);
			enable = enable && (readCheckBox.Checked || writeCheckBox.Checked);
			okButton.Enabled = enable;
		}

		private void nameComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetOKButtonEnabled();
		}

		private void readCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			SetOKButtonEnabled();
		}
	}
}
