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
using System.IO;

namespace MessyLab.Debugger
{
	/// <summary>
	/// This class encapsulates the Debugger functionaloty.
	/// </summary>
	public class Debugger
	{
		/// <summary>
		/// This event is used to signalize an execution interuption (breakpoint hit,
		/// execution raised, etc.)
		/// </summary>
		public event ExecutionInterruptHandler ExecutionInterrupt;

		/// <summary>
		/// Fires the <c>ExecutionInterrupt</c> event.
		/// </summary>
		/// <param name="reason">The reason for the interruption</param>
		internal void OnExecutionInterrupt(ExecutionInterruptReason reason)
		{
			if (ExecutionInterrupt != null)
			{
				ExecutionInterrupt(reason);
			}
		}

		public ITarget Target { get; set; }
		public BreakpointManager Breakpoints { get; protected set; }
		public DebugInformation DebugInformation { get; protected set; }
		protected Engine Engine { get; set; }

		/// <summary>
		/// A volatile value for <c>ExecuteBackwards</c> property.
		/// </summary>
		private volatile bool _executeBackwards;

		/// <summary>
		/// Indicates whether the program should be executed backwards.
		/// </summary>
		/// <remarks>The backward execution need to be supported by the Target.</remarks>
		public bool ExecuteBackwards { get { return _executeBackwards; } set { _executeBackwards = value; } }

		/// <summary>
		/// Constructs the Debugger for the specified Target.
		/// </summary>
		/// <param name="target">The target to be debugged.</param>
		public Debugger(ITarget target)
		{
			if (target == null) throw new ArgumentNullException();

			Target = target;
			Breakpoints = new BreakpointManager(this);
			DebugInformation = new DebugInformation();
		}

		/// <summary>
		/// Performs a single step (generally, executes a single instruction).
		/// </summary>
		public void StepInto()
		{
			if (Engine == null) throw new NoProgramLoadedException();
			Breakpoints.Add(new StepBreakpoint(Target.ProgramLocation, StepKind.Into));
			Engine.Continue();
		}

		/// <summary>
		/// Performs a step over subroutines. If the current instruction is not a
		/// subroutine call, the effect is the same as <c>StepInto</c>.
		/// </summary>
		public void StepOver()
		{
			if (Engine == null) throw new NoProgramLoadedException();
			Breakpoints.Add(new StepBreakpoint(Target.ProgramLocation, StepKind.Over));
			Engine.Continue();
		}

		/// <summary>
		/// Steps out of a subroutine.
		/// </summary>
		public void StepOut()
		{
			if (Engine == null) throw new NoProgramLoadedException();
			Breakpoints.Add(new StepBreakpoint(Target.ProgramLocation, StepKind.Out));
			Engine.Continue();
		}

		/// <summary>
		/// Runs the program. Execution stops if either a breakpoint is hit,
		/// an exception is thrown or if <c>Pause</c> method was called.
		/// </summary>
		public void Run()
		{
			if (Engine == null) throw new NoProgramLoadedException();
			Engine.Continue();
		}

		/// <summary>
		/// Manually interrupts the program execution.
		/// </summary>
		public void Pause()
		{
			if (Engine == null) throw new NoProgramLoadedException();
			Engine.Pause();
		}

		/// <summary>
		/// Unloads the currently loaded program.
		/// </summary>
		public void Unload()
		{
			if (Engine == null) throw new NoProgramLoadedException();
			
			var oldEngine = Engine;
			Engine = null;
			DebugInformation.Clear();
			
			// It is important to set the Engine to null and Clear the
			// Debug information before calling Stop, because it is
			// the Engine who signals the end of operation.
			
			// So if Stop was called first, it could be signaled
			// before the debugger is fully stopped, especially
			// if the engine thread was not started at all.
			oldEngine.Stop();
		}

		/// <summary>
		/// Loads the specified program.
		/// </summary>
		/// <param name="path">Full path to the program binary.</param>
		/// <param name="debugInfoPath">Full path to the Debug Information file.</param>
		public void Load(string path, string debugInfoPath)
		{
			Target.Load(path);
			if (File.Exists(debugInfoPath))
			{
				DebugInformation.LoadFromFile(debugInfoPath);
			}
			Engine = new Engine(this);
		}
		
		/// <summary>
		/// Loads the specified program.
		/// </summary>
		/// <remarks>
		/// It is assumed that the Debug Information file is in the same directory
		/// as the binary with an additional ".mldbg" extension.
		/// </remarks>
		/// <param name="path">Full path to the program binary.</param>
		public void Load(string path)
		{
			Load(path, path + ".mldbg");
		}

	}

	/// <summary>
	/// Holds the reason for an Execution Interrupt
	/// </summary>
	public class ExecutionInterruptReason
	{
		/// <summary>
		/// The exception that caused the interruption.
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		/// The Breakpoint(s) that caused the interruption.
		/// </summary>
		public IEnumerable<Breakpoint> HitBreakpoints { get; set; }

		public ExecutionInterruptReason() : this(null, new Breakpoint[] { }) { }
		
		public ExecutionInterruptReason(Exception exception) : this(exception, new Breakpoint[] { }) { }
		
		public ExecutionInterruptReason(IEnumerable<Breakpoint> hitBreakpoints) : this(null, hitBreakpoints) { }
		
		public ExecutionInterruptReason(Exception exception, IEnumerable<Breakpoint> hitBreakpoints)
		{
			Exception = exception;
			HitBreakpoints = hitBreakpoints;
		}
	}

	/// <summary>
	/// Delegate for an Execution Interrupt Handler.
	/// </summary>
	/// <param name="reason">The reason for the interruption</param>
	public delegate void ExecutionInterruptHandler(ExecutionInterruptReason reason);

	/// <summary>
	/// The exception thrown when attempting to execute a program prior to
	/// loading it.
	/// </summary>
	public class NoProgramLoadedException : Exception
	{
		public override string Message { get { return "No program loaded."; } }
	}
}
