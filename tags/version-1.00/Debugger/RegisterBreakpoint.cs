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
	/// Breakpoint for Register access
	/// </summary>
	public class RegisterBreakpoint : Breakpoint
	{
		/// <summary>
		/// Name of the register.
		/// </summary>
		public string Register { get; set; }

		/// <summary>
		/// Creates a Register breakpoint fo the specified
		/// register name.
		/// </summary>
		/// <remarks>
		/// By default the breakpoint is triggered on Write.
		/// </remarks>
		/// <param name="register">The register name.</param>
		public RegisterBreakpoint(string register)
		{
			Register = register;
			OnWrite = true;
			Enabled = true;
		}

		/// <summary>
		/// Platform specific Register breakpoint checker.
		/// </summary>
		public IRegisterBreakpointChecker Checker { get; set; }

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
			if (OnRead)
			{
				if (Checker.IsAboutToRead(Register))
					return true;
			}
			if (OnWrite)
			{
				if (Checker.IsAboutToWrite(Register))
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
			IRegisterBreakpointChecker c = checker as IRegisterBreakpointChecker;
			if (c != null)
			{
				Checker = c;
				return true;
			}
			return false;
		}
	}
}
