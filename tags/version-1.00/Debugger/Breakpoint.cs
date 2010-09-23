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

namespace MessyLab.Debugger
{
	/// <summary>
	/// An abstract class representing a general breakpoint.
	/// </summary>
	public abstract class Breakpoint
	{
		/// <summary>
		/// A value indicating whether the breakpoint is enabled.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// A value indicating whether the breakpoint should be triggered
		/// on execution.
		/// </summary>
		public bool OnExecute { get; set; }

		/// <summary>
		/// A value indicating whether the breakpoint should be triggered
		/// on read.
		/// </summary>
		public bool OnRead { get; set; }

		/// <summary>
		/// A value indicating whether the breakpoint should be triggered
		/// on write.
		/// </summary>
		public bool OnWrite { get; set; }

		/// <summary>
		/// A value indicating whether this breakpoint is a "one-shot"
		/// breakpoint, meaning that the breakpoint is automatically
		/// removed by the manager once it is hit.
		/// </summary>
		public bool IsOneShot { get; set; }

		/// <summary>
		/// Checks if the Breakpoint Checker is set.
		/// </summary>
		/// <returns>A value indicating whether the Checker is set.</returns>
		public abstract bool IsCheckerSet();

		/// <summary>
		/// Set the Checker if the specified object is of the correct type
		/// for this breakpoint.
		/// </summary>
		/// <param name="checker">A Breakpoint Checker</param>
		/// <returns><c>true</c> if <c>checker</c> is the right type; otherwise <c>false</c></returns>
		public abstract bool TrySetChecker(object checker);

		/// <summary>
		/// Checks whether the breakpoint is hit.
		/// </summary>
		/// <remarks>
		/// If the Checker is not set, throws a <c>NullCheckerException</c>.
		/// </remarks>
		/// <returns>A value indicating whether the breakpoint is hit</returns>
		public abstract bool Check();
	}

	/// <summary>
	/// This exception is thrown when calling the Check() method of a breakpoint prior to
	/// setting an appropriate Checker object.
	/// </summary>
	public class NullCheckerException : Exception
	{
		public override string Message
		{
			get { return "No Breakpoint Checker set in the Checker property of Breakpoint."; }
		}
	}
}
