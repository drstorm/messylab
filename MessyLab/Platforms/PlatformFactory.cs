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
using MessyLab.Platforms.Pico;

namespace MessyLab.Platforms
{
	/// <summary>
	/// Provides a Platform creation capability.
	/// </summary>
	public class PlatformFactory
	{
		/// <summary>
		/// Creates the platform for the specified project using the specified
		/// platform name.
		/// </summary>
		/// <param name="project">The project to create a platform for.</param>
		/// <param name="platformName">The name of the platform to create.</param>
		/// <returns></returns>
		public Platform CreatePlatform(Project project, string platformName)
		{
			Platform pl;
			switch (platformName.ToLower())
			{
				case "picocomputer":
					pl = new PicoPlatform(project, Program.MainForm);
					break;
				default:
					throw new ArgumentException("Platform \"" + platformName + "\" is not supported.", "platformName");
			}
			pl.Initialize();
			return pl;
		}

		private static PlatformFactory _default = new PlatformFactory();
		/// <summary>
		/// Default Factory.
		/// </summary>
		public static PlatformFactory Default { get { return _default; } }
	}
}
