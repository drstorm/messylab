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
using WeifenLuo.WinFormsUI.Docking;

namespace MessyLab
{
	/// <summary>
	/// A Pad is a dockable tool window.
	/// </summary>
	public partial class Pad : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public Pad()
		{
			InitializeComponent();
			DockState = DockState.Unknown;
		}

		public Pad(Project project) : this()
		{
			Project = project;
		}

		public Project Project { get; set; }

		/// <summary>
		/// Show the pad on the main form.
		/// </summary>
		public void ShowOnMainForm()
		{
			if (Project == null || Project.Platform == null || Project.Platform.Gui.MainForm == null) return;
			Project.Platform.Gui.MainForm.ShowPad(this);
		}

		/// <summary>
		/// Creates the view menu item.
		/// </summary>
		/// <param name="text">The item text.</param>
		/// <param name="icon">The item icon.</param>
		protected virtual void CreateMenuItem(string text, Bitmap icon)
		{
			MenuItem = new ToolStripMenuItem(text, icon);
			MenuItem.Click += (sender, e) => ShowOnMainForm();
		}

		/// <summary>
		/// Creates the view toolbar item.
		/// </summary>
		/// <param name="text">The item text.</param>
		/// <param name="icon">The item icon.</param>
		protected virtual void CreateToolbarItem(string text, Bitmap icon)
		{
			ToolbarItem = new ToolStripButton(text, icon);
			ToolbarItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			ToolbarItem.Click += (sender, e) => ShowOnMainForm();
		}

		/// <summary>
		/// The view menu item.
		/// </summary>
		public ToolStripMenuItem MenuItem { get; set; }
		/// <summary>
		/// The view toolbar item.
		/// </summary>
		public ToolStripItem ToolbarItem { get; set; }
	}
}
