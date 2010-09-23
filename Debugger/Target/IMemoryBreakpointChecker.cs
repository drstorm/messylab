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
	/// Interface of a Memory Breakpoint Checker.
	/// </summary>
	/// <remarks>
	/// A Breakpoint checker object provides functionality necessary to
	/// determine whether a specific breakpoint type is triggered.
	/// </remarks>
	public interface IMemoryBreakpointChecker
	{
		/// <summary>
		/// Checks whether the specified memory location is about to be
		/// executed.
		/// </summary>
		/// <param name="address">The memory address</param>
		/// <returns>A value indicating whether the specified memory location is about to be executed.</returns>
		bool IsAboutToExecute(long address);

		/// <summary>
		/// Checks whether the specified memory location is about to be
		/// read.
		/// </summary>
		/// <param name="address">The memory address</param>
		/// <returns>A value indicating whether the specified memory location is about to be read.</returns>
		bool IsAboutToRead(long address);

		/// <summary>
		/// Checks whether the specified memory location is about to be
		/// written to.
		/// </summary>
		/// <param name="address">The memory address</param>
		/// <returns>A value indicating whether the specified memory location is about to be written to.</returns>
		bool IsAboutToWrite(long address);

		/// <summary>
		/// Checks whether a location in the specified memory range is about to be
		/// executed.
		/// </summary>
		/// <param name="address">The staring memory address.</param>
		/// <param name="count">The the number of memory locations.</param>
		/// <returns>A value indicating whether a location in the specified memory range is about to be executed.</returns>
		bool IsAboutToExecute(long address, int count);

		/// <summary>
		/// Checks whether a location in the specified memory range is about to be
		/// read.
		/// </summary>
		/// <param name="address">The staring memory address.</param>
		/// <param name="count">The the number of memory locations.</param>
		/// <returns>A value indicating whether a location in the specified memory range is about to be read.</returns>
		bool IsAboutToRead(long address, int count);

		/// <summary>
		/// Checks whether a location in the specified memory range is about to be
		/// written to.
		/// </summary>
		/// <param name="address">The staring memory address.</param>
		/// <param name="count">The the number of memory locations.</param>
		/// <returns>A value indicating whether a location in the specified memory range is about to be written to.</returns>
		bool IsAboutToWrite(long address, int count);
	}
}
