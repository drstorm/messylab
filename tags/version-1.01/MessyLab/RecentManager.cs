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
using MessyLab.Properties;
using MessyLab.Platforms;
using System.IO;
using System.Xml.Serialization;

namespace MessyLab
{
	/// <summary>
	/// Keeps tabs on recently open projects.
	/// </summary>
	public class RecentManager
	{
		[Serializable]
		public class RecentList : List<string> { }

		/// <summary>
		/// The maximum count of recent items.
		/// </summary>
		protected static readonly int Size = 6;

		/// <summary>
		/// A list of paths to the recent projects.
		/// </summary>
		protected RecentList Items { get; set; }

		public RecentManager()
		{
			Items = new RecentList();
			Load();
		}

		/// <summary>
		/// Notifies the manager that the specified object has been opened.
		/// </summary>
		/// <param name="path">The path to the opened project.</param>
		public void Notify(string path)
		{
			// If the path is already in the list, remove it.
			for (int i = Items.Count - 1; i >= 0; i--)
			{
				if (Program.IsSamePath(Items[i], path))
				{
					Items.RemoveAt(i);
				}
			}

			// Insert the path at the top.
			Items.Insert(0, path);

			// Ensure that the list is not longer than Size.
			while (Items.Count > Size)
			{
				Items.RemoveAt(Items.Count - 1);
			}

			Save();
		}

		/// <summary>
		/// An array representing the recently opened items.
		/// </summary>
		public string[] Recent
		{
			get
			{
				Update();
				return Items.ToArray();
			}
		}

		/// <summary>
		/// Clears the list.
		/// </summary>
		public void Clear()
		{
			Items.Clear();
			Save();
		}

		/// <summary>
		/// Updates the list by loading it from file if it is empty and
		/// removing the non-existing items.
		/// </summary>
		protected void Update()
		{
			if (Items.Count == 0)
			{
				Load();
			}
			bool save = false;
			for (int i = Items.Count - 1; i >= 0; i--)
			{
				if (!File.Exists(Items[i]))
				{
					Items.RemoveAt(i);
					save = true;
				}
			}
			if (save) Save();
		}

		/// <summary>
		/// Loads the list from file.
		/// </summary>
		protected void Load()
		{
			string path = Path.Combine(Platform.GetSettingsPath(), "Recent.xml");
			if (File.Exists(path))
			{
				XmlSerializer s = new XmlSerializer(typeof(RecentList));
				TextReader r = new StreamReader(path);
				var list = s.Deserialize(r) as RecentList;
				r.Close();
				Items = list;
			}
		}

		/// <summary>
		/// Saves the list to file.
		/// </summary>
		protected void Save()
		{
			string path = Path.Combine(Platform.GetSettingsPath(), "Recent.xml");
			try
			{
				XmlSerializer s = new XmlSerializer(Items.GetType());
				TextWriter w = new StreamWriter(path);
				s.Serialize(w, Items);
				w.Close();
			}
			catch { }
		}
	}
}
