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

namespace MessyLab
{
	/// <summary>
	/// The About Box.
	/// </summary>
	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			InitializeComponent();
			InitializeVersions();
		}

		/// <summary>
		/// Adds component versions to the list box.
		/// </summary>
		protected void InitializeVersions()
		{
			// TODO: Update when outdated or rewrite to dynamically load the data.
			listBox.Items.Clear();
			listBox.Items.Add("Messy Lab IDE 1.00");
			listBox.Items.Add("ML Debugger 1.00");
			listBox.Items.Add("PicoAsm 1.00");
			listBox.Items.Add("PicoVM 1.00");
		}

		private void homeLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.messylab.com");
		}

		private void infoLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show(
				"This software uses the following third party components:\n\n" +
				"Grammatica Parser Generator licensed under the GNU LGPL\n" +
				"by Per Cederberg, Marielle Fois and Adrian Moore.\n\n" +
				"DockPanel Suite for .Net 2.0 licensed under the MIT License\n" +
				"by Weifen Luo.\n\n" +
				"ICSharpCode.TextEditor licensed under the GNU LGPL as a\n" +
				"component of SharpDevelop by IC#Code.\n\n\n" +
				"picoComputer architecture was design by Jozo Dujmović © 1989\n\n\n" +
				"Messy Lab was originally developed as a part of Miloš Anđelković's\n" +
				"Master's thesis at the Department of Software Engineering, School\n" +
				"of Electrical Engineering, University of Belgrade.\n\n" +
				"The name \"Messy Lab\" comes from the fact that assembly programs\n" +
				"tend to get, well, messy. It is also an anagram of the word \"assembly\"."
				,
				"Additional Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void licenseLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.apache.org/licenses/LICENSE-2.0");
		}
	}
}
