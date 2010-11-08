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
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using System.IO;

namespace MessyLab
{
	public partial class TextEditorForm : AbstractEditorForm
	{
		public TextEditorForm()
		{
			InitializeComponent();
		}

		#region Control Initialization
		/// <summary>
		/// Indicates whether the control has been loaded, i.e. initialized.
		/// </summary>
		private bool _loaded = false;

		/// <summary>
		/// Handles the form load event by initializing the editor.
		/// </summary>
		private void TextEditorForm_Load(object sender, EventArgs e)
		{
			_loaded = true;
			LoadSettings();
			InitializeEditor();
			InitializeInfoEditor();
		}

		/// <summary>
		/// Initializes the editor control.
		/// </summary>
		private void InitializeEditor()
		{
			textEditorControl.Document.DocumentChanged +=
				(sender, e) =>
				{ IsModified = true; };

			textEditorControl.Document.BookmarkManager.Factory = new BreakpointBookmarkFactory(this);
			textEditorControl.Document.BookmarkManager.Removed +=
				(sender, e) =>
				{
					var mb = e.Bookmark as MarkerBookmark;
					if (mb != null) mb.RemoveMarker();
				};

			textEditorControl.ActiveTextAreaControl.TextArea.IconBarMargin.MouseDown +=
				new MarginMouseEventHandler(IconBarMargin_MouseDown);

			var caret = textEditorControl.ActiveTextAreaControl.Caret;
			caret.PositionChanged +=
				(sender, e) =>
				{
					lineToolStripStatusLabel.Text = "Ln " + (caret.Line + 1);
					colToolStripStatusLabel.Text = "Col " + (caret.Column + 1);
				};
			caret.CaretModeChanged +=
				(sender, e) =>
				{ insToolStripStatusLabel.Text = (caret.CaretMode == CaretMode.InsertMode) ? "INS" : "OVR"; };

			if (_bpsToAdd != null)
			{
				Breakpoints = _bpsToAdd;
				_bpsToAdd = null;
			}
		}

		/// <summary>
		/// Toggles breakpoints when clicked on the icon margin of the editor.
		/// </summary>
		private void IconBarMargin_MouseDown(AbstractMargin sender, Point mousepos, MouseButtons mouseButtons)
		{
			var textArea = textEditorControl.ActiveTextAreaControl.TextArea;
			int clickedVisibleLine = (mousepos.Y + textArea.VirtualTop.Y) / textArea.TextView.FontHeight;
			int lineNumber = textArea.Document.GetFirstLogicalLine(clickedVisibleLine);

			if ((mouseButtons & MouseButtons.Left) == MouseButtons.Left)
			{
				ToggleBreakpointAt(lineNumber + 1);
			}
		}

		/// <summary>
		/// Initializes the info editor.
		/// </summary>
		private void InitializeInfoEditor()
		{
			infoTextEditorControl.ActiveTextAreaControl.VScrollBar.Visible = false;
			infoTextEditorControl.IsReadOnly = true;

			textEditorControl.ActiveTextAreaControl.VScrollBar.ValueChanged += (sender, e) => AdjustInfoEditorScrollbar();
		}

		/// <summary>
		/// Adjusts info editor's scrollbar according to the main editor's scrollbar.
		/// </summary>
		protected void AdjustInfoEditorScrollbar()
		{
			int v = textEditorControl.ActiveTextAreaControl.VScrollBar.Value;
			var scroll = infoTextEditorControl.ActiveTextAreaControl.VScrollBar;
			v = Math.Min(scroll.Maximum, Math.Max(scroll.Minimum, v));
			scroll.Value = v;
		}

		#endregion

		/// <summary>
		/// The main editor control.
		/// </summary>
		public TextEditorControl Editor { get { return textEditorControl; } }

		/// <summary>
		/// The info editor control.
		/// </summary>
		public TextEditorControl InfoEditor { get { return infoTextEditorControl; } }

		private volatile bool _modified;
		/// <summary>
		/// A value indicating whether the document is modified.
		/// </summary>
		public override bool IsModified
		{
			get { return _modified; }
			protected set
			{
				_modified = value;
				Action<TextEditorForm> updateText =
					f => f.TabText = f.Text = _title + (value ? "*" : string.Empty);
				if (InvokeRequired)
				{
					BeginInvoke(updateText, this);
				}
				else
				{
					updateText(this);
				}
			}
		}

		private volatile string _title = "Untitled";
		/// <summary>
		/// Document title.
		/// </summary>
		public override string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				Action<TextEditorForm> updateText =
					f => f.TabText = f.Text = value + (IsModified ? "*" : string.Empty);
				if (InvokeRequired)
				{
					BeginInvoke(updateText, this);
				}
				else
				{
					updateText(this);
				}
			}
		}

		/// <summary>
		/// A value indicating whether the document is read-only.
		/// </summary>
		public override bool IsReadOnly
		{
			get { return Editor.IsReadOnly; }
			set { Editor.IsReadOnly = value; }
		}

		#region Edit operations
		/// <summary>
		/// Performs an action encapsulated in IEditAction.
		/// </summary>
		private void DoEditAction(IEditAction action)
		{
			if (action != null)
			{
				var area = Editor.ActiveTextAreaControl.TextArea;
				Editor.BeginUpdate();
				try
				{
					lock (Editor.Document)
					{
						action.Execute(area);
						if (area.SelectionManager.HasSomethingSelected && area.AutoClearSelection)
						{
							if (area.Document.TextEditorProperties.DocumentSelectionMode == ICSharpCode.TextEditor.Document.DocumentSelectionMode.Normal)
							{
								area.SelectionManager.ClearSelection();
							}
						}
					}
				}
				finally
				{
					Editor.EndUpdate();
					area.Caret.UpdateCaretPosition();
				}
			}
		}

		/// <summary>
		/// Undos the last action.
		/// </summary>
		public override void Undo()
		{
			if (Editor.Document.UndoStack.CanUndo)
			{ Editor.Undo(); }
		}

		/// <summary>
		/// Redos the last undone action.
		/// </summary>
		public override void Redo()
		{
			if (Editor.Document.UndoStack.CanRedo)
			{ Editor.Redo(); }
		}

		/// <summary>
		/// Copies selection to the clipboard.
		/// </summary>
		public override void Copy()
		{
			DoEditAction(new Copy());
		}

		/// <summary>
		/// Cuts selection to the clipboard.
		/// </summary>
		public override void Cut()
		{
			DoEditAction(new Cut());
		}

		/// <summary>
		/// Pastes from the clipboard.
		/// </summary>
		public override void Paste()
		{
			DoEditAction(new Paste());
		}

		/// <summary>
		/// Selects all.
		/// </summary>
		public override void SelectAll()
		{
			DoEditAction(new SelectWholeDocument());
		}

		/// <summary>
		/// Goes to the specified line.
		/// </summary>
		/// <param name="line">The line number to go to.</param>
		public override void GoToLine(int line)
		{
			Editor.ActiveTextAreaControl.Caret.Position = new TextLocation(0, line);
		}

		/// <summary>
		/// Displays a dialog to enter a line to go to.
		/// </summary>
		public override void GoTo()
		{
			int lines = Editor.Document.TotalNumberOfLines;
			IntegerInputForm f = new IntegerInputForm("Go To Line", "Line number (1 - " + lines + "):", 1, lines);
			f.Value = Editor.ActiveTextAreaControl.Caret.Line + 1;
			if (f.ShowDialog() == DialogResult.OK)
			{
				GoToLine(f.Value - 1);
			}
		}

		/// <summary>
		/// Changes the case of the selection.
		/// </summary>
		/// <param name="toUpper">A value indicating whether to change to upper-case; otherwise lower-case.</param>
		public override void ChangeSelectionCase(bool toUpper)
		{
			var action = toUpper ? new ToUpperCase() as IEditAction : new ToLowerCase() as IEditAction;
			DoEditAction(action);
		}

		/// <summary>
		/// Gets the start and end lines of the current selection.
		/// </summary>
		/// <param name="startLine">The starting line of the selection.</param>
		/// <param name="endLine"></param>
		private void GetSelectedLines(out int startLine, out int endLine)
		{
			startLine = Editor.ActiveTextAreaControl.Caret.Line;
			endLine = startLine;

			foreach (var sel in Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection)
			{
				startLine = sel.StartPosition.Line;
				var seg = Editor.Document.GetLineSegment(startLine);
				if (seg.Length == sel.StartPosition.Column)
				{ startLine++; }
				endLine = sel.EndPosition.Line;
				if (sel.EndPosition.Column == 0)
				{ endLine--; }
			}
		}

		/// <summary>
		/// Inserts or removes the specified leading sequence on the selected lines.
		/// </summary>
		/// <remarks>
		/// This method is useful for implementing identation and line commenting.
		/// </remarks>
		/// <param name="insert">A value indicating whether to insert the sequence at the beginning; remove otherwise.</param>
		/// <param name="sequence">The sequence to insert or remove.</param>
		private void InsertOrRemoveLeadingSequence(bool insert, string sequence)
		{
			int startLine;
			int endLine;
			GetSelectedLines(out startLine, out endLine);

			Editor.BeginUpdate();
			var doc = Editor.Document;
			lock (doc)
			{
				doc.UndoStack.StartUndoGroup();
				for (int i = startLine; i <= endLine; i++)
				{
					int startOffset = doc.PositionToOffset(new TextLocation(0, i));
					if (insert)
					{
						doc.Insert(startOffset, sequence);
					}
					else
					{
						try
						{
							if (doc.GetText(startOffset, sequence.Length) == sequence)
							{ doc.Remove(startOffset, sequence.Length); }
						}
						catch (ArgumentOutOfRangeException) { }
					}
				}
				doc.UndoStack.EndUndoGroup();
			}
			Editor.ActiveTextAreaControl.Caret.ValidateCaretPos();
			Editor.EndUpdate();
			Editor.Refresh();
		}

		/// <summary>
		/// Comments or uncomments selection using the specified comment marker.
		/// </summary>
		/// <param name="comment">A value indicating whether to comment the selection; otherwise uncomment.</param>
		/// <param name="commentMarker">The comment marker.</param>
		public override void CommentSelection(bool comment, string commentMarker)
		{
			InsertOrRemoveLeadingSequence(comment, commentMarker);
		}

		/// <summary>
		/// Idents the selection.
		/// </summary>
		/// <param name="increase">A value indicating whether to increase ident; otherwise decrease.</param>
		public override void Ident(bool increase)
		{
			InsertOrRemoveLeadingSequence(increase, "\t");
		}

		#endregion

		#region File operations
		/// <summary>
		/// Opens the specified file.
		/// </summary>
		/// <param name="path">The path of the file to open.</param>
		public override void Open(string path)
		{
			Editor.LoadFile(path);
			IsModified = false;
			Title = Path.GetFileName(path);
		}

		/// <summary>
		/// Saves changes.
		/// </summary>
		/// <returns>A value indicating success.</returns>
		public override bool Save()
		{
			if (string.IsNullOrEmpty(Editor.FileName)) return false;
			Editor.SaveFile(Editor.FileName);
			IsModified = false;
			return true;
		}

		/// <summary>
		/// Reverts the document to the last saved state.
		/// </summary>
		public override void Revert()
		{
			if (!IsModified) return;
			if (Item != null)
			{
				Open(Item);
			}
			else
			{
				Editor.Document.TextContent = string.Empty;
				IsModified = false;
			}
		}

		/// <summary>
		/// Prints the document with a preview.
		/// </summary>
		public override void Print()
		{
			PrintPreviewDialog dialog = new PrintPreviewDialog();
			dialog.WindowState = FormWindowState.Maximized;
			dialog.Icon = Icon.Clone() as Icon;
			dialog.Document = Editor.PrintDocument;
			dialog.ShowDialog();
		}
		#endregion

		#region Find and Replace
		private FindAndReplaceForm _findForm = new FindAndReplaceForm();
		/// <summary>
		/// An instance of the find/replace form
		/// </summary>
		protected FindAndReplaceForm FindAndReplaceForm
		{
			get { return _findForm; }
			set { _findForm = value; }
		}

		/// <summary>
		/// Displays the find dialog.
		/// </summary>
		public override void Find()
		{
			_findForm.ShowFor(Editor, false);
		}

		/// <summary>
		/// Finds the next instance.
		/// </summary>
		public override void FindNext()
		{
			_findForm.FindNext(true, false,
				string.Format("Search text «{0}» not found.", _findForm.FindWhat));
		}

		/// <summary>
		/// Finds the previous instance.
		/// </summary>
		public override void FindPrevious()
		{
			_findForm.FindNext(true, true,
				string.Format("Search text «{0}» not found.", _findForm.FindWhat));
		}

		/// <summary>
		/// Replaces the current instance.
		/// </summary>
		public override void Replace()
		{
			_findForm.ShowFor(Editor, true);
		}
		#endregion

		#region Can do Stuff
		/// <summary>
		/// A value indicating whether an Undo is possible.
		/// </summary>
		public override bool CanUndo { get { return Editor.Document.UndoStack.CanUndo; } }
		/// <summary>
		/// A value indicating whether a Redo is possible.
		/// </summary>
		public override bool CanRedo { get { return Editor.Document.UndoStack.CanRedo; } }

		/// <summary>
		/// A value indicating whether Cut is possible.
		/// </summary>
		public override bool CanCut { get { return Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCut; } }
		/// <summary>
		/// A value indicating whether Copy is possible.
		/// </summary>
		public override bool CanCopy { get { return Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.EnableCopy; } }
		/// <summary>
		/// A value indicating whether Paste is possible.
		/// </summary>
		public override bool CanPaste { get { return Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.EnablePaste; } }

		/// <summary>
		/// A value indicating whether Select All is possible.
		/// </summary>
		public override bool CanSelectAll { get { return true; } }
		/// <summary>
		/// A value indicating whether Find or Replace are possible.
		/// </summary>
		public override bool CanFindReplace { get { return true; } }

		/// <summary>
		/// A value indicating whether Go To is possible.
		/// </summary>
		public override bool CanGoTo { get { return true; } }

		/// <summary>
		/// A value indicating whether advanced text editing is possible, e.g. commenting, changing ident, etc.
		/// </summary>
		public override bool CanDoAdvancedTextEditing { get { return !Editor.IsReadOnly; } }
		#endregion

		#region Event Handlers
		private void TextEditorForm_Activated(object sender, EventArgs e)
		{
			if (_findForm.IsShown)
			{
				_findForm.Show();
			}
		}

		private void TextEditorForm_Deactivate(object sender, EventArgs e)
		{
			_findForm.Hide();
		}

		#region Context Menu Events
		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			cutToolStripMenuItem.Enabled = CanCut;
			copyToolStripMenuItem.Enabled = CanCopy;
			pasteToolStripMenuItem.Enabled = CanPaste;
			toggleBreakpointToolStripMenuItem.Enabled = AllowBreakpoints;
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Paste();
		}

		private void toggleBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToggleBreakpoint();
		}
		#endregion

		#region Tab Menu Events
		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Save();
		}

		private void printToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Print();
		}
		#endregion

		#endregion

		#region Breakpoints
		/// <summary>
		/// A value indicating whether Breakpoints are allowed.
		/// </summary>
		public override bool AllowBreakpoints { get; set; }

		/// <summary>
		/// Toggles the breakpoint at ste current line.
		/// </summary>
		public override void ToggleBreakpoint()
		{
			if (!AllowBreakpoints) return;
			int line = Editor.ActiveTextAreaControl.Caret.Line + 1;
			ToggleBreakpointAt(line);
		}

		/// <summary>
		/// Removes the breakpoint at the specified line.
		/// </summary>
		/// <param name="line">The line to remove the breakpoint at.</param>
		/// <returns><c>false</c> if there is no breakpoint at the specified line; <c>true</c> otherwise.</returns>
		public bool RemoveBreakpointAt(int line)
		{
			if (!AllowBreakpoints) return true;
			line--;
			if (line < 0 || line >= Editor.Document.TotalNumberOfLines) return true;

			var manager = Editor.Document.BookmarkManager;
			for (int i = 0; i < manager.Marks.Count; i++)
			{
				var mark = manager.Marks[i];
				if (mark.LineNumber == line && mark is BreakpointBookmark)
				{
					manager.RemoveMark(mark);
					Editor.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, line));
					Editor.Document.CommitUpdate();
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Adds a breakpoint at the specified line.
		/// </summary>
		/// <param name="line">The line to add the breakpoint at.</param>
		public void AddBreakpointAt(int line)
		{
			if (!AllowBreakpoints) return;
			line--;
			if (line < 0 || line >= Editor.Document.TotalNumberOfLines) return;

			Editor.Document.BookmarkManager.ToggleMarkAt(new TextLocation(0, line));
			Editor.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, line));
			Editor.Document.CommitUpdate();
		}

		/// <summary>
		/// Toggles the breakpoint at the specified line.
		/// </summary>
		/// <param name="line">The line to toggle breakpoint at.</param>
		public override void ToggleBreakpointAt(int line)
		{
			if (RemoveBreakpointAt(line))
			{
				OnBreadpointRemoved(Item, line);
			}
			else
			{
				AddBreakpointAt(line);
				OnBreakpointAdded(Item, line);
			}
		}

		/// <summary>
		/// Removes all breakpoints.
		/// </summary>
		public override void ClearBreakpoints()
		{
			var marks = Editor.Document.BookmarkManager.Marks;
			for (int i = marks.Count - 1; i >= 0; i--)
			{
				var bb = marks[i] as BreakpointBookmark;
				if (bb == null) continue;
				Editor.Document.BookmarkManager.RemoveMark(bb);
			}
		}

		/// <summary>
		/// A list of breakpoints to add once the control is loaded.
		/// </summary>
		private IEnumerable<int> _bpsToAdd;

		/// <summary>
		/// Gets or sets a list of line numbers with breakpoints.
		/// </summary>
		public override IEnumerable<int> Breakpoints
		{
			get
			{
				List<int> list = new List<int>();
				foreach (var mark in Editor.Document.BookmarkManager.Marks)
				{
					if (mark is BreakpointBookmark) list.Add(mark.LineNumber + 1);
				}
				return list;
			}
			set
			{
				if (_loaded)
				{
					ClearBreakpoints();
					foreach (int line in value)
					{ AddBreakpointAt(line); }
				}
				else
				{
					_bpsToAdd = value;
				}
			}
		}
		#endregion

		#region Highlight
		/// <summary>
		/// The current highlight bookmark.
		/// </summary>
		protected HighlightBookmark Highlight { get; set; }
		/// <summary>
		/// Highlights the specified line.
		/// </summary>
		/// <param name="line">1-based index of the line to highlight. 0 or less to remove the highlight.</param>
		public override void HighlightLine(int line)
		{
			var doc = Editor.Document;
			line = Math.Min(line, doc.TotalNumberOfLines);
			line--;
			// Check if the line is already highlighted.
			if (Highlight != null && Highlight.LineNumber == line) return;

			// Remove highlight.
			if (Highlight != null)
			{
				doc.BookmarkManager.RemoveMark(Highlight);
				Highlight = null;
			}

			// Add new HighlightBookmark if line was greater than 0.
			if (line >= 0)
			{
				Highlight = new HighlightBookmark(doc, new TextLocation(0, line));
				doc.BookmarkManager.AddMark(Highlight);
				Editor.ActiveTextAreaControl.Caret.Line = line;
				Editor.ActiveTextAreaControl.Caret.Column = 0;
			}
		}
		#endregion

		/// <summary>
		/// Prompts to save changes and hides the form instead of closing it.
		/// </summary>
		private void TextEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (IsHidden || e.CloseReason == CloseReason.MdiFormClosing) return;

			e.Cancel = true;
			if (IsModified)
			{
				var result = MessageBox.Show(string.Format("\"{0}\" has been modified.\n\nWould you like to save these changes?", Title),
					"Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

				if (result == DialogResult.Yes)
				{
					Save();
				}
				else if (result == DialogResult.No)
				{
					Revert();
				}
				else // Cancel
				{
					return;
				}
			}
			IsHidden = true;
		}

		/// <summary>
		/// Gets the content of the item as text.
		/// </summary>
		/// <returns>The string representing the content.</returns>
		public override string GetText()
		{
			return Editor.Document.TextContent;
		}

		/// <summary>
		/// Loads the editor settings.
		/// </summary>
		public override void LoadSettings()
		{
			var settings = MessyLab.Properties.Settings.Default;
			Editor.Font = settings.EditorFont;
			InfoEditor.Font = settings.EditorFont;
			InfoEditor.Width = settings.EditorFont.Height * 12;
		}
	}
}
