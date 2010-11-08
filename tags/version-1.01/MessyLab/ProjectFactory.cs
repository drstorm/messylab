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

namespace MessyLab
{
	/// <summary>
	/// Provides a project creation capability.
	/// </summary>
	public class ProjectFactory
	{
		/// <summary>
		/// Creates a picoComputer project at the specified path.
		/// </summary>
		/// <param name="path">The path to the project file.</param>
		/// <returns>The created Project.</returns>
		protected Project CreatePicoProject(string path)
		{
			path = Path.GetFullPath(path);
			string dir = Path.GetDirectoryName(path);
			
			if (!Directory.Exists(dir))
			{ Directory.CreateDirectory(dir); }
			
			if (Path.GetExtension(path).ToLower() != ".mlp")
			{ path += ".mlp"; }

			Project p = new Project();
			p.SetPlatformByName("picoComputer");
			p.Path = path;

			// Create and add the Main item.
			var mainItem = p.Platform.ProjectItemFactory.CreateProjectItem(p, "Main");
			p.Items.Add(mainItem);
			p.MainItem = mainItem;

			p.Save(true);

			return p;
		}

		/// <summary>
		/// Creates a project using the specified parameters.
		/// </summary>
		/// <param name="platform">The platform name for the project.</param>
		/// <param name="path">The path to the project file.</param>
		/// <returns>The created Project.</returns>
		public virtual Project CreateProject(string platform, string path)
		{
			switch (platform.ToLower())
			{
				case "picocomputer":
					return CreatePicoProject(path);
				default:
					throw new ArgumentException("Platform \"" + platform + "\" is not supported.", "platform");
			}
		}

		private static ProjectFactory _default = new ProjectFactory();
		/// <summary>
		/// Default Factory.
		/// </summary>
		public static ProjectFactory Default { get { return _default; } }
	}
}
