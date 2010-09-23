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
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MessyLab.Platforms.Pico
{
	public partial class ConsolePad : MessyLab.Pad, MessyLab.PicoComputer.IIODevice
	{
		public ConsolePad(Project project)
			: base(project)
		{
			InitializeComponent();
			CreateMenuItem("&Console", MessyLab.Properties.Resources.Console);
			CreateToolbarItem("Console", MessyLab.Properties.Resources.Console);
			ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;

			Done = new AutoResetEvent(false);
		}
		public ConsolePad() : this(null) { }

		private AutoResetEvent Done { get; set; }
		private volatile string ReadValue;

		private void Input(string query)
		{
			ShowOnMainForm();
			queryLabel.Text = query;
			inTextBox.Text = string.Empty;
			inputPanel.Visible = true;
			ScrollDown();
			inTextBox.Focus();
		}

		private void ScrollDown()
		{
			outTextBox.Focus();
			outTextBox.SelectionStart = int.MaxValue;
			outTextBox.SelectionLength = 0;
			outTextBox.ScrollToCaret();
		}

		private void enterButton_Click(object sender, EventArgs e)
		{
			ReadValue = inTextBox.Text;
			inputPanel.Visible = false;
			var sb = new StringBuilder();
			foreach (string l in outTextBox.Lines)
			{
				if (string.IsNullOrEmpty(l)) continue;
				sb.AppendLine(l);
			}
			sb.Append(queryLabel.Text);
			sb.AppendLine(ReadValue);

			outTextBox.Lines = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

			ScrollDown();
			Done.Set();
		}

		public void Clear()
		{
			outTextBox.Clear();
		}

		#region IIODevice Members

		private bool _abort;
		public void Read(ushort address, int count, MessyLab.PicoComputer.Data data)
		{
			_abort = false;
			// We use main form for invoking because the handle for the pad might not be created yet.
			var mainForm = Project.Platform.Gui.MainForm;
			for (int i = 0; i < count; i++)
			{
				if (mainForm.InvokeRequired)
				{
					mainForm.Invoke(new Action<string>(Input), string.Format("An integer value for location #{0}: ", address));
					lock (this)
					{
						if (_abort) break;
						Done.Reset();
					}
					Done.WaitOne();
					data[address++] = (ushort)short.Parse(ReadValue);
				}
				if (_abort) break;
			}
		}

		public void Write(ushort address, int count, MessyLab.PicoComputer.Data data)
		{
			// We use main form for invoking because the handle for the pad might not be created yet.
			var mainForm = Project.Platform.Gui.MainForm;
			if (mainForm.InvokeRequired)
			{
				mainForm.Invoke(new Action<ushort, int, MessyLab.PicoComputer.Data>(Write), address, count, data);
				return;
			}
			ShowOnMainForm();
			var sb = new StringBuilder();
			foreach (string l in outTextBox.Lines)
			{
				if (string.IsNullOrEmpty(l)) continue;
				sb.AppendLine(l);
			}
			for (int i = 0; i < count; i++)
			{
				short value = (short)data[address];
				sb.AppendLine(string.Format("The contents of memory location #{0} = {1}", address, value));
				address++;
			}
			outTextBox.Lines = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

			ScrollDown();

			Application.DoEvents();
		}

		#endregion

		public void Abort()
		{
			lock (this)
			{
				if (enterButton.Visible)
				{
					_abort = true;
					enterButton_Click(null, null);
				}
			}
		}
	}
}
