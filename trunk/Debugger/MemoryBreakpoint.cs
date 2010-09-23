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
using MessyLab.Debugger.Target;

namespace MessyLab.Debugger
{
	/// <summary>
	/// Breakpoint for Memory operations.
	/// </summary>
	public class MemoryBreakpoint : Breakpoint
	{
		/// <summary>
		/// Memory address.
		/// </summary>
		public long Address { get; set; }

		/// <summary>
		/// A variable backing the <c>Count</c> Property.
		/// </summary>
		private int _count = 1;

		/// <summary>
		/// Count of successive memory locations starting from the one at <c>Address</c>
		/// </summary>
		public int Count
		{
			get { return _count; }
			set { if (value > 0) _count = value; }
		}

		/// <summary>
		/// Creates a Memory breakpoint for the specified address.
		/// </summary>
		/// <remarks>
		/// Created breakpoint is triggered on Execution, but this behavior
		/// can be changed by setting the appropreate Properties.
		/// </remarks>
		/// <param name="address">Memory address</param>
		public MemoryBreakpoint(long address)
		{
			Address = address;
			OnExecute = true;
			Enabled = true;
		}

		/// <summary>
		/// Creates a Memory breakpoint by optaining the address from
		/// the Debug Information object of the Debugger.
		/// </summary>
		/// <remarks>
		/// If the Debug Symbol is marked as Executable, the breakpoint
		/// will be triggered on Execution by default; otherwise on Write.
		/// </remarks>
		/// <param name="debugger">Debugger used to access Debug Information</param>
		/// <param name="file">Source filename where the symbol is defined. If null or empty,
		/// the filename of the first Debug symbol in the table will be assumed.</param>
		/// <param name="line">Line within the file where the symbol is defined.</param>
		public MemoryBreakpoint(Debugger debugger, string file, int line)
		{
			if (debugger == null) throw new ArgumentNullException("debugger");

			if (string.IsNullOrEmpty(file))
			{
				try { file = debugger.DebugInformation.Symbols[0].Location.File; }
				catch { }
			}

			DebugSymbolLocation location = new DebugSymbolLocation();
			location.File = file;
			location.Line = line;

			DebugSymbol symbol;
			if (!debugger.DebugInformation.ByLocation.TryGetValue(location, out symbol))
			{
				throw new DebugSymbolNotFoundException();
			}

			Address = symbol.Value;

			OnExecute = symbol.IsExecutable;
			OnWrite = !symbol.IsExecutable;
			Enabled = true;
		}
		
		/// <summary>
		/// Creates a Memory breakpoint by optaining the address from
		/// the Debug Information object of the Debugger.
		/// </summary>
		/// <remarks>
		/// If the Debug Symbol is marked as Executable, the breakpoint
		/// will be triggered on Execution by default; otherwise on Write.
		/// </remarks>
		/// <param name="debugger">Debugger used to access Debug Information</param>
		/// <param name="line">Line within the default source file, where the symbol is defined.</param>
		public MemoryBreakpoint(Debugger debugger, int line) : this(debugger, string.Empty, line) { }

		/// <summary>
		/// Platform specific Memory breakpoint checker.
		/// </summary>
		public IMemoryBreakpointChecker Checker { get; set; }

		/// <summary>
		/// Checks whether the breakpoint is hit.
		/// </summary>
		/// <remarks>
		/// If the Checker is not set, throws a <c>NullCheckerException</c>.
		/// </remarks>
		/// <returns>A value indicating whether the breakpoint is hit</returns>
		public override bool Check()
		{
			if (Checker == null) throw new NullCheckerException();
			if (!Enabled) return false;
			if (OnExecute)
			{
				if (Checker.IsAboutToExecute(Address, Count))
					return true;
			}
			if (OnRead)
			{
				if (Checker.IsAboutToRead(Address, Count))
					return true;
			}
			if (OnWrite)
			{
				if (Checker.IsAboutToWrite(Address, Count))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Checks if the Breakpoint Checker is set.
		/// </summary>
		/// <returns>A value indicating whether the Checker is set.</returns>
		public override bool IsCheckerSet()
		{
			return Checker != null;
		}

		/// <summary>
		/// Set the Checker if the specified object is of the correct type
		/// for this breakpoint.
		/// </summary>
		/// <param name="checker">A Breakpoint Checker</param>
		/// <returns><c>true</c> if <c>checker</c> is the right type; otherwise <c>false</c></returns>
		public override bool TrySetChecker(object checker)
		{
			IMemoryBreakpointChecker c = checker as IMemoryBreakpointChecker;
			if (c != null)
			{
				Checker = c;
				return true;
			}
			return false;
		}
	}
}
