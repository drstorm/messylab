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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MessyLab.Platforms;

namespace MessyLab
{
	/// <summary>
	/// Watch management pad.
	/// </summary>
	public partial class WatchPad : Pad
	{
		/// <summary>
		/// Represents a watch item, i.e. the model item.
		/// </summary>
		public class WatchItem
		{
			public WatchItem(ListViewItem listViewItem)
			{
				if (listViewItem == null) throw new ArgumentNullException("listViewItem");
				Item = listViewItem;
			}

			public string Name
			{
				get { try { return Item.Text; } catch { return string.Empty; } }
				set { try { Item.Text = value; } catch { } }
			}
			
			public string Type
			{
				get { try { return Item.SubItems[2].Text; } catch { return string.Empty; } }
				set { try { Item.SubItems[2].Text = value; } catch { } }
			}

			public string Value
			{
				get { try { return Item.SubItems[1].Text; } catch { return string.Empty; } }
				set { try { Item.SubItems[1].Text = value; } catch { } }
			}

			private ListViewItem _item;
			public ListViewItem Item {
				get { return _item; }
				set
				{
					_item = value;
					_item.Tag = this;
				}
			}
		}

		/// <summary>
		/// Memento object that represents the watch list.
		/// </summary>
		[Serializable]
		public class WatchMemento
		{
			public struct Watch
			{
				public string Name;
				public string Type;
			}

			public Watch[] WatchItems = new Watch[0];
		}

		public WatchPad(Project project)
			: base(project)
		{
			InitializeComponent();
			CreateMenuItem("&Watch", MessyLab.Properties.Resources.Watch);
			CreateToolbarItem("Watch", MessyLab.Properties.Resources.Watch);
			ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;

			WatchItems = new List<WatchItem>();
		}
		public WatchPad() : this(null) { }

		public List<WatchItem> WatchItems { get; protected set; }

		/// <summary>
		/// Clears the list.
		/// </summary>
		public void Clear()
		{
			WatchItems.Clear();
			listView.Items.Clear();
		}

		/// <summary>
		/// Creates the memento object representing the list.
		/// </summary>
		/// <returns></returns>
		public WatchMemento CreateMemento()
		{
			var l = new List<WatchMemento.Watch>();
			foreach (var item in WatchItems)
			{
				l.Add(new WatchMemento.Watch { Name = item.Name, Type = item.Type });
			}
			return new WatchMemento { WatchItems = l.ToArray() };
		}

		/// <summary>
		/// Sets the watch list stored in the specified memento object.
		/// </summary>
		/// <param name="memento">The memento object to set.</param>
		public void SetMemento(WatchMemento memento)
		{
			Clear();
			foreach (var item in memento.WatchItems)
			{
				AddNewWatch(item.Name, item.Type);
			}
		}

		/// <summary>
		/// A list of valid data type names.
		/// </summary>
		public string[] Types { get; set; }

		/// <summary>
		/// Occurs when a watch item is added.
		/// </summary>
		public event Action<WatchItem> WatchAdded;
		protected void OnWatchAdded(WatchItem item) { if (WatchAdded != null) WatchAdded(item); }

		/// <summary>
		/// Creates and adds a new watch item using the specified name and type.
		/// </summary>
		/// <param name="name">The watch item name.</param>
		/// <param name="type">The type name.</param>
		public void AddNewWatch(string name, string type)
		{
			var li = new ListViewItem(name);
			li.SubItems.Add("?");
			li.SubItems.Add(type);
			var watchItem = new WatchItem(li);
			WatchItems.Add(watchItem);
			listView.Items.Add(li);

			OnWatchAdded(watchItem);
		}

		/// <summary>
		/// Displays the New Watch dialog and adds a new watch using the user
		/// specified data.
		/// </summary>
		public void AddNewWatch()
		{
			using (var f = new NewWatchForm())
			{
				f.SetTypes(Types);
				if (f.ShowDialog() == DialogResult.OK)
				{
					AddNewWatch(f.WatchName, f.WatchType);
				}
			}
		}

		private void addToolStripButton_Click(object sender, EventArgs e)
		{
			AddNewWatch();
		}

		/// <summary>
		/// Deletes selected watch items.
		/// </summary>
		protected void DeleteSelected()
		{
			if (listView.SelectedItems.Count > 0)
			{
				var toDel = new List<WatchItem>();

				// Find all WatchItems to delete
				foreach (ListViewItem item in listView.SelectedItems)
				{
					var w = item.Tag as WatchItem;
					if (w != null) toDel.Add(w);
				}

				// Delete found items.
				foreach (var item in toDel)
				{
					WatchItems.Remove(item);
					listView.Items.Remove(item.Item);
				}
			}
		}

		private void deleteToolStripButton_Click(object sender, EventArgs e)
		{
			DeleteSelected();
		}

		private void listView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete) DeleteSelected();
			if (e.KeyCode == Keys.Enter) EditValueOfSelected();
			if (e.KeyCode == Keys.Insert) AddNewWatch();
		}

		/// <summary>
		/// Occurs when a watch value is edited.
		/// </summary>
		public event Action<WatchItem, long> WatchValueEdited;
		protected void OnWatchValueEdited(WatchItem item, long value)
		{ if (WatchValueEdited != null)WatchValueEdited(item, value); }

		/// <summary>
		/// A value indicating whether it is possible to edit watch values.
		/// </summary>
		public bool CanEdit { get; set; }

		/// <summary>
		/// Edits the value of the selected watch item.
		/// </summary>
		protected void EditValueOfSelected()
		{
			if (listView.SelectedItems.Count != 1) return;
			ListViewItem l = listView.SelectedItems[0];
			var w = l.Tag as WatchItem;
			
			if (w == null || !CanEdit) return;

			using (var f = new IntegerInputForm("Edit value", "Enter new value for " + w.Name + ":"))
			{
				long val;
				if (long.TryParse(w.Value, out val))
				{
					f.LongValue = val;
				}

				if (f.ShowDialog() == DialogResult.OK)
				{
					OnWatchValueEdited(w, f.LongValue);
				}
			}
		}

		private void listView_DoubleClick(object sender, EventArgs e)
		{
			EditValueOfSelected();
		}

	}
}
