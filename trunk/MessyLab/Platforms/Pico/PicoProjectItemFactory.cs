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

namespace MessyLab.Platforms.Pico
{
	/// <summary>
	/// Provides a picoComputer project item creation capability.
	/// </summary>
	public class PicoProjectItemFactory : ProjectItemAbstractFactory
	{
		/// <summary>
		/// Creates a picoComputer project item for specified parameters.
		/// </summary>
		/// <param name="project">The project to create item for.</param>
		/// <param name="relativePath">The relative path to the item file.</param>
		/// <returns>The created ProjectItem.</returns>
		public override ProjectItem CreateProjectItem(Project project, string filename)
		{
			ProjectItem item = new ProjectItem();
			item.Project = project;
			item.RelativePath = Path.GetFileNameWithoutExtension(filename) + ".pca";
			if (!File.Exists(item.Path))
			{
				File.Copy(Path.Combine(Platform.GetPlatformsDirectory("Pico"), "Empty.pca"), item.Path);
			}
			return item;
		}
	}
}
