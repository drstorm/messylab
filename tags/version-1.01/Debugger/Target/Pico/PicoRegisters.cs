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

namespace MessyLab.Debugger.Target.Pico
{
	/// <summary>
	/// Provides access to the picoComputer Registers.
	/// </summary>
	public class PicoRegisters : IRegisters
	{
		/// <summary>
		/// Creates object using the specified PicoTarget.
		/// </summary>
		/// <remarks>
		/// The registers are accessed through the Targets' VirualMachine
		/// property.
		/// </remarks>
		/// <param name="target">PicoTarget object</param>
		public PicoRegisters(PicoTarget target)
		{
			Target = target;
		}

		/// <summary>
		/// Debugging target used to get the reference to the VM.
		/// </summary>
		public PicoTarget Target { get; set; }

		/// <summary>
		/// Provides access to registers by name.
		/// </summary>
		/// <remarks>
		/// Valid names are "PC" and "SP". Names are case-insensitive.
		/// If an invalid name is provided, the returned value is 0.
		/// </remarks>
		/// <param name="name">Register name</param>
		/// <returns>Contents of the specified register.</returns>
		public long this[string name]
		{
			get
			{
				switch (name.ToLower())
				{
					case "pc": return Target.VirtualMachine.Data.DirectPC;
					case "sp": return Target.VirtualMachine.Data.DirectSP;
					default: return 0;
				}
			}
			set
			{
				switch (name.ToLower())
				{
					case "pc": Target.VirtualMachine.Data.DirectPC = (ushort)value; break;
					case "sp": Target.VirtualMachine.Data.DirectSP = (ushort)value; break;
				}
			}
		}

		/// <summary>
		/// List of picoComputer Register Names (PC and SP).
		/// </summary>
		public string[] Names
		{
			get { return new string[] { "PC", "SP" }; }
		}
	}
}
