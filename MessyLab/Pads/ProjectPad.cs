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
	/// Project management pad.
	/// </summary>
	public partial class ProjectPad : Pad
	{
		public ProjectPad(Project project)
			: base(project)
		{
			InitializeComponent();
			CreateMenuItem("&Project Explorer", MessyLab.Properties.Resources.ProjectExplorer);
			CreateToolbarItem("Project Explorer", MessyLab.Properties.Resources.ProjectExplorer);
			ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;

			project.Saved += UpdateItemList;
		}
		public ProjectPad() : this(null) { }

		/// <summary>
		/// Updates list according to the items of the current project.
		/// </summary>
		public void UpdateItemList()
		{
			listView.Items.Clear();
			if (Project == null) return;
			foreach (var item in Project.Items)
			{
				var it = new ListViewItem(ReferenceEquals(Project.MainItem, item) ? "*" : string.Empty);
				it.SubItems.Add(item.Filename);
				it.Tag = item;
				listView.Items.Add(it);
			}
		}

		/// <summary>
		/// Selected project item.
		/// </summary>
		public ProjectItem SelectedItem { get; protected set; }

		/// <summary>
		/// Occurs when the Add new item command is clicked.
		/// </summary>
		public event Action AddNewItemClicked;
		/// <summary>
		/// Occurs when the Add existing item command is clicked.
		/// </summary>
		public event Action AddExistingItemClicked;
		/// <summary>
		/// Occurs when the Remove command is clicked.
		/// </summary>
		public event Action RemoveClicked;
		/// <summary>
		/// Occurs when an item is double-clicked.
		/// </summary>
		public event Action ItemDoubleClicked;

		protected void OnAddNewItemClicked()
		{ if (AddNewItemClicked != null) AddNewItemClicked(); }
		protected void OnAddExistingItemClicked()
		{ if (AddExistingItemClicked != null) AddExistingItemClicked(); }
		protected void OnRemoveClicked()
		{ if (RemoveClicked != null) RemoveClicked(); }
		protected void OnItemDoubleClicked()
		{ if (ItemDoubleClicked != null) ItemDoubleClicked(); }

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count == 1)
			{
				SelectedItem = listView.SelectedItems[0].Tag as ProjectItem;
			}
		}

		protected void OpenSelected()
		{
			if (SelectedItem != null)
			{
				OnItemDoubleClicked();
			}
		}

		private void listView_DoubleClick(object sender, EventArgs e)
		{
			OpenSelected();
		}

		private void addNewFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnAddNewItemClicked();
		}

		private void addExistingFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnAddExistingItemClicked();
		}

		private void removeToolStripButton_Click(object sender, EventArgs e)
		{
			OnRemoveClicked();
		}

		private void ProjectPad_Load(object sender, EventArgs e)
		{
			UpdateItemList();
		}

		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			if (listView.SelectedItems.Count != 1) e.Cancel = true;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenSelected();
		}

		private void setAsMainToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (SelectedItem != null)
			{
				Project.MainItem = SelectedItem;
				Project.Save();
			}
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnRemoveClicked();
		}

		private void listView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				OpenSelected();
			}
		}

	}
}
