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
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;

namespace MessyLab
{
	/// <summary>
	/// A bookmark coupled with a text marker.
	/// </summary>
	public abstract class MarkerBookmark : Bookmark
	{
		public MarkerBookmark(IDocument document, TextLocation location)
			: base(document, location)
		{
			SetMarker();
		}

		/// <summary>
		/// Creates the marker.
		/// </summary>
		/// <returns>The created text marker.</returns>
		protected abstract TextMarker CreateMarker();

		private TextMarker _marker;
		
		/// <summary>
		/// Sets marker in the current document.
		/// </summary>
		private void SetMarker()
		{
			RemoveMarker();
			if (Document != null)
			{
				_marker = CreateMarker();
				Document.MarkerStrategy.AddMarker(_marker);
				Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, LineNumber));
				Document.CommitUpdate();
			}
		}

		/// <summary>
		/// Gets the line number within the document for the specified offset.
		/// </summary>
		/// <returns>The line number for the offset.</returns>
		private int GetLineNumberForOffset(IDocument document, int offset)
		{
			if (offset <= 0)
				return 0;
			if (offset >= document.TextLength)
				return document.TotalNumberOfLines;
			return document.GetLineNumberForOffset(offset);
		}

		/// <summary>
		/// Removes the marker.
		/// </summary>
		public void RemoveMarker()
		{
			if (Document != null && _marker != null)
			{
				Document.MarkerStrategy.RemoveMarker(_marker);

				int from = GetLineNumberForOffset(Document, _marker.Offset);
				int to = GetLineNumberForOffset(Document, _marker.Offset + _marker.Length);
				Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.LinesBetween, from, to));
				Document.CommitUpdate();
			}
			_marker = null;
		}
	}
}
