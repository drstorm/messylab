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
	public partial class StartForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public StartForm()
		{
			InitializeComponent();
		}

		public MainForm MainForm { get; set; }

		private void newProjectLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MainForm.NewProjectClick();
		}

		private void openProjectLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MainForm.OpenProjectClick();
		}

		private void optionsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MainForm.OptionsClick();
		}

		private void StartForm_Shown(object sender, EventArgs e)
		{
			string[] recent = MainForm.RecentManager.Recent;
			var l = new List<LinkLabel>
			{
				recentLinkLabel1,
				recentLinkLabel2,
				recentLinkLabel3,
				recentLinkLabel4,
				recentLinkLabel5,
				recentLinkLabel6
			};
			for (int i = 0; i < l.Count; i++)
			{
				if (i < recent.Length)
				{
					l[i].Text = System.IO.Path.GetFileNameWithoutExtension(recent[i]);
					l[i].Tag = recent[i];
				}
				else
				{
					l[i].Text = string.Empty;
					l[i].Tag = null;
				}
			}
		}

		private void recentLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var l = sender as LinkLabel;
			string s = l.Tag as string;
			if (!string.IsNullOrEmpty(s))
			{
				MainForm.OpenProject(s);
			}
		}
	}
}
