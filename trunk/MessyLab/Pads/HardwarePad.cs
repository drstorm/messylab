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
using System.Linq;
using System.Windows.Forms;

namespace MessyLab
{
	/// <summary>
	/// Pad for displaying the hardware state.
	/// </summary>
	public partial class HardwarePad : MessyLab.Pad
	{
		/// <summary>
		/// Represents the content and manages a list view control.
		/// </summary>
		public class HardwareContent
		{
			/// <summary>
			/// Represents a single item group, e.g. Registers.
			/// </summary>
			public class Group
			{
				public ListView ListView { get; set; }
				public Group(ListView listView)
				{
					ListView = listView;
					Values = new Dictionary<string, string>();
				}

				/// <summary>
				/// Gets the corresponding list view group.
				/// </summary>
				/// <param name="group">The list view group.</param>
				/// <returns>A value indicating success.</returns>
				private bool GetGroup(out ListViewGroup group)
				{
					bool exists = false;
					group = null;
					foreach (ListViewGroup g in ListView.Groups)
					{
						if (!string.IsNullOrEmpty(_name) && g.Header == _name)
						{
							exists = true;
							group = g;
							break;
						}
					}
					return exists;
				}

				private string _name;
				/// <summary>
				/// Gets or sets the group name.
				/// </summary>
				public string Name
				{
					get { return _name; }
					set
					{
						ListViewGroup g;
						if (GetGroup(out g))
						{
							g.Header = value;
						}
						else
						{
							ListView.Groups.Add(new ListViewGroup(value));
						}
						_name = value;
					}
				}

				/// <summary>
				/// Group items.
				/// </summary>
				public Dictionary<string, string> Values { get; protected set; }
				
				/// <summary>
				/// Gets or sets group item values.
				/// </summary>
				/// <param name="index">Item name.</param>
				/// <returns>The value of the item.</returns>
				public string this[string index]
				{
					get
					{
						string v = string.Empty;
						Values.TryGetValue(index, out v);
						return v;
					}
					set
					{
						if (Values.ContainsKey(index))
						{
							var items = from ListViewItem item in ListView.Items
										where item.Text == index && item.Group != null && item.Group.Header == Name
										select item;
							foreach (var item in items)
							{
								item.SubItems[1].Text = value;
							}
						}
						else
						{
							var item = new ListViewItem(index);
							item.SubItems.Add(value);
							ListViewGroup g;
							if (GetGroup(out g))
							{
								item.Group = g;
							}
							ListView.Items.Add(item);
						}
						Values[index] = value;
					}
				}
			}

			public ListView ListView { get; set; }
			public HardwareContent(ListView listView)
			{
				ListView = listView;
				Groups = new Dictionary<string, Group>();
			}

			/// <summary>
			/// A list of groups by name.
			/// </summary>
			public Dictionary<string, Group> Groups { get; protected set; }

			/// <summary>
			/// Gets or sets groups.
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			public Group this[string index]
			{
				get
				{
					Group s;
					if (!Groups.TryGetValue(index, out s))
					{
						s = new Group(ListView) { Name = index };
						Groups.Add(index, s);
					}
					return s;
				}
				set
				{
					Groups[index] = value;
				}
			}
		}

		public HardwarePad(Project project)
			: base(project)
		{
			InitializeComponent();
			CreateMenuItem("&Hardware", MessyLab.Properties.Resources.Hardware);
			CreateToolbarItem("Hardware", MessyLab.Properties.Resources.Hardware);
			ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;

			Content = new HardwareContent(listView);
		}
		public HardwarePad() : this(null) { }

		public HardwareContent Content { get; protected set; }
	}
}
