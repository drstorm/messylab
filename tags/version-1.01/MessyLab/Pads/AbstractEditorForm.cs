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
using System.Linq;
using System.Text;

namespace MessyLab
{
	/// <summary>
	/// An Abstract editor form.
	/// </summary>
	public abstract class
// Please refer to the documentation of the dummy
// AbstractEditorForm class defined below for an
// explanation for these #if directives.
#if DEBUG
		RealAbstractEditorForm
#else
		AbstractEditorForm
#endif
		: WeifenLuo.WinFormsUI.Docking.DockContent
	{

#if DEBUG
		public RealAbstractEditorForm()
#else
		public AbstractEditorForm()
#endif
		{
			DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
			Text = TabText = "Untitled";
			CommentMarker = ";";
		}

		public delegate void BreakpointToggledHandler(ProjectItem item, int line);

		/// <summary>
		/// Occurs when a breakpoint is added.
		/// </summary>
		public event BreakpointToggledHandler BreakpointAdded;
		protected void OnBreakpointAdded(ProjectItem item, int line)
		{ if (BreakpointAdded != null) BreakpointAdded(item, line); }
		
		/// <summary>
		/// Occurs when a breakpoint is removed.
		/// </summary>
		public event BreakpointToggledHandler BreadpointRemoved;
		protected void OnBreadpointRemoved(ProjectItem item, int line)
		{ if (BreadpointRemoved != null) BreadpointRemoved(item, line); }

		/// <summary>
		/// A value indicating whether the document is modified.
		/// </summary>
		public abstract bool IsModified { get; protected set; }

		/// <summary>
		/// Document title.
		/// </summary>
		public abstract string Title { get; set; }

		/// <summary>
		/// A value indicating whether the document is read-only.
		/// </summary>
		public abstract bool IsReadOnly { get; set; }

		/// <summary>
		/// The project item.
		/// </summary>
		public ProjectItem Item { get; set; }

		/// <summary>
		/// Opens the specified project item.
		/// </summary>
		/// <param name="item">The project item to open.</param>
		public virtual void Open(ProjectItem item)
		{
			Item = item;
			Open(item.Path);
		}

		/// <summary>
		/// Opens the specified file.
		/// </summary>
		/// <param name="path">The path of the file to open.</param>
		public abstract void Open(string path);
		/// <summary>
		/// Saves changes.
		/// </summary>
		/// <returns>A value indicating success.</returns>
		public abstract bool Save();
		/// <summary>
		/// Reverts the document to the last saved state.
		/// </summary>
		public abstract void Revert();
		/// <summary>
		/// Prints the document.
		/// </summary>
		public abstract void Print();

		/// <summary>
		/// Undos the last action.
		/// </summary>
		public abstract void Undo();
		/// <summary>
		/// Redos the last undone action.
		/// </summary>
		public abstract void Redo();

		/// <summary>
		/// Copies selection to the clipboard.
		/// </summary>
		public abstract void Copy();
		/// <summary>
		/// Cuts selection to the clipboard.
		/// </summary>
		public abstract void Cut();
		/// <summary>
		/// Pastes from the clipboard.
		/// </summary>
		public abstract void Paste();

		/// <summary>
		/// Selects all.
		/// </summary>
		public abstract void SelectAll();
		
		/// <summary>
		/// Goes to the specified line.
		/// </summary>
		/// <param name="line">The line number to go to.</param>
		public abstract void GoToLine(int line);
		/// <summary>
		/// Displays a dialog to enter a line to go to.
		/// </summary>
		public abstract void GoTo();

		/// <summary>
		/// Changes the case of the selection.
		/// </summary>
		/// <param name="toUpper">A value indicating whether to change to upper-case; otherwise lower-case.</param>
		public abstract void ChangeSelectionCase(bool toUpper);
		/// <summary>
		/// Changes the case of the selection to upper-case.
		/// </summary>
		public virtual void SelectionToUpper() { ChangeSelectionCase(true); }
		/// <summary>
		/// Changes the case of the selection to lower-case.
		/// </summary>
		public virtual void SelectionToLower() { ChangeSelectionCase(false); }

		/// <summary>
		/// Sets of gets the comment marker, e. g. ";".
		/// </summary>
		public string CommentMarker { get; set; }
		/// <summary>
		/// Comments or uncomments selection using the specified comment marker.
		/// </summary>
		/// <param name="comment">A value indicating whether to comment the selection; otherwise uncomment.</param>
		/// <param name="commentMarker">The comment marker.</param>
		public abstract void CommentSelection(bool comment, string commentMarker);
		/// <summary>
		/// Comments or uncomments selection using the default comment marker.
		/// </summary>
		/// <param name="comment">A value indicating whether to comment the selection; otherwise uncomment.</param>
		public virtual void CommentSelection(bool comment) { CommentSelection(comment, CommentMarker); }

		/// <summary>
		/// Idents the selection.
		/// </summary>
		/// <param name="increase">A value indicating whether to increase ident; otherwise decrease.</param>
		public abstract void Ident(bool increase);

		/// <summary>
		/// Displays the find dialog.
		/// </summary>
		public abstract void Find();
		/// <summary>
		/// Finds the next instance.
		/// </summary>
		public abstract void FindNext();
		/// <summary>
		/// Finds the previous instance.
		/// </summary>
		public abstract void FindPrevious();
		/// <summary>
		/// Replaces the current instance.
		/// </summary>
		public abstract void Replace();

		/// <summary>
		/// A value indicating whether an Undo is possible.
		/// </summary>
		public abstract bool CanUndo { get; }
		/// <summary>
		/// A value indicating whether a Redo is possible.
		/// </summary>
		public abstract bool CanRedo { get; }

		/// <summary>
		/// A value indicating whether Cut is possible.
		/// </summary>
		public abstract bool CanCut { get; }
		/// <summary>
		/// A value indicating whether Copy is possible.
		/// </summary>
		public abstract bool CanCopy { get; }
		/// <summary>
		/// A value indicating whether Paste is possible.
		/// </summary>
		public abstract bool CanPaste { get; }

		/// <summary>
		/// A value indicating whether Select All is possible.
		/// </summary>
		public abstract bool CanSelectAll { get; }
		/// <summary>
		/// A value indicating whether Find or Replace are possible.
		/// </summary>
		public abstract bool CanFindReplace { get; }

		/// <summary>
		/// A value indicating whether Go To is possible.
		/// </summary>
		public abstract bool CanGoTo { get; }

		/// <summary>
		/// A value indicating whether advanced text editing is possible, e.g. commenting, changing ident, etc.
		/// </summary>
		public abstract bool CanDoAdvancedTextEditing { get; }

		/// <summary>
		/// A value indicating whether Breakpoints are allowed.
		/// </summary>
		public abstract bool AllowBreakpoints { get; set; }
		/// <summary>
		/// Toggles the breakpoint at ste current line.
		/// </summary>
		public abstract void ToggleBreakpoint();
		/// <summary>
		/// Toggles the breakpoint at the specified line.
		/// </summary>
		/// <param name="line">The line to toggle breakpoint at.</param>
		public abstract void ToggleBreakpointAt(int line);
		/// <summary>
		/// Removes all breakpoints.
		/// </summary>
		public abstract void ClearBreakpoints();
		/// <summary>
		/// Gets or sets a list of line numbers with breakpoints.
		/// </summary>
		public abstract IEnumerable<int> Breakpoints { get; set; }

		/// <summary>
		/// Highlights the specified line.
		/// </summary>
		/// <param name="line">1-based index of the line to highlight. 0 or less to remove the highlight.</param>
		public abstract void HighlightLine(int line);

		/// <summary>
		/// Gets the content of the item as text.
		/// </summary>
		/// <returns>The string representing the content.</returns>
		public abstract string GetText();

		/// <summary>
		/// Loads the editor settings.
		/// </summary>
		public abstract void LoadSettings();
	}

#if DEBUG
	/// <summary>
	/// This class is used as a workaround for the problem caused
	/// by the limitations of the Visual Studio form designer.
	/// </summary>
	/// <remarks>
	/// <para>The form designer does not support inheriting
	/// asbtract form classes. Because of this, in DEBUG mode,
	/// the original <c>AbstractEditorForm</c> is replaced with
	/// this dummy implementation.</para>
	/// <para>Using the #if directives the original <c>AbstractEditorForm</c>
	/// is renamed to <c>RealAbstractEditorForm</c> in Debug mode.</para>
	/// <para>There should be no side effects of this hack other
	/// than having to switch to Debug Configuration to use the
	/// Form Designer, and to Release Configuration to use
	/// automatic stub generation for abstract members.</para>
	/// </remarks>
	public class AbstractEditorForm : RealAbstractEditorForm
	{
		[System.ComponentModel.Browsable(false)]
		public override bool IsModified { get { throw new NotImplementedException(); } protected set { } }
		[System.ComponentModel.Browsable(false)]
		public override string Title { get { return null; } set { } }
		public override bool IsReadOnly { get { return false; } set { } }
		public override void Open(string path) { throw new NotImplementedException(); }
		public override bool Save() { throw new NotImplementedException(); }
		public override void Revert() { throw new NotImplementedException(); }
		public override void Print() { throw new NotImplementedException(); }
		public override void Undo() { throw new NotImplementedException(); }
		public override void Redo() { throw new NotImplementedException(); }
		public override void Copy() { throw new NotImplementedException(); }
		public override void Cut() { throw new NotImplementedException(); }
		public override void Paste() { throw new NotImplementedException(); }
		public override void SelectAll() { throw new NotImplementedException(); }
		public override void GoToLine(int line) { throw new NotImplementedException(); }
		public override void GoTo() { throw new NotImplementedException(); }
		public override void ChangeSelectionCase(bool toUpper) { throw new NotImplementedException(); }
		public override void CommentSelection(bool comment, string commentMarker) { throw new NotImplementedException(); }
		public override void Ident(bool increase) { throw new NotImplementedException(); }
		public override void Find() { throw new NotImplementedException(); }
		public override void FindNext() { throw new NotImplementedException(); }
		public override void FindPrevious() { throw new NotImplementedException(); }
		public override void Replace() { throw new NotImplementedException(); }
		[System.ComponentModel.Browsable(false)]
		public override bool CanUndo { get { throw new NotImplementedException(); } }
		[System.ComponentModel.Browsable(false)]
		public override bool CanRedo { get { throw new NotImplementedException(); } }
		[System.ComponentModel.Browsable(false)]
		public override bool CanCut { get { throw new NotImplementedException(); } }
		[System.ComponentModel.Browsable(false)]
		public override bool CanCopy { get { throw new NotImplementedException(); } }
		[System.ComponentModel.Browsable(false)]
		public override bool CanPaste { get { throw new NotImplementedException(); } }
		[System.ComponentModel.Browsable(false)]
		public override bool CanSelectAll { get { throw new NotImplementedException(); } }
		[System.ComponentModel.Browsable(false)]
		public override bool CanFindReplace { get { throw new NotImplementedException(); } }
		[System.ComponentModel.Browsable(false)]
		public override bool CanGoTo { get { throw new NotImplementedException(); } }
		[System.ComponentModel.Browsable(false)]
		public override bool CanDoAdvancedTextEditing { get { throw new NotImplementedException(); } }
		[System.ComponentModel.Browsable(false)]
		public override bool AllowBreakpoints { get { return false; } set { } }
		public override void ToggleBreakpoint() { throw new NotImplementedException(); }
		public override void ToggleBreakpointAt(int line) { throw new NotImplementedException(); }
		public override void ClearBreakpoints() { throw new NotImplementedException(); }
		[System.ComponentModel.Browsable(false)]
		public override IEnumerable<int> Breakpoints { get { return null; } set { } }
		public override void HighlightLine(int line) { throw new NotImplementedException(); }
		public override string GetText() { throw new NotImplementedException(); }

		public override void LoadSettings() { throw new NotImplementedException(); }
	}
#endif
}
