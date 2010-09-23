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
	/// Breakpoint used by the debugger to perform Step Operations.
	/// </summary>
	public class StepBreakpoint : Breakpoint
	{
		/// <summary>
		/// The program location of the beginning of the step.
		/// </summary>
		public IProgramLocation OriginLocation { get; set; }

		/// <summary>
		/// The kind of step (into, out, etc.)
		/// </summary>
		public StepKind Kind { get; set; }

		/// <summary>
		/// Creates a Step breakpoint of the specified kind using the
		/// specified target to obtain current program location.
		/// </summary>
		/// <param name="target">Debugger target.</param>
		/// <param name="kind">Step kind.</param>
		public StepBreakpoint(ITarget target, StepKind kind) : this(target.ProgramLocation, kind) { }

		/// <summary>
		/// Creates a Step breakpoint using the specified parameters.
		/// </summary>
		/// <param name="originLocation">Origin location of the step.</param>
		/// <param name="kind">Step kind.</param>
		public StepBreakpoint(IProgramLocation originLocation, StepKind kind)
		{
			OriginLocation = originLocation;
			Kind = kind;
			OnExecute = true;
			IsOneShot = true;
			Enabled = true;
		}

		/// <summary>
		/// Platform specific Step breakpoint checker.
		/// </summary>
		public IStepBreakpointChecker Checker { get; set; }

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
			if (Checker.IsStepCompleted(OriginLocation, Kind))
				return true;
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
			IStepBreakpointChecker c = checker as IStepBreakpointChecker;
			if (c != null)
			{
				Checker = c;
				return true;
			}
			return false;
		}
	}

	/// <summary>
	/// Represents the supported Step Kinds.
	/// </summary>
	public enum StepKind
	{
		Into, Over, Out
	}
}
