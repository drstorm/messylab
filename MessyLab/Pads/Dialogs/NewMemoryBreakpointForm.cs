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
	/// New Memory Breakpoint dialog.
	/// </summary>
	public partial class NewMemoryBreakpointForm : Form, INewBreakpointDialog
	{
		public NewMemoryBreakpointForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Shows the dialog.
		/// </summary>
		/// <param name="breakpoint">The new breakpoint definition as specified by the user.</param>
		/// <param name="controller">The parent breakpoint controller.</param>
		/// <returns>A value indicating success.</returns>
		public bool ShowNewBreakpointDialog(out BreakpointController.BreakpointDefinition breakpoint, BreakpointController controller)
		{
			if (ShowDialog() == DialogResult.OK)
			{
				var bp = new BreakpointController.MemoryBreakpointDefinition(controller)
				{
					Address = (long)addressNumericUpDown.Value,
					Count = (int)countNumericUpDown.Value,
					OnRead = readCheckBox.Checked,
					OnWrite = writeCheckBox.Checked,
					OnExecute = executeCheckBox.Checked
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

		private void readCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			bool enable = false;
			if (readCheckBox.Checked) enable = true;
			if (writeCheckBox.Checked) enable = true;
			if (executeCheckBox.Checked) enable = true;
			okButton.Enabled = enable;
		}
	}
}
