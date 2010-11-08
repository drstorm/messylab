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
	/// ITarget represents a debugging target. Support for a specific architecture
	/// is provided by implementing this interface.
	/// </summary>
	public interface ITarget
	{
		/// <summary>
		/// Loads the specified program.
		/// </summary>
		/// <param name="path">The full path to the program binary</param>
		void Load(string path);

		/// <summary>
		/// Peforms a single step.
		/// </summary>
		/// <remarks>Typically, a step is a single instruction.</remarks>
		void Step();

		/// <summary>
		/// Performs a single step backwards if supported.
		/// </summary>
		void StepBack();

		#region Memory access
		/// <summary>
		/// Provides access to the memory of the target.
		/// </summary>
		IMemory Memory { get; }

		/// <summary>
		/// Provides access to the registers of the target.
		/// </summary>
		IRegisters Registers { get; }

		/// <summary>
		/// Provides access to the current location.
		/// </summary>
		IProgramLocation ProgramLocation { get; }
		#endregion

		#region Breakpoint Checkers
		IMemoryBreakpointChecker MemoryBreakpointChecker { get; }
		IRegisterBreakpointChecker RegisterBreakpointChecker { get; }
		IStepBreakpointChecker StepBreakpointChecker { get; }
		IIOBreakpointChecker IOBreakpointChecker { get; }

		/// <summary>
		/// Sets the appropriate Breakpoint checker in the specified breakpoint.
		/// </summary>
		/// <param name="breakpoint">Breakpoint to set the checker.</param>
		void SetBreakpointChecker(Breakpoint breakpoint);
		#endregion
	}

	/// <summary>
	/// Platform specific program location which identifies the state of execution.
	/// Example data include a call stack state and PC register.
	/// </summary>
	public interface IProgramLocation
	{
		long CurrentInstructionAddress { get; set; }
	}
}
