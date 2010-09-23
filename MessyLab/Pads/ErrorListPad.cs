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
	/// Error list pad.
	/// </summary>
	public partial class ErrorListPad : ListPad
	{
		public ErrorListPad(Project project)
			: base(project)
		{
			InitializeComponent();
			listView.SmallImageList = imageList;
			CreateMenuItem("&Error List", MessyLab.Properties.Resources.ErrorList);
			CreateToolbarItem("Error List", MessyLab.Properties.Resources.ErrorList);
		}

		public ErrorListPad() : this(null) { }

		/// <summary>
		/// Same as items. Defined for convenience.
		/// </summary>
		public IEnumerable<ListItem> Errors { get { return Items; } set { Items = value; } }
	}
}
