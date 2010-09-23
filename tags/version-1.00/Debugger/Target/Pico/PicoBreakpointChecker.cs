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
	/// Implements all Breakpoint Checker interfaces for the picoComputer.
	/// </summary>
	public class PicoBreakpointChecker : IMemoryBreakpointChecker, IRegisterBreakpointChecker,
		IStepBreakpointChecker, IIOBreakpointChecker
	{
		/// <summary>
		/// Creates object using the specified PicoTarget.
		/// </summary>
		/// <param name="target">PicoTarget object</param>
		public PicoBreakpointChecker(PicoTarget target)
		{
			Target = target;
		}

		/// <summary>
		/// Debugging target used to get the reference to the VM,
		/// more specifically its Data object.
		/// </summary>
		public PicoTarget Target { get; set; }

		/// <summary>
		/// NullDevice is used to probe I/O instructions.
		/// </summary>
		private NullIODevice _nullDevice = new NullIODevice();
		
		/// <summary>
		/// A picoComputer processor used for probing.
		/// </summary>
		/// <remarks>
		/// Probing is the process of executing and undoing the next instruction
		/// in order to collect data on its effects.
		/// The data is stored in a DataDelta object and it is used to check if
		/// specifics breakpoints are hit.
		/// </remarks>
		private Processor _processor;

		/// <summary>
		/// Calculates the effects of the next instruction.
		/// </summary>
		/// <remarks>
		/// This calculation in advance allows breakpoints to be hit BEFORE the
		/// actual execution.
		/// </remarks>
		/// <returns>A value holding the effects of the instruction.</returns>
		protected DataDelta CalculateNextDelta()
		{
			Data data = Target.VirtualMachine.Data;
			if (_processor == null || _processor.Data != data)
			{
				_processor = new Processor(data, _nullDevice);
			}
			
			data.Commit(); // Commits any previous changes

			try { _processor.Step(); } // Execute next instruction (breakpoint probe).
			catch (Exception) { }
			
			DataDelta result = data.Delta; // Delta object holds the changes made by the probe

			if (!data.Uncommit()) // Uncommit restores previous Delta object from history and discards the probe changes.
			{
				// If uncommit fails, we need to manually discard probe changes by calling Rollback().
				data.Rollback();
			}

			return result;
		}

		/// <summary>
		/// In order to avoid unnecessary recalculation of the next delta,
		/// a reference to the object is stored for further use.
		/// </summary>
		private DataDelta _nextDelta;

		/// <summary>
		/// The address of the instruction corresponding to the _nextDelta object.
		/// </summary>
		private ushort _nextDeltaPC;

		/// <summary>
		/// Return the Delta (effect) of the instruction that will be executed next.
		/// </summary>
		/// <remarks>
		/// The value is recalculated by calling CalculateNextDelta() if necessary;
		/// otherwise _nextDelta is returned.
		/// </remarks>
		public DataDelta NextDelta
		{
			get
			{
				ushort pc = Target.VirtualMachine.Data.DirectPC;
				if (_nextDelta == null || _nextDeltaPC != pc)
				{
					_nextDeltaPC = pc;
					_nextDelta = CalculateNextDelta();
				}
				return _nextDelta;
			}
		}

		/// <summary>
		/// Sets the NextDelta and the corresponding PC.
		/// </summary>
		/// <remarks>
		/// This method should be used when stepping back to avoid
		/// reexecuting the undone instruction in CalculateNextDelta.
		/// </remarks>
		/// <param name="nextDelta">The Delta of the next instruction</param>
		/// <param name="pc">The address of the instruction</param>
		public void SetNextDelta(DataDelta nextDelta, ushort pc)
		{
			_nextDelta = nextDelta;
			_nextDeltaPC = pc;
		}

		#region IMemoryBreakpointChecker Members

		public bool IsAboutToExecute(long address)
		{
			return Target.VirtualMachine.Data.DirectPC == address;
		}

		public bool IsAboutToRead(long address)
		{
			return NextDelta.ReadLocations.Contains((ushort)address);
		}

		public bool IsAboutToWrite(long address)
		{
			return NextDelta.WrittenLocations.ContainsKey((ushort)address);
		}

		public bool IsAboutToExecute(long address, int count)
		{
			if (count < 1) return false;
			if (count == 1) return IsAboutToExecute(address);

			long lastAddress = address + count - 1;

			ushort pc = Target.VirtualMachine.Data.DirectPC;
			return pc >= address && pc <= lastAddress;
		}

		public bool IsAboutToRead(long address, int count)
		{
			if (count < 1) return false;
			if (count == 1) return IsAboutToRead(address);

			long lastAddress = address + count - 1;

			foreach (ushort loc in NextDelta.ReadLocations)
			{
				if (loc >= address && loc <= lastAddress) return true;
			}
			return false;
		}

		public bool IsAboutToWrite(long address, int count)
		{
			if (count < 1) return false;
			if (count == 1) return IsAboutToWrite(address);

			long lastAddress = address + count - 1;

			foreach (ushort loc in NextDelta.WrittenLocations.Keys)
			{
				if (loc >= address && loc <= lastAddress) return true;
			}
			return false;
		}

		#endregion

		#region IRegisterBreakpointChecker Members

		/// <summary>
		/// Pico Computer VM does NOT support on read breakpoints from registers.
		/// </summary>
		/// <param name="register">Register name</param>
		/// <returns>Always return false.</returns>
		public bool IsAboutToRead(string register)
		{
			return false;
		}

		public bool IsAboutToWrite(string register)
		{
			switch (register.ToLower())
			{
				case "pc": return NextDelta.PCWritten;
				case "sp": return NextDelta.SPWritten;
				default: return false;
			}
		}

		#endregion

		#region IStepBreakpointChecker Members

		public bool IsStepCompleted(IProgramLocation originLocation, StepKind stepKind)
		{
			PicoProgramLocation loc = (PicoProgramLocation)originLocation;
			ushort pc = Target.VirtualMachine.Data.DirectPC;
			ushort sp = Target.VirtualMachine.Data.DirectSP;

			switch (stepKind)
			{
				case StepKind.Into:
					// If PC is changed, a step is performed.
					return pc != loc.PC;
				case StepKind.Over:
					// To complete a step over PC has to change, and the call stack has to remain the same.
					return pc != loc.PC && sp == loc.SP;
				case StepKind.Out:
					// To step out of a subroutine, SP needs to increment (Pop RetAddress).
					return sp > loc.SP;
			}
			return false; // Unreachable, but makes the compiler happy. :)
		}

		#endregion

		#region IIOBreakpointChecker Members

		/// <summary>
		/// Checks whether the I/O operation is about to commence.
		/// </summary>
		/// <remarks>
		/// Works by decoding the next instruction and checking whether it is IN or OUT.
		/// </remarks>
		/// <param name="input">Indicates if the operation is Input; otherwise Output.</param>
		/// <param name="address">Address is ignored, because picoComputer has only one I/O device.</param>
		/// <returns>A value indicating whether the specified I/O operation is about to commence.</returns>
		public bool IsAboutToPerformIO(bool input, long address)
		{
			ushort instruction = Target.VirtualMachine.Data.DirectMemoryRead(Target.VirtualMachine.Data.DirectPC);
			instruction >>= 12; // OP Code.
			ushort lookingFor = (ushort)(input ? 7 : 8); // In: 0111 (0x7), Out: 1000 (0x8)
			return instruction == lookingFor;
		}

		#endregion

	}
}
