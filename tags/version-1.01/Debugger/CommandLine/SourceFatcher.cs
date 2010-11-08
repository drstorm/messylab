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

namespace MessyLab.Debugger.CommandLine
{
	/// <summary>
	/// Provides Source Code reading capabilities.
	/// </summary>
	public class SourceFatcher
	{
		public SourceFatcher()
		{
			Files = new Dictionary<string, string[]>();
		}

		/// <summary>
		/// Cache of loaded source files.
		/// </summary>
		public Dictionary<string, string[]> Files { get; protected set; }

		/// <summary>
		/// Gets a source code line for the specified location.
		/// </summary>
		/// <remarks>
		/// Uses information from <c>Files</c> if available.
		/// Otherwise, loads the file.
		/// </remarks>
		/// <param name="location">Location in the source code.</param>
		/// <returns>Requested source code line of an N/A message.</returns>
		public string GetLine(DebugSymbolLocation location)
		{
			string[] file;
			if (!Files.TryGetValue(location.File, out file))
			{
				try
				{
					file = File.ReadAllLines(location.File);
					Files.Add(location.File, file);
				}
				catch
				{
					return "Source code is not available.";
				}
			}
			try
			{
				string line = file[location.Line - 1];
				return line;
			}
			catch
			{
				return "Source file does not contain the specified line.";
			}
		}
	}
}
