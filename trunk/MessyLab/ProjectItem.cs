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
	/// Represents a project item, i.e. a source file.
	/// </summary>
	public class ProjectItem
	{
		/// <summary>
		/// The name of the item file.
		/// </summary>
		/// <example>Main.pca</example>
		public string Filename
		{
			get
			{
				string res = string.Empty;
				try
				{ res = System.IO.Path.GetFileName(Path); }
				catch { }
				return res;
			}
		}

		/// <summary>
		/// Path to the item file relative to the project directory.
		/// </summary>
		/// <example>Main\Main.pca</example>
		public string RelativePath { get; set; }

		/// <summary>
		/// Full path to the item file.
		/// </summary>
		/// <example>
		/// c:\Users\JSmith\Ducuments\Messy Lab Projects\UberCode\Main\Main.pca
		/// </example>
		public string Path
		{
			get
			{
				string res = string.Empty;
				try
				{
					string dir = System.IO.Path.GetDirectoryName(Project.Path);
					res = System.IO.Path.Combine(dir, RelativePath);
				}
				catch { }
				return res;
			}
		}

		/// <summary>
		/// The parent project.
		/// </summary>
		public Project Project { get; set; }

		/// <summary>
		/// Saves the changes to the item.
		/// </summary>
		/// <remarks>
		/// Finds the open editor for the item (if any) and
		/// calls its Save functionality.
		/// </remarks>
		public void Save()
		{
			var platform = Project.Platform;
			var editor = platform.Editors.GetEditorFormIfExists(this);
			if (editor != null)
			{
				if (editor.IsModified)
				{ editor.Save(); }
			}
		}

		/// <summary>
		/// Gets an existing Editor Form for the item or creates one.
		/// </summary>
		/// <returns>The Editor Form for the item.</returns>
		public AbstractEditorForm GetEditorForm()
		{
			var platform = Project.Platform;
			return platform.Editors.GetEditorForm(this);
		}

		/// <summary>
		/// Gets text lines representing the item.
		/// </summary>
		/// <returns>The lines representing the item.</returns>
		public virtual string[] GetLines()
		{
			string[] lines;
			var editor = Project.Platform.Editors.GetEditorFormIfExists(this);
			if (editor != null)
			{
				// Get lines from open editor.

				string s = editor.GetText();
				lines = s.Split(new string[] { "\r\n" }, StringSplitOptions.None);
				
				// If there is less than 2 lines, it is likely that an alternate New Line encoding is used.
				if (lines.Length < 2)
				{
					lines = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				}
			}
			else
			{
				// Get lines directly from file.

				try
				{
					lines = System.IO.File.ReadAllLines(Path);
				}
				catch
				{ lines = new string[] { }; }
			}
			return lines;
		}

		/// <summary>
		/// Gets a list of tasks for the item by searching
		/// for 'TODO' markers in the text.
		/// </summary>
		/// <returns>A list of Tasks for the item.</returns>
		public virtual IEnumerable<ListItem> GetTasks()
		{
			var l = new List<ListItem>();
			string[] lines = GetLines();
			int line = 0;
			foreach (string s in lines)
			{
				line++;
				int i = s.IndexOf("todo", StringComparison.OrdinalIgnoreCase);
				if (i != -1)
				{
					string description = s;
					if (i > 0) { description = s.Remove(0, i - 1); }
					l.Add(new ListItem(description, this, line, i + 1));
				}
			}
			return l;
		}
	}
}
