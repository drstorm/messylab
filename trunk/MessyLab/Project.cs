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
using System.IO;
using System.Xml.Serialization;
using MessyLab.Platforms;

namespace MessyLab
{
	/// <summary>
	/// Represents a Messy Lab project.
	/// </summary>
	public class Project
	{
		public Project()
		{
			Items = new List<ProjectItem>();
		}

		/// <summary>
		/// The name of the project file.
		/// </summary>
		/// <example>UberCode.mlp</example>
		public string Filename
		{
			get
			{
				string res = string.Empty;
				try
				{ res = System.IO.Path.GetFileName(Path); }
				catch { }
				return res;
			}
		}

		/// <summary>
		/// Full directory name of the project.
		/// </summary>
		/// <example>
		/// c:\Users\JSmith\Ducuments\Messy Lab Projects\UberCode
		/// </example>
		public string Directory
		{
			get
			{
				string res = string.Empty;
				try
				{ res = System.IO.Path.GetDirectoryName(Path); }
				catch { }
				return res;
			}
		}

		/// <summary>
		/// Full path to the project file.
		/// </summary>
		/// <example>
		/// c:\Users\JSmith\Ducuments\Messy Lab Projects\UberCode\UberCode.mlp
		/// </example>
		public string Path { get; set; }

		/// <summary>
		/// A list of project items.
		/// </summary>
		public List<ProjectItem> Items { get; private set; }

		/// <summary>
		/// The main (start-up) project item.
		/// </summary>
		public ProjectItem MainItem { get; set; }

		/// <summary>
		/// Check whether the an item with the specified path
		/// already exists.
		/// </summary>
		/// <param name="relativePath">The relative path to check.</param>
		/// <returns>A value indicating whether the an item with the
		/// specified path already exists withing the project</returns>
		public bool ItemExists(string relativePath)
		{
			foreach (var i in Items)
			{
				if (Program.IsSamePath(i.RelativePath, relativePath))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Instance of the Project's Platform.
		/// </summary>
		public Platform Platform { get; set; }

		/// <summary>
		/// Factory object used to create the platform by name.
		/// </summary>
		public PlatformFactory PlatformFactory { get; set; }

		/// <summary>
		/// Sets the Platform by creating it for the specified name
		/// using the PlatformFactory.
		/// </summary>
		/// <param name="name">The platform name.</param>
		public void SetPlatformByName(string name)
		{
			PlatformFactory factory;
			if (PlatformFactory == null)
			{ factory = PlatformFactory.Default; }
			else
			{ factory = PlatformFactory; }

			Platform = factory.CreatePlatform(this, name);
		}

		/// <summary>
		/// Saves the project state to a memento.
		/// </summary>
		/// <returns>Memento containing the project state.</returns>
		public ProjectMemento CreateMemento()
		{
			var memento = new ProjectMemento();
			foreach (var item in Items)
			{
				memento.Items.Add(item.RelativePath);
			}
			
			if (MainItem != null)
			{ memento.MainItem = MainItem.RelativePath; }
			
			memento.Platform = Platform.Name;
			return memento;
		}

		/// <summary>
		/// Restores the project state from the specified memento.
		/// </summary>
		/// <param name="memento">The memento to restore.</param>
		public void SetMemento(ProjectMemento memento)
		{
			Items.Clear();
			MainItem = null;
			foreach (var i in memento.Items)
			{
				var item = new ProjectItem();
				item.Project = this;
				item.RelativePath = i;
				if (!File.Exists(item.Path))
				{
					File.WriteAllText(item.Path, string.Empty);
				}
				Items.Add(item);

				if (i == memento.MainItem)
				{ MainItem = item; }
			}

			SetPlatformByName(memento.Platform);
		}

		/// <summary>
		/// Save the project to file.
		/// </summary>
		/// <param name="newProject">A value indicating whether the project to save is newly created.</param>
		public void Save(bool newProject)
		{
			foreach (var item in Items)
			{
				item.Save();
			}
			var memento = CreateMemento();
			
			XmlSerializer s = new XmlSerializer(memento.GetType());
			TextWriter w = new StreamWriter(Path);
			s.Serialize(w, memento);
			w.Close();

			// Only save settings for the open projects.
			if (!newProject)
			{
				Platform.SaveSettings();
			}

			OnSaved();
		}

		/// <summary>
		/// Saves the project to file.
		/// </summary>
		public void Save()
		{
			Save(false);
		}

		/// <summary>
		/// Opens the project form file.
		/// </summary>
		/// <param name="path">The path to the project file.</param>
		public void Open(string path)
		{
			XmlSerializer s = new XmlSerializer(typeof(ProjectMemento));
			TextReader r = new StreamReader(path);
			var memento = s.Deserialize(r) as ProjectMemento;
			r.Close();
			Path = path;
			SetMemento(memento);
			Platform.LoadSettings();
		}

		/// <summary>
		/// Indicates whether the any of the items has been modified.
		/// </summary>
		public bool IsModified
		{
			get
			{
				foreach (var item in Items)
				{
					var editor = Platform.Editors.GetEditorFormIfExists(item);
					if (editor != null && editor.IsModified) return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Reverts changes made to the items.
		/// </summary>
		public void Revert()
		{
			foreach (var item in Items)
			{
				var editor = Platform.Editors.GetEditorFormIfExists(item);
				if (editor != null) editor.Revert();
			}
		}

		/// <summary>
		/// Occurs when the project is opened.
		/// </summary>
		public event Action Saved;
		protected void OnSaved() { if (Saved != null) Saved(); }
	}
}
