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
	/// New I/O Breakpoint dialog.
	/// </summary>
	public partial class NewIOBreakpointForm : Form, INewBreakpointDialog
	{
		public NewIOBreakpointForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Sets the Address behavior.
		/// </summary>
		/// <param name="address">The default address.</param>
		/// <param name="addressEnabled">A value indicating whether the editbox is enabled.</param>
		public void SetAddress(long address, bool addressEnabled)
		{
			addressNumericUpDown.Value = address;
			addressNumericUpDown.Enabled = addressEnabled;
		}

		/// <summary>
		/// Sets the Input behavior.
		/// </summary>
		/// <param name="input">A value indicating whether to break on input by default.</param>
		/// <param name="inputEnabled">A value indicating whether the checkbox is enabled.</param>
		public void SetInput(bool input, bool inputEnabled)
		{
			inputCheckBox.Checked = input;
			inputCheckBox.Enabled = inputEnabled;
		}

		/// <summary>
		/// Sets the Output behavior.
		/// </summary>
		/// <param name="output">A value indicating whether to break on output by default.</param>
		/// <param name="outputEnabled">A value indicating whether the checkbox is enabled.</param>
		public void SetOutput(bool output, bool outputEnabled)
		{
			outputCheckBox.Checked = output;
			outputCheckBox.Enabled = outputEnabled;
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
				var bp = new BreakpointController.IOBreakpointDefinition(controller)
				{
					Address = (long)addressNumericUpDown.Value,
					Input = inputCheckBox.Checked,
					Output = outputCheckBox.Checked
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

		private void inputCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			okButton.Enabled = inputCheckBox.Checked || outputCheckBox.Checked;
		}
	}
}
