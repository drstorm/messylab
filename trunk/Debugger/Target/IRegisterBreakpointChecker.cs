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
	/// Interface of a Register Breakpoint Checker.
	/// </summary>
	/// <remarks>
	/// A Breakpoint checker object provides functionality necessary to
	/// determine whether a specific breakpoint type is triggered.
	/// </remarks>
	public interface IRegisterBreakpointChecker
	{
		/// <summary>
		/// Checks whether the specified register is about to be
		/// read.
		/// </summary>
		/// <param name="register">Register name</param>
		/// <returns>A value indicating whether the specified register is about to be read.</returns>
		bool IsAboutToRead(string register);
		
		/// <summary>
		/// Checks whether the specified register is about to be
		/// written to.
		/// </summary>
		/// <param name="register">Register name</param>
		/// <returns>A value indicating whether the specified register is about to be written to.</returns>
		bool IsAboutToWrite(string register);
	}
}
