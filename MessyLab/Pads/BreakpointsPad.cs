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
using MessyLab.Platforms;

namespace MessyLab
{
	/// <summary>
	/// The pad for breakpoint management.
	/// </summary>
	public partial class BreakpointsPad : Pad
	{
		public BreakpointsPad(Project project)
			: base(project)
		{
			InitializeComponent();
			CreateMenuItem("&Breakpoints", MessyLab.Properties.Resources.Breakpoints);
			CreateToolbarItem("Breakpoints", MessyLab.Properties.Resources.Breakpoints);
			ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;

			ListViewItems = new Dictionary<BreakpointController.BreakpointDefinition, ListViewItem>();
			Breakpoints = new Dictionary<ListViewItem, BreakpointController.BreakpointDefinition>();

			NewMemoryBreakpointDialog = new NewMemoryBreakpointForm();
			NewRegisterBreakpointDialog = new NewRegisterBreakpointForm();
			NewIOBreakpointDialog = new NewIOBreakpointForm();
		}
		public BreakpointsPad() : this(null) { }

		public INewBreakpointDialog NewMemoryBreakpointDialog { get; set; }
		public INewBreakpointDialog NewRegisterBreakpointDialog { get; set; }
		public INewBreakpointDialog NewIOBreakpointDialog { get; set; }

		public BreakpointController BreakpointController { get; set; }

		/// <summary>
		/// Adds a new memory breakpoint.
		/// </summary>
		private void memoryBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (BreakpointController == null || NewMemoryBreakpointDialog == null) return;

			BreakpointController.BreakpointDefinition bp;
			if (NewMemoryBreakpointDialog.ShowNewBreakpointDialog(out bp, BreakpointController))
			{
				BreakpointController.AddBreakpoint(bp);
				UpdateList();
			}
		}

		/// <summary>
		/// Adds a new register breakpoint.
		/// </summary>
		private void registerBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (BreakpointController == null || NewRegisterBreakpointDialog == null) return;

			BreakpointController.BreakpointDefinition bp;
			if (NewRegisterBreakpointDialog.ShowNewBreakpointDialog(out bp, BreakpointController))
			{
				BreakpointController.AddBreakpoint(bp);
				UpdateList();
			}
		}

		/// <summary>
		/// Adds a new I/O breakpoint.
		/// </summary>
		private void iOBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (BreakpointController == null || NewIOBreakpointDialog == null) return;

			BreakpointController.BreakpointDefinition bp;
			if (NewIOBreakpointDialog.ShowNewBreakpointDialog(out bp, BreakpointController))
			{
				BreakpointController.AddBreakpoint(bp);
				UpdateList();
			}
		}

		protected Dictionary<BreakpointController.BreakpointDefinition, ListViewItem> ListViewItems { get; private set; }
		protected Dictionary<ListViewItem, BreakpointController.BreakpointDefinition> Breakpoints { get; private set; }

		/// <summary>
		/// Updates the list by using the data from the breakpoint controller.
		/// </summary>
		public void UpdateList()
		{
			var oldList = ListViewItems;
			ListViewItems = new Dictionary<BreakpointController.BreakpointDefinition, ListViewItem>();

			foreach (var bp in BreakpointController.Breakpoints)
			{
				if (bp is BreakpointController.LineBreakpointDefinition) continue;

				ListViewItem item;
				if (oldList.TryGetValue(bp, out item))
				{
					item.Text = bp.Description;
					item.SubItems[1].Text = bp.Flags;

					ListViewItems[bp] = oldList[bp];
					oldList.Remove(bp);
				}
				else
				{
					item = new ListViewItem(bp.Description);
					item.SubItems.Add(bp.Flags);

					listView.Items.Add(item);

					ListViewItems[bp] = item;
				}

				if (bp is BreakpointController.MemoryBreakpointDefinition)
				{
					item.Group = listView.Groups["memory"];
				}
				else if (bp is BreakpointController.RegisterBreakpointDefinition)
				{
					item.Group = listView.Groups["register"];
				}
				else if (bp is BreakpointController.IOBreakpointDefinition)
				{
					item.Group = listView.Groups["io"];
				}
			}

			foreach (var item in oldList.Values)
			{
				listView.Items.Remove(item);
			}

			Breakpoints.Clear();
			foreach (var bp in ListViewItems.Keys)
			{
				Breakpoints.Add(ListViewItems[bp], bp);
			}
		}

		/// <summary>
		/// Removes the specified breakpoint from the breakpoint controller and updates the list.
		/// </summary>
		/// <param name="breakpointDefinition">The breakpoint to remove.</param>
		private void RemoveBreakpoint(BreakpointController.BreakpointDefinition breakpointDefinition)
		{
			BreakpointController.RemoveBreakpoint(breakpointDefinition);
			UpdateList();
		}

		/// <summary>
		/// Removes the breakpoint represented by the specified list view item.
		/// </summary>
		/// <param name="item">The item representing the breakpoint to remove.</param>
		private void RemoveItem(ListViewItem item)
		{
			BreakpointController.BreakpointDefinition bp;
			if (Breakpoints.TryGetValue(item, out bp))
			{
				RemoveBreakpoint(bp);
			}
		}

		/// <summary>
		/// Removes breakpoints represented by the selected list view items.
		/// </summary>
		private void RemoveSelected()
		{
			if (listView.SelectedItems.Count > 0)
			{
				var array = new ListViewItem[listView.SelectedItems.Count];
				listView.SelectedItems.CopyTo(array, 0);

				foreach (var item in array)
				{
					RemoveItem(item);
				}
			}
		}

		/// <summary>
		/// Selects list view items that match the specified list of breakpoints.
		/// </summary>
		/// <param name="breakpoints">The list of breakpoints to select.</param>
		public void HighlightBreakpoints(IEnumerable<MessyLab.Debugger.Breakpoint> breakpoints)
		{
			listView.SelectedItems.Clear();

			foreach (var dbp in breakpoints)
			{
				foreach (var bp in ListViewItems.Keys)
				{
					if (bp.Breakpoint == dbp) ListViewItems[bp].Selected = true;
				}
			}
		}

		private void deleteToolStripButton_Click(object sender, EventArgs e)
		{
			RemoveSelected();
		}

		private void listView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete) RemoveSelected();
		}
	}

	/// <summary>
	/// New Breakpoint Dialog interface.
	/// </summary>
	public interface INewBreakpointDialog
	{
		/// <summary>
		/// Shows the dialog.
		/// </summary>
		/// <param name="breakpoint">The new breakpoint definition as specified by the user.</param>
		/// <param name="controller">The parent breakpoint controller.</param>
		/// <returns>A value indicating success.</returns>
		bool ShowNewBreakpointDialog(out BreakpointController.BreakpointDefinition breakpoint, BreakpointController controller);
	}
}
