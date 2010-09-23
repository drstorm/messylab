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
	/// Provides access to the picoComputer Memory.
	/// </summary>
	public class PicoMemory : IMemory
	{
		/// <summary>
		/// Creates object using the specified PicoTarget.
		/// </summary>
		/// <remarks>
		/// The memory is accessed through the Targets' VirualMachine
		/// property.
		/// </remarks>
		/// <param name="target">PicoTarget object</param>
		public PicoMemory(PicoTarget target)
		{
			Target = target;
		}

		/// <summary>
		/// Debugging target used to get the reference to the VM.
		/// </summary>
		public PicoTarget Target { get; set; }

		public long this[long address]
		{
			get
			{
				return Target.VirtualMachine.Data.DirectMemoryRead((ushort)address);
			}
			set
			{
				Target.VirtualMachine.Data.DirectMemoryWrite((ushort)address, (ushort)value);
			}
		}
	}
}
