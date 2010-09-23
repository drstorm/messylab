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

// This file is based on implementation from TextEditor sample
// by Qwertie which is licensed under The MIT License.
// http://www.codeproject.com/KB/edit/TextEditorControl.aspx
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using System.Diagnostics;
using System.IO;

namespace MessyLab
{
	public partial class FindAndReplaceForm : Form
	{
		public bool IsShown { get; set; }

		public FindAndReplaceForm()
		{
			InitializeComponent();
			_search = new TextEditorSearcher();
			IsShown = false;
		}

		TextEditorSearcher _search;
		TextEditorControl _editor;
		TextEditorControl Editor
		{
			get { return _editor; }
			set
			{
				_editor = value;
				_search.Document = _editor.Document;
				UpdateTitleBar();
			}
		}

		private void UpdateTitleBar()
		{
			string text = ReplaceMode ? "Find and Replace" : "Find";
			if (_editor != null && _editor.FileName != null)
				text += " - " + Path.GetFileName(_editor.FileName);
			if (_search.HasScanRegion)
				text += " (selection only)";
			this.Text = text;
		}

		public void ShowFor(TextEditorControl editor, bool replaceMode)
		{
			Editor = editor;

			_search.ClearScanRegion();
			var sm = editor.ActiveTextAreaControl.SelectionManager;
			if (sm.HasSomethingSelected && sm.SelectionCollection.Count == 1)
			{
				var sel = sm.SelectionCollection[0];
				if (sel.StartPosition.Line == sel.EndPosition.Line)
					findWhatTextBox.Text = sm.SelectedText;
				else
					_search.SetScanRegion(sel);
			}
			else
			{
				// Get the current word that the caret is on
				Caret caret = editor.ActiveTextAreaControl.Caret;
				int start = TextUtilities.FindWordStart(editor.Document, caret.Offset);
				int endAt = TextUtilities.FindWordEnd(editor.Document, caret.Offset);
				findWhatTextBox.Text = editor.Document.GetText(start, endAt - start);
			}

			ReplaceMode = replaceMode;

			this.Owner = (Form)editor.TopLevelControl;
			this.Show();
			IsShown = true;

			findWhatTextBox.SelectAll();
			findWhatTextBox.Focus();
		}

		public bool ReplaceMode
		{
			get { return replaceWithTextBox.Visible; }
			set
			{
				replaceButton.Visible = replaceAllButton.Visible = value;
				replaceWithLabel.Visible = replaceWithTextBox.Visible = value;
				highlightAllButton.Visible = !value;
				this.AcceptButton = value ? replaceButton : findNextButton;
				replaceToolStripButton.Checked = value;
				findToolStripButton.Checked = !value;
				UpdateTitleBar();
			}
		}

		private void findPreviousButton_Click(object sender, EventArgs e)
		{
			FindNext(false, true, "Text not found.");
		}
		private void findNextButton_Click(object sender, EventArgs e)
		{
			FindNext(false, false, "Text not found.");
		}

		public bool _lastSearchWasBackward = false;
		public bool _lastSearchLoopedAround;

		private bool IsInRange(int x, int lo, int hi)
		{
			return x >= lo && x <= hi;
		}

		public TextRange FindNext(bool viaF3, bool searchBackward, string messageIfNotFound)
		{
			if (string.IsNullOrEmpty(findWhatTextBox.Text))
			{
				MessageBox.Show("No string specified to look for.");
				return null;
			}
			_lastSearchWasBackward = searchBackward;
			_search.FindWhat = findWhatTextBox.Text;
			_search.MatchCase = matchCaseCheckBox.Checked;
			_search.MatchWholeWordOnly = matchWholeWordCheckBox.Checked;

			var caret = _editor.ActiveTextAreaControl.Caret;
			if (viaF3 && _search.HasScanRegion && !IsInRange(caret.Offset,
				_search.BeginOffset, _search.EndOffset))
			{
				// user moved outside of the originally selected region
				_search.ClearScanRegion();
				UpdateTitleBar();
			}

			int startFrom = caret.Offset - (searchBackward ? 1 : 0);
			TextRange range = _search.FindNext(startFrom, searchBackward, out _lastSearchLoopedAround);
			if (range != null)
				SelectResult(range);
			else if (messageIfNotFound != null)
				MessageBox.Show(messageIfNotFound);
			return range;
		}

		private void SelectResult(TextRange range)
		{
			TextLocation p1 = _editor.Document.OffsetToPosition(range.Offset);
			TextLocation p2 = _editor.Document.OffsetToPosition(range.Offset + range.Length);
			_editor.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
			_editor.ActiveTextAreaControl.ScrollTo(p1.Line, p1.Column);
			// Also move the caret to the end of the selection, because when the user 
			// presses F3, the caret is where we start searching next time.
			_editor.ActiveTextAreaControl.Caret.Position =
				_editor.Document.OffsetToPosition(range.Offset + range.Length);
		}

		Dictionary<TextEditorControl, HighlightGroup> _highlightGroups = new Dictionary<TextEditorControl, HighlightGroup>();

		private void highlightAllButton_Click(object sender, EventArgs e)
		{
			if (!_highlightGroups.ContainsKey(_editor))
				_highlightGroups[_editor] = new HighlightGroup(_editor);
			HighlightGroup group = _highlightGroups[_editor];

			if (string.IsNullOrEmpty(FindWhat))
				// Clear highlights
				group.ClearMarkers();
			else
			{
				_search.FindWhat = findWhatTextBox.Text;
				_search.MatchCase = matchCaseCheckBox.Checked;
				_search.MatchWholeWordOnly = matchWholeWordCheckBox.Checked;

				bool looped = false;
				int offset = 0;
				int count = 0;
				while (true)
				{
					TextRange range = _search.FindNext(offset, false, out looped);
					if (range == null || looped)
						break;
					offset = range.Offset + range.Length;
					count++;

					var m = new TextMarker(range.Offset, range.Length,
							TextMarkerType.SolidBlock, Color.LightBlue, Color.Black);
					group.AddMarker(m);
				}
				if (count == 0)
					MessageBox.Show("Search text not found.");
				else
					Close();
			}
		}

		private void FindAndReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			IsShown = false;
			// Prevent dispose, as this form can be re-used
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				if (this.Owner != null)
					this.Owner.Select(); // prevent another app from being activated instead

				e.Cancel = true;
				Hide();

				// Discard search region
				_search.ClearScanRegion();
				_editor.Refresh(); // must repaint manually
			}
		}

		private void replaceButton_Click(object sender, EventArgs e)
		{
			var sm = _editor.ActiveTextAreaControl.SelectionManager;
			if (string.Equals(sm.SelectedText, findWhatTextBox.Text, StringComparison.OrdinalIgnoreCase))
				InsertText(replaceWithTextBox.Text);
			FindNext(false, _lastSearchWasBackward, "Text not found.");
		}

		private void replaceAllButton_Click(object sender, EventArgs e)
		{
			int count = 0;
			// BUG FIX: if the replacement string contains the original search string
			// (e.g. replace "red" with "very red") we must avoid looping around and
			// replacing forever! To fix, start replacing at beginning of region (by 
			// moving the caret) and stop as soon as we loop around.
			_editor.ActiveTextAreaControl.Caret.Position =
				_editor.Document.OffsetToPosition(_search.BeginOffset);

			_editor.Document.UndoStack.StartUndoGroup();
			try
			{
				while (FindNext(false, false, null) != null)
				{
					if (_lastSearchLoopedAround)
						break;

					// Replace
					count++;
					InsertText(replaceWithTextBox.Text);
				}
			}
			finally
			{
				_editor.Document.UndoStack.EndUndoGroup();
			}
			if (count == 0)
				MessageBox.Show("No occurrances found.");
			else
			{
				MessageBox.Show(string.Format("Replaced {0} occurrances.", count));
				Close();
			}
		}

		private void InsertText(string text)
		{
			var textArea = _editor.ActiveTextAreaControl.TextArea;
			textArea.Document.UndoStack.StartUndoGroup();
			try
			{
				if (textArea.SelectionManager.HasSomethingSelected)
				{
					textArea.Caret.Position = textArea.SelectionManager.SelectionCollection[0].StartPosition;
					textArea.SelectionManager.RemoveSelectedText();
				}
				textArea.InsertString(text);
			}
			finally
			{
				textArea.Document.UndoStack.EndUndoGroup();
			}
		}

		public string FindWhat { get { return findWhatTextBox.Text; } }

		private void FindAndReplaceForm_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					e.SuppressKeyPress = true;
					e.Handled = true;
					Close();
					break;
				case Keys.F:
					if (e.Control)
					{
						e.SuppressKeyPress = true;
						e.Handled = true;
						ReplaceMode = false;
					}
					break;
				case Keys.F3:
					e.SuppressKeyPress = true;
					e.Handled = true;
					findNextButton_Click(sender, e);
					break;
				case Keys.H:
					if (e.Control)
					{
						e.SuppressKeyPress = true;
						e.Handled = true;
						ReplaceMode = true;
					}
					break;
				default:
					break;
			}
		}

		private void findToolStripButton_Click(object sender, EventArgs e)
		{
			ReplaceMode = false;
		}

		private void replaceToolStripButton_Click(object sender, EventArgs e)
		{
			ReplaceMode = true;
		}

		private void findWhatTextBox_TextChanged(object sender, EventArgs e)
		{
			bool enabled = !string.IsNullOrEmpty(FindWhat);
			findNextButton.Enabled = findPreviousButton.Enabled = enabled;
			replaceButton.Enabled = replaceAllButton.Enabled = enabled;
		}
	}

	public class TextRange : AbstractSegment
	{
		IDocument _document;
		public TextRange(IDocument document, int offset, int length)
		{
			_document = document;
			this.offset = offset;
			this.length = length;
		}
	}

	/// <summary>
	/// This class finds occurrances of a search string in a text 
	/// editor's IDocument.
	/// </summary>
	public class TextEditorSearcher
	{
		IDocument _document;
		public IDocument Document
		{
			get { return _document; }
			set
			{
				if (_document != value)
				{
					ClearScanRegion();
					_document = value;
				}
			}
		}

		// TextMarker gives the opportunity to highlight the region.
		// Note that all the markers and coloring information is associated with
		// the text document, not the editor control, so TextEditorSearcher doesn't
		// need a reference to the TextEditorControl. After adding the marker to
		// the document, we must remember to remove it when it is no longer needed.
		TextMarker _region = null;
		
		/// <summary>
		/// Sets the region to search. The region is updated 
		/// automatically as the document changes.
		/// </summary>
		public void SetScanRegion(ISelection sel)
		{
			SetScanRegion(sel.Offset, sel.Length);
		}

		private Color HalfMix(Color one, Color two)
		{
			return Color.FromArgb(
				(one.A + two.A) >> 1,
				(one.R + two.R) >> 1,
				(one.G + two.G) >> 1,
				(one.B + two.B) >> 1);
		}

		/// <summary>Sets the region to search. The region is updated 
		/// automatically as the document changes.</summary>
		public void SetScanRegion(int offset, int length)
		{
			var bkgColor = _document.HighlightingStrategy.GetColorFor("Default").BackgroundColor;
			_region = new TextMarker(offset, length, TextMarkerType.SolidBlock,
				HalfMix(bkgColor, Color.FromArgb(160, 160, 160)));
			_document.MarkerStrategy.AddMarker(_region);
		}

		public bool HasScanRegion
		{
			get { return _region != null; }
		}

		public void ClearScanRegion()
		{
			if (_region != null)
			{
				_document.MarkerStrategy.RemoveMarker(_region);
				_region = null;
			}
		}

		/// <summary>Begins the start offset for searching</summary>
		public int BeginOffset
		{
			get
			{
				if (_region != null)
					return _region.Offset;
				else
					return 0;
			}
		}
		/// <summary>Begins the end offset for searching</summary>
		public int EndOffset
		{
			get
			{
				if (_region != null)
					return _region.EndOffset;
				else
					return _document.TextLength;
			}
		}

		public bool MatchCase;

		public bool MatchWholeWordOnly;

		string _findWhat;
		string _findWhat2; // uppercase in case-insensitive mode
		public string FindWhat
		{
			get { return _findWhat; }
			set { _findWhat = value; }
		}

		private int InRange(int x, int lo, int hi)
		{
			Debug.Assert(lo <= hi);
			return x < lo ? lo : (x > hi ? hi : x);
		}

		/// <summary>Finds next instance of FindWhat, according to the search rules 
		/// (MatchCase, MatchWholeWordOnly).</summary>
		/// <param name="beginAtOffset">Offset in Document at which to begin the search</param>
		/// <remarks>If there is a match at beginAtOffset precisely, it will be returned.</remarks>
		/// <returns>Region of document that matches the search string</returns>
		public TextRange FindNext(int beginAtOffset, bool searchBackward, out bool loopedAround)
		{
			Debug.Assert(!string.IsNullOrEmpty(_findWhat));
			loopedAround = false;

			int startAt = BeginOffset;
			int endAt = EndOffset;
			int curOffs = InRange(beginAtOffset, startAt, endAt);

			_findWhat2 = MatchCase ? _findWhat : _findWhat.ToUpperInvariant();

			TextRange result;
			if (searchBackward)
			{
				result = FindNextIn(startAt, curOffs, true);
				if (result == null)
				{
					loopedAround = true;
					result = FindNextIn(curOffs, endAt, true);
				}
			}
			else
			{
				result = FindNextIn(curOffs, endAt, false);
				if (result == null)
				{
					loopedAround = true;
					result = FindNextIn(startAt, curOffs, false);
				}
			}
			return result;
		}

		private TextRange FindNextIn(int offset1, int offset2, bool searchBackward)
		{
			Debug.Assert(offset2 >= offset1);
			offset2 -= _findWhat.Length;

			// Make behavior decisions before starting search loop
			Func<char, char, bool> matchFirstCh;
			Func<int, bool> matchWord;
			if (MatchCase)
				matchFirstCh = (findWhat, c) => (findWhat == c);
			else
				matchFirstCh = (findWhat, c) => (findWhat == Char.ToUpperInvariant(c));
			if (MatchWholeWordOnly)
				matchWord = IsWholeWordMatch;
			else
				matchWord = IsPartWordMatch;

			// Search
			char findWhatCh = _findWhat2[0];
			if (searchBackward)
			{
				for (int offset = offset2; offset >= offset1; offset--)
				{
					if (matchFirstCh(findWhatCh, _document.GetCharAt(offset))
						&& matchWord(offset))
						return new TextRange(_document, offset, _findWhat.Length);
				}
			}
			else
			{
				for (int offset = offset1; offset <= offset2; offset++)
				{
					if (matchFirstCh(findWhatCh, _document.GetCharAt(offset))
						&& matchWord(offset))
						return new TextRange(_document, offset, _findWhat.Length);
				}
			}
			return null;
		}

		private bool IsWholeWordMatch(int offset)
		{
			if (IsWordBoundary(offset) && IsWordBoundary(offset + _findWhat.Length))
				return IsPartWordMatch(offset);
			else
				return false;
		}

		private bool IsWordBoundary(int offset)
		{
			return offset <= 0 || offset >= _document.TextLength ||
				!IsAlphaNumeric(offset - 1) || !IsAlphaNumeric(offset);
		}

		private bool IsAlphaNumeric(int offset)
		{
			char c = _document.GetCharAt(offset);
			return Char.IsLetterOrDigit(c) || c == '_';
		}

		private bool IsPartWordMatch(int offset)
		{
			string substr = _document.GetText(offset, _findWhat.Length);
			if (!MatchCase)
				substr = substr.ToUpperInvariant();
			return substr == _findWhat2;
		}
	}

	/// <summary>
	/// Bundles a group of markers together so that they can be cleared 
	/// together.
	/// </summary>
	public class HighlightGroup
	{
		List<TextMarker> _markers = new List<TextMarker>();
		TextEditorControl _editor;
		IDocument _document;
		public HighlightGroup(TextEditorControl editor)
		{
			_editor = editor;
			_document = editor.Document;
		}
		public void AddMarker(TextMarker marker)
		{
			_markers.Add(marker);
			_document.MarkerStrategy.AddMarker(marker);
		}
		public void ClearMarkers()
		{
			foreach (TextMarker m in _markers)
				_document.MarkerStrategy.RemoveMarker(m);
			_markers.Clear();
			_editor.Refresh();
		}
		public IList<TextMarker> Markers { get { return _markers.AsReadOnly(); } }
	}
}
