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

namespace MessyLab.Debugger.Target
{
	/// <summary>
	/// Provides access to the target's registers.
	/// </summary>
	public interface IRegisters
	{
		/// <summary>
		/// Provides access to registers by name.
		/// </summary>
		/// <param name="name">Register name</param>
		/// <returns>Contents of the specified register.</returns>
		long this[string name] { get; set; }

		/// <summary>
		/// Platform specific list of register names.
		/// </summary>
		string[] Names { get; }
	}
}
