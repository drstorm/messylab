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
using MessyLab.PicoComputer;
using MessyLab.Properties;

namespace MessyLab.Platforms.Pico
{
	/// <summary>
	/// Text editor for picoComputer.
	/// </summary>
	public partial class PicoTextEditorForm : MessyLab.TextEditorForm
	{
		public PicoTextEditorForm()
		{
			InitializeComponent();

			AllowBreakpoints = true;

			// Initialize info editor.
			InfoEditor.Visible = Settings.Default.Pico_AutoAssemble;
			InfoEditor.Document.HighlightingStrategy = ICSharpCode.TextEditor.Document.HighlightingStrategyFactory.CreateHighlightingStrategy("Info");

			// Create and initialize Auto-assemble button.
			var b = new ToolStripButton("Auto-assemble");
			b.Checked = Settings.Default.Pico_AutoAssemble;
			b.Click +=
				(sender, e) =>
				{
					InfoEditor.Visible = !InfoEditor.Visible;

					b.Checked = InfoEditor.Visible;
					infoTimer.Enabled = InfoEditor.Visible;
					if (!InfoEditor.Visible) errorPanel.Visible = false;
				};
			statusStrip.Items.Insert(0, b);

			Editor.Document.DocumentChanged += new ICSharpCode.TextEditor.Document.DocumentEventHandler(Document_DocumentChanged);
		}

		/// <summary>
		/// Restarts the infoTimer on document change.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Document_DocumentChanged(object sender, ICSharpCode.TextEditor.Document.DocumentEventArgs e)
		{
			if (InfoEditor.Visible)
			{
				// Restart the timer.
				infoTimer.Enabled = false;
				infoTimer.Enabled = true;
			}
		}

		/// <summary>
		/// Assembles the current document and updates info editor
		/// or files an error.
		/// </summary>
		private void infoTimer_Tick(object sender, EventArgs e)
		{
			infoTimer.Enabled = false;

			if (!InfoEditor.Visible) return;

			var gui = Item.Project.Platform.Gui as DebuggablePlatform.DebuggableGuiProvider;
			var errPad = gui.ErrorsPad;
			errPad.ClearItems();

			var doc = Editor.Document;
			Assembler a = new Assembler(doc.TextContent);
			if (a.Process())
			{
				errorPanel.Visible = false;

				string[] info = new string[doc.TotalNumberOfLines];
				foreach (var ins in a.Instructions)
				{
					info[ins.Line - 1] = string.Format("{0,5}: {1}", ins.Address, ins.CodeToString());
				}
				var sb = new StringBuilder();
				foreach (var s in info)
				{
					sb.AppendLine(s);
				}
				InfoEditor.Document.TextContent = sb.ToString();
				InfoEditor.Document.RequestUpdate(new ICSharpCode.TextEditor.TextAreaUpdate(ICSharpCode.TextEditor.TextAreaUpdateType.WholeTextArea));
				AdjustInfoEditorScrollbar();
			}
			else
			{
				errorPanel.Visible = true;
				foreach (var error in a.Errors)
				{
					string desc = string.Format("E{0:0000}: {1}.", error.ID, error.Description);
					errPad.AddItem(new ListItem(desc, Item, error.Line, error.Column));
				}
			}
		}

		public override void Open(string path)
		{
			base.Open(path);
			infoTimer.Enabled = true;
		}

		/// <summary>
		/// Occurs when the user clicks on the Errors icon.
		/// </summary>
		public event EventHandler ErrorsClick;
		private void OnErrorsClick(object sender, EventArgs e)
		{ if (ErrorsClick != null) ErrorsClick(sender, e); }

		private void errorPanel_Click(object sender, EventArgs e)
		{
			OnErrorsClick(sender, e);
		}

	}
}
