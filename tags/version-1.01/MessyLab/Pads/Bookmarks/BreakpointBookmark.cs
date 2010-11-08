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
using ICSharpCode.TextEditor;
using System.Drawing;
using ICSharpCode.TextEditor.Document;

namespace MessyLab
{
	/// <summary>
	/// Breakpoint bookmark.
	/// </summary>
	public class BreakpointBookmark : MarkerBookmark
	{
		public BreakpointBookmark(IDocument document, ICSharpCode.TextEditor.TextLocation location, AbstractEditorForm editorForm)
			: base(document, location)
		{
			EditorForm = editorForm;
		}

		public AbstractEditorForm EditorForm { get; set; }

		/// <summary>
		/// Draws the breakpoint icon.
		/// </summary>
		public override void Draw(ICSharpCode.TextEditor.IconBarMargin margin, System.Drawing.Graphics g, System.Drawing.Point p)
		{
			margin.DrawBreakpoint(g, p.Y, true, true);
		}

		protected override TextMarker CreateMarker()
		{
			LineSegment lineSeg = Anchor.Line;
			TextMarker marker = new TextMarker(lineSeg.Offset, lineSeg.Length, TextMarkerType.SolidBlock,
				Color.FromArgb(180, 38, 38), Color.White);
			return marker;
		}

		/// <summary>
		/// Handles the click event.
		/// </summary>
		/// <returns>A value indicating whether the click was handled.</returns>
		public override bool Click(System.Windows.Forms.Control parent, System.Windows.Forms.MouseEventArgs e)
		{
			EditorForm.ToggleBreakpointAt(Location.Line + 1);
			return true;
		}
	}

	/// <summary>
	/// An IBookmarkFactory that produces breakpoint bookmarks.
	/// </summary>
	public class BreakpointBookmarkFactory : IBookmarkFactory
	{
		public BreakpointBookmarkFactory(AbstractEditorForm editorForm)
		{
			EditorForm = editorForm;
		}

		public AbstractEditorForm EditorForm { get; set; }

		public Bookmark CreateBookmark(IDocument document, ICSharpCode.TextEditor.TextLocation location)
		{
			return new BreakpointBookmark(document, location, EditorForm);
		}
	}
}
