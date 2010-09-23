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
using System.Drawing;

namespace MessyLab
{
	/// <summary>
	/// Highlighted (current) line bookmark.
	/// </summary>
	public class HighlightBookmark : MarkerBookmark
	{
		public HighlightBookmark(IDocument document, ICSharpCode.TextEditor.TextLocation location) : base(document, location) { }

		/// <summary>
		/// Draws an arrow.
		/// </summary>
		public override void Draw(ICSharpCode.TextEditor.IconBarMargin margin, Graphics g, Point p)
		{
			margin.DrawArrow(g, p.Y);
		}

		public override bool CanToggle
		{
			get { return false; }
		}

		protected override TextMarker CreateMarker()
		{
			LineSegment lineSeg = Anchor.Line;
			TextMarker marker = new TextMarker(lineSeg.Offset, lineSeg.Length, TextMarkerType.SolidBlock, Color.Yellow);
			return marker;
		}
	}
}
