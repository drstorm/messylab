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
using System.Threading;
using MessyLab.Debugger.Target;

namespace MessyLab.Debugger
{
	/// <summary>
	/// Encapsulates the Thread that executes the program
	/// by calling <c>Step</c> and <c>StepBack</c> methods
	/// of the target.
	/// </summary>
	public class Engine
	{
		/// <summary>
		/// Constructs an Engine for the specified Debugger.
		/// </summary>
		/// <param name="debugger">Debugger instance.</param>
		public Engine(Debugger debugger)
		{
			Debugger = debugger;
			_pause = false;
			_stop = false;
			AllowedToContinue = new AutoResetEvent(false);
		}

		public Debugger Debugger { get; set; }
		
		/// <summary>
		/// Thread executing the Target's program.
		/// </summary>
		public Thread Thread { get; set; }

		/// <summary>
		/// The events signalizes the execution thread when to continue
		/// after an execution interruption.
		/// </summary>
		protected AutoResetEvent AllowedToContinue { get; set; }

		/// <summary>
		/// Signals that the execution was interrupted due to calling the <c>Pause</c>
		/// method.
		/// </summary>
		protected void SignalPause()
		{
			Debugger.OnExecutionInterrupt(new ExecutionInterruptReason());
			AllowedToContinue.Reset();
			AllowedToContinue.WaitOne();
		}

		/// <summary>
		/// Signals that the execution was interrupted due to hitting one or more
		/// breakpoints.
		/// </summary>
		protected void SignalBreakpoint()
		{
			ExecutionInterruptReason reason =
				new ExecutionInterruptReason(Debugger.Breakpoints.HitBreakpoints.ToArray());
			Debugger.OnExecutionInterrupt(reason);
			AllowedToContinue.Reset();
			AllowedToContinue.WaitOne();
		}

		/// <summary>
		/// Signals that the execution was interrupted due to en exception.
		/// </summary>
		/// <param name="exception">Exception that caused the interruption</param>
		protected void SignalException(Exception exception)
		{
			Debugger.OnExecutionInterrupt(new ExecutionInterruptReason(exception));
			AllowedToContinue.Reset();
			AllowedToContinue.WaitOne();
		}

		/// <summary>
		/// Signals that the execution has been stopped.
		/// </summary>
		protected void SignalStop()
		{
			Debugger.OnExecutionInterrupt(new ExecutionInterruptReason(new NoProgramLoadedException()));
		}

		/// <summary>
		/// A volatile value holding the request for an execution Pause.
		/// </summary>
		private volatile bool _pause;
		/// <summary>
		/// A volatile value holding the reqest to stop the execution.
		/// </summary>
		private volatile bool _stop;
		
		/// <summary>
		/// Pauses the execution.
		/// </summary>
		public void Pause()
		{
			_pause = true;
		}

		/// <summary>
		/// Starts or Continues the program execution.
		/// </summary>
		public void Continue()
		{
			lock (this)
			{
				if (Thread == null)
				{
					_stop = false;
					_pause = false;
					Thread = new Thread(new ThreadStart(Run));
					Thread.Start();
				}
				else
				{
					AllowedToContinue.Set();
				}
			}
		}

		/// <summary>
		/// Stops the execution.
		/// </summary>
		public void Stop()
		{
			if (Thread == null)
			{
				SignalStop();
				return;
			}
			_stop = true;
			AllowedToContinue.Set();
		}

		/// <summary>
		/// Runs the program. This method is executed in a separate Thread.
		/// </summary>
		protected void Run()
		{
			while (!_stop)
			{
				Debugger.Breakpoints.Process();
				if (Debugger.Breakpoints.HitBreakpoints.Count > 0)
				{
					Debugger.Breakpoints.RemoveStepBreakpoints();
					SignalBreakpoint();
					_pause = false;
				}

				if (_stop) break;

				if (_pause)
				{
					Debugger.Breakpoints.RemoveStepBreakpoints();
					SignalPause();
					_pause = false;
				}

				if (_stop) break;

				try
				{
					if (Debugger.ExecuteBackwards) { Debugger.Target.StepBack(); }
					else { Debugger.Target.Step(); }
				}
				catch (Exception e)
				{
					if (_stop) break;
					Debugger.Breakpoints.RemoveStepBreakpoints();
					SignalException(e);
					_pause = false;
				}
			}
			SignalStop();
			Thread = null;
		}
	}
}
