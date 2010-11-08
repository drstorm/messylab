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
	/// Contains the data of a Project.
	/// </summary>
	[Serializable]
	public class ProjectMemento
	{
		private List<string> _items = new List<string>();

		/// <summary>
		/// A list of relative paths to the project items.
		/// </summary>
		public List<string> Items { get { return _items; } }

		/// <summary>
		/// The relative path to the main item.
		/// </summary>
		public string MainItem { get; set; }

		/// <summary>
		/// Name of the platform.
		/// </summary>
		public string Platform { get; set; }
	}
}
