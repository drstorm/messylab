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

namespace MessyLab
{
	/// <summary>
	/// A generic list pad.
	/// </summary>
	public partial class ListPad : Pad
	{
		public ListPad(Project project)
			: base(project)
		{
			InitializeComponent();
			ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;
			List = new List<ListItem>();
		}
		public ListPad() : this(null) { }

		/// <summary>
		/// Refreshes the list view using the data from the List object.
		/// </summary>
		public virtual void RefreshList()
		{
			try
			{
				SuspendLayout();
				listView.Items.Clear();
				int n = 1;
				foreach (var item in List)
				{
					item.Number = n++;
					ListViewItem i = new ListViewItem();
					i.SubItems.Add(item.Number.ToString());
					i.SubItems.Add(item.Description);
					i.SubItems.Add(item.File.ToString());
					i.SubItems.Add(item.Line.ToString());
					i.SubItems.Add(item.Column.ToString());
					i.Tag = item;
					if (listView.SmallImageList != null) i.ImageIndex = 0;
					listView.Items.Add(i);
				}
			}
			finally
			{
				ResumeLayout();
			}
		}

		/// <summary>
		/// The list to display.
		/// </summary>
		protected List<ListItem> List { get; set; }

		/// <summary>
		/// Gets or sets the list of items to display.
		/// </summary>
		public virtual IEnumerable<ListItem> Items
		{
			get { return List; }
			set
			{
				List.Clear();
				foreach (var item in value)
				{
					List.Add(item);
				}
				RefreshList();
			}
		}

		/// <summary>
		/// Adds the specified item to the list.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public virtual void AddItem(ListItem item)
		{
			List.Add(item);
			RefreshList();
		}

		/// <summary>
		/// Creates and adds an item using the specified data.
		/// </summary>
		/// <param name="description">The item description.</param>
		/// <param name="projectItem">The name of the project item.</param>
		/// <param name="line">The source code line.</param>
		/// <param name="column">The source code column.</param>
		/// <returns>The created ListItem.</returns>
		public virtual ListItem AddItem(string description, ProjectItem projectItem, int line, int column)
		{
			ListItem item = new ListItem(description, projectItem, line, column);
			AddItem(item);
			return item;
		}

		/// <summary>
		/// Clears the list.
		/// </summary>
		public virtual void ClearItems()
		{
			List.Clear();
			RefreshList();
		}

		private void listView_DoubleClick(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count == 1)
			{
				var item = listView.SelectedItems[0].Tag as ListItem;
				item.OnListItemDoubleClick();
			}
		}

		private void listView_Resize(object sender, EventArgs e)
		{
			if (DockState == WeifenLuo.WinFormsUI.Docking.DockState.Hidden ||
				DockState == WeifenLuo.WinFormsUI.Docking.DockState.Unknown ||
				listView.Columns.Count < 3)
			{ return; }

			int w = listView.Width - 24;
			foreach (ColumnHeader col in listView.Columns)
			{
				if (col.DisplayIndex == 2) continue;
				w -= col.Width;
			}
			listView.Columns[2].Width = w;
		}

		private void ListPad_DockStateChanged(object sender, EventArgs e)
		{
			listView_Resize(sender, e);
		}

	}

	/// <summary>
	/// Represents a list item to display, i.e. a model.
	/// </summary>
	public class ListItem
	{
		public ListItem(string description, ProjectItem projectItem, int line, int column)
		{
			Description = description;
			ProjectItem = projectItem;
			Line = line;
			Column = column;
		}

		public int Number { get; set; }
		public string Description { get; set; }
		public string File { get { return ProjectItem.Filename; } }
		public ProjectItem ProjectItem { get; set; }
		public int Line { get; set; }
		public int Column { get; set; }

		/// <summary>
		/// Occurs when the item has been double-clicked.
		/// </summary>
		public event ListItemDoubleClickHandler ListItemDoubleClick;

		/// <summary>
		/// Opens the editor representing the item and
		/// fires the ListItemDoubleClick event.
		/// </summary>
		protected internal void OnListItemDoubleClick()
		{
			try
			{
				var pl = ProjectItem.Project.Platform;
				pl.Gui.MainForm.ShowEditorFor(ProjectItem);
				var ed = pl.Editors.GetEditorFormIfExists(ProjectItem) as TextEditorForm;
				if (ed != null)
				{
					var caret = ed.Editor.ActiveTextAreaControl.Caret;
					caret.Line = Line - 1;
					caret.Column = Column - 1;
				}
			}
			catch { }
			if (ListItemDoubleClick != null) ListItemDoubleClick(this);
		}
	}

	public delegate void ListItemDoubleClickHandler(ListItem sender);
}
