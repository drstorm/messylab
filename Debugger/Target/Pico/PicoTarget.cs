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
using MessyLab.PicoComputer;

namespace MessyLab.Debugger.Target.Pico
{
	/// <summary>
	/// Implements the ITarget for the picoComputer.
	/// </summary>
	public class PicoTarget : ITarget
	{
		/// <summary>
		/// Initializes the object with a new Virtual Machine
		/// using the Console I/O device and 10000 History.
		/// </summary>
		public PicoTarget() : this(new VirtualMachine(10000)) { }

		/// <summary>
		/// Initializes the object with the specified Virtual Machine.
		/// </summary>
		/// <param name="virtualMachine">A picoComputer Virtual Machine</param>
		public PicoTarget(VirtualMachine virtualMachine)
		{
			VirtualMachine = virtualMachine;
			Memory = new PicoMemory(this);
			Registers = new PicoRegisters(this);
			BreakpointChecker = new PicoBreakpointChecker(this);
		}

		/// <summary>
		/// The currently used picoComputer Virual Machine.
		/// </summary>
		public VirtualMachine VirtualMachine { get; set; }

		/// <summary>
		/// Loads a .BIN or .HEX file into the VM.
		/// </summary>
		/// <param name="path">The full path to the program binary</param>
		public void Load(string path)
		{
			try
			{
				VirtualMachine.LoadFromHexFile(path);
			}
			catch (Exception)
			{
				VirtualMachine.LoadFromBinaryFile(path);
			}
		}

		/// <summary>
		/// Executes a single instruction.
		/// </summary>
		public void Step()
		{
			try
			{
				VirtualMachine.Processor.Step();
				VirtualMachine.Data.Commit();
			}
			catch (RuntimeException)
			{
				VirtualMachine.Data.Rollback();
				throw;
			}
		}

		/// <summary>
		/// Steps back a single instruction. Throws a HistoryEmptyRuntimeException
		/// if the history of the VM is empty.
		/// </summary>
		public void StepBack()
		{
			if (!VirtualMachine.Data.Uncommit())
			{
				throw new HistoryEmptyRuntimeException(new ushort[] { 0 }, VirtualMachine.Data.DirectPC);
			}
			DataDelta uncommited = VirtualMachine.Data.Delta;
			VirtualMachine.Data.Rollback();

			// The breakpoint checker executes an instruction in advance to determine
			// whether it will trigger any breakpoints.
			// Since we are undoing an instruction, we already know its effect,
			// so we are informing the checker here.
			BreakpointChecker.SetNextDelta(uncommited, VirtualMachine.Data.DirectPC);
		}

		#region Memory Access
		public IMemory Memory { get; protected set; }

		public IRegisters Registers { get; protected set; }

		private PicoProgramLocation _programLocation = new PicoProgramLocation();
		public IProgramLocation ProgramLocation
		{
			get
			{
				_programLocation.PC = VirtualMachine.Data.DirectPC;
				_programLocation.CurrentInstructionAddress = _programLocation.PC;
				_programLocation.SP = VirtualMachine.Data.DirectSP;
				_programLocation.Head = VirtualMachine.Data.DirectMemoryRead((ushort)(_programLocation.SP + 1));
				return _programLocation;
			}
		}
		#endregion

		#region Breakpoint Checkers
		/// <summary>
		/// PicoBreakpointChecker implements all Breakpoint Checker interfaces.
		/// </summary>
		public PicoBreakpointChecker BreakpointChecker { get; protected set; }

		public IMemoryBreakpointChecker MemoryBreakpointChecker
		{
			get { return BreakpointChecker; }
		}

		public IRegisterBreakpointChecker RegisterBreakpointChecker
		{
			get { return BreakpointChecker; }
		}

		public IStepBreakpointChecker StepBreakpointChecker
		{
			get { return BreakpointChecker; }
		}

		public IIOBreakpointChecker IOBreakpointChecker
		{
			get { return BreakpointChecker; }
		}

		public void SetBreakpointChecker(Breakpoint breakpoint)
		{
			breakpoint.TrySetChecker(BreakpointChecker);
		}
		#endregion
	}

	/// <summary>
	/// Holds the execution state of the picoComputer.
	/// </summary>
	public class PicoProgramLocation : IProgramLocation
	{
		/// <summary>
		/// Program Counter Register
		/// </summary>
		public ushort PC { get; set; }

		/// <summary>
		/// Stack Pointer Register
		/// </summary>
		public ushort SP { get; set; }

		/// <summary>
		/// Stack Head value;
		/// </summary>
		public ushort Head { get; set; }

		/// <summary>
		/// CurrentInstructionAddress has the same value as PC.
		/// </summary>
		public long CurrentInstructionAddress { get; set; }
	}
}
