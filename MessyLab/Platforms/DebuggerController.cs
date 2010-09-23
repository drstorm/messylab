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
using MessyLab.Debugger;

namespace MessyLab.Platforms
{
	/// <summary>
	/// Debugger Integration.
	/// </summary>
	public abstract class DebuggerController
	{
		#region States
		/// <summary>
		/// Debugger states.
		/// </summary>
		public enum States
		{
			NotLoaded, Suspended, Running
		}

		/// <summary>
		/// Handles the Debugger state change.
		/// </summary>
		/// <param name="state">The new debugger state.</param>
		public delegate void StateChangedHandler(States state);

		/// <summary>
		/// State specific behavior.
		/// </summary>
		public interface IState
		{
			/// <summary>
			/// Handles execution interruption.
			/// </summary>
			/// <param name="reason">The interrupt reason.</param>
			void HandleExecutionInterrupt(MessyLab.Debugger.ExecutionInterruptReason reason);

			/// <summary>
			/// Initializes state. Should be called when entering state.
			/// </summary>
			void Initialize();

			void Run();
			void Pause();
			void Stop();
			void Restart();

			void StepInto(bool back);
			void StepOver(bool back);
			void StepOut(bool back);

			States State { get; }
		}
		#endregion

		#region IState Implementations

		#region NotLoadedState
		/// <summary>
		/// Implements the behavior when there is no program loaded.
		/// </summary>
		public class NotLoadedState : IState
		{
			#region Controller
			public NotLoadedState(DebuggerController controller)
			{
				Controller = controller;
				Platform = controller.Platform;
			}

			public DebuggerController Controller { get; set; }
			public DebuggablePlatform Platform { get; set; }

			public States State { get { return States.NotLoaded; } }
			#endregion

			public virtual void HandleExecutionInterrupt(ExecutionInterruptReason reason) { }

			public virtual void Initialize()
			{
				Controller.ExecutionInterruptReason = null;
			}

			public virtual void Run()
			{
				string path;
				if (Platform.Build(out path))
				{
					Controller.Debugger.Load(path);

					// Note: It is important to set state before calling Run as
					// this eliminates the race condition.
					Controller.State = States.Running;
					Controller.Debugger.ExecuteBackwards = false;
					Controller.Debugger.Run();
				}
			}

			public virtual void StepInto(bool back)
			{
				string path;
				if (Platform.Build(out path))
				{
					Controller.Debugger.Load(path);
					Controller.State = States.Suspended;
				}
			}

			public virtual void StepOver(bool back) { StepInto(back); }
			public virtual void StepOut(bool back) { if (!back) Run(); }

			#region N/A
			public virtual void Pause() { }
			public virtual void Stop() { }
			public virtual void Restart() { }
			#endregion

		}
		#endregion

		#region SuspendedState
		/// <summary>
		/// Implements the behavior when the debugger is in suspended state.
		/// </summary>
		public class SuspendedState : IState
		{
			#region Controller
			public SuspendedState(DebuggerController controller)
			{
				Controller = controller;
				Platform = controller.Platform;
			}

			public DebuggerController Controller { get; set; }
			public DebuggablePlatform Platform { get; set; }

			public States State { get { return States.Suspended; } }
			#endregion

			/// <summary>
			/// Is restart requested.
			/// </summary>
			private volatile bool _restart;

			public virtual void HandleExecutionInterrupt(ExecutionInterruptReason reason)
			{
				// If (program stopped)
				if (reason.Exception != null && reason.Exception is NoProgramLoadedException)
				{
					Controller.State = States.NotLoaded;

					if (_restart)
					{
						Controller.StepInto(false);
					}
				}
			}

			/// <summary>
			/// Sets HighlightedFile and HighlightedLine properties of the controller
			/// according to the current instruction.
			/// </summary>
			private void SetHighlight()
			{
				long address = Controller.Debugger.Target.ProgramLocation.CurrentInstructionAddress;
				try
				{
					DebugSymbol s = Controller.Debugger.DebugInformation.ByValue[address];
					Controller.HighlightedFile = s.Location.File;
					Controller.HighlightedLine = s.Location.Line;
				}
				catch (KeyNotFoundException)
				{ }
			}

			public virtual void Initialize()
			{
				SetHighlight();
			}

			public virtual void Run()
			{
				Controller.State = States.Running;
				Controller.Debugger.ExecuteBackwards = false;
				Controller.Debugger.Run();
			}

			public virtual void Stop()
			{
				_restart = false;
				Controller.Debugger.Unload();
			}

			public virtual void Restart()
			{
				_restart = true;
				Controller.Debugger.Unload();
			}

			public virtual void StepInto(bool back)
			{
				Controller.State = States.Running;
				Controller.Debugger.ExecuteBackwards = back;
				Controller.Debugger.StepInto();
			}

			public virtual void StepOver(bool back)
			{
				Controller.State = States.Running;
				Controller.Debugger.ExecuteBackwards = back;
				Controller.Debugger.StepOver();
			}

			public virtual void StepOut(bool back)
			{
				Controller.State = States.Running;
				Controller.Debugger.ExecuteBackwards = back;
				Controller.Debugger.StepOut();
			}

			#region N/A
			public virtual void Pause() { }
			#endregion
		}
		#endregion

		#region RunningState
		/// <summary>
		/// Implements the behavior when the debugger is running.
		/// </summary>
		public class RunningState : IState
		{
			#region Controller
			public RunningState(DebuggerController controller)
			{
				Controller = controller;
				Platform = controller.Platform;
			}

			public DebuggerController Controller { get; set; }
			public DebuggablePlatform Platform { get; set; }

			public States State { get { return States.Running; } }
			#endregion

			/// <summary>
			/// Is restart requested.
			/// </summary>
			private volatile bool _restart;

			public virtual void HandleExecutionInterrupt(ExecutionInterruptReason reason)
			{
				// If (program stopped)
				if (reason.Exception != null && reason.Exception is NoProgramLoadedException)
				{
					Controller.State = States.NotLoaded;

					if (_restart)
					{
						Controller.StepInto(false);
					}
				}
				else
				{
					// Other reasons
					Controller.State = States.Suspended;
				}
			}

			public virtual void Initialize() { }

			public virtual void Pause()
			{
				Controller.Debugger.Pause();
			}

			public virtual void Stop()
			{
				_restart = false;
				Controller.Debugger.Unload();
			}

			public virtual void Restart()
			{
				_restart = true;
				Controller.Debugger.Unload();
			}

			#region N/A
			public virtual void Run() { }
			public virtual void StepInto(bool back) { }
			public virtual void StepOver(bool back) { }
			public virtual void StepOut(bool back) { }
			#endregion
		}
		#endregion

		#endregion

		public DebuggerController(DebuggablePlatform platform)
		{
			Platform = platform;
		}

		/// <summary>
		/// Initializes the controller.
		/// </summary>
		public virtual void Initialize()
		{
			CreateDebugger();
			CreateBreakpointController();
			State = States.NotLoaded;
		}

		/// <summary>
		/// Parent debuggable platform.
		/// </summary>
		public DebuggablePlatform Platform { get; set; }

		#region Breakpoint Controller
		public BreakpointController BreakpointController { get; protected set; }

		/// <summary>
		/// Creates the Breakpoint controller.
		/// </summary>
		protected virtual void CreateBreakpointController()
		{
			BreakpointController = new BreakpointController(this);
		}
		#endregion

		#region Debugger
		public Debugger.Debugger Debugger { get; protected set; }

		/// <summary>
		/// Creates and initializes the debugger.
		/// </summary>
		public abstract void CreateDebugger();

		/// <summary>
		/// Indicates whether the backward execution is supported.
		/// </summary>
		public abstract bool IsBackwardsExecutionSupported { get; }

		/// <summary>
		/// Handles the execution interruption by calling the handler of the current IState.
		/// </summary>
		/// <param name="reason">The interrupt reason.</param>
		protected virtual void HandleExecutionInterrupt(ExecutionInterruptReason reason)
		{
			var main = Platform.Gui.MainForm;
			// Invoke will most likely be required because
			// Debugger runs in a separate thread.
			if (main.InvokeRequired)
			{
				main.BeginInvoke(new Action<ExecutionInterruptReason>(HandleExecutionInterrupt), reason);
				return;
			}

			ExecutionInterruptReason = reason;
			if (CurrentIState != null) CurrentIState.HandleExecutionInterrupt(reason);
		}
		#endregion

		#region Program Data
		/// <summary>
		/// The reason of the last interrupt.
		/// </summary>
		public virtual ExecutionInterruptReason ExecutionInterruptReason { get; protected set; }

		public string HighlightedFile { get; set; }
		public int HighlightedLine { get; set; }
		#endregion

		#region Stepping
		/// <summary>
		/// Starts on continues program execution.
		/// </summary>
		public virtual void Run() { CurrentIState.Run(); }
		/// <summary>
		/// Pauses program execution.
		/// </summary>
		public virtual void Pause() { CurrentIState.Pause(); }
		/// <summary>
		/// Stops the program execution. It is not possible to continue.
		/// </summary>
		public virtual void Stop() { CurrentIState.Stop(); }
		/// <summary>
		/// Restarts the current program.
		/// </summary>
		public virtual void Restart() { CurrentIState.Restart(); }

		/// <summary>
		/// Steps one instruction.
		/// </summary>
		/// <param name="back">A value indicating whether to take the step backwards.</param>
		public virtual void StepInto(bool back) { CurrentIState.StepInto(back); }
		/// <summary>
		/// Steps one line.
		/// </summary>
		/// <param name="back">A value indicating whether to take the step backwards.</param>
		public virtual void StepOver(bool back) { CurrentIState.StepOver(back); }
		/// <summary>
		/// Steps out of the current sub-routine.
		/// </summary>
		/// <param name="back">A value indicating whether to take the step backwards.</param>
		public virtual void StepOut(bool back) { CurrentIState.StepOut(back); }
		#endregion

		#region State
		protected abstract IState NotLoadedIState { get; }
		protected abstract IState SuspendedIState { get; }
		protected abstract IState RunningIState { get; }

		private IState _currentIState;
		/// <summary>
		/// Gets or sets the current IState object.
		/// </summary>
		protected virtual DebuggerController.IState CurrentIState
		{
			get
			{
				lock (this) { return _currentIState; }
			}
			set
			{
				lock (this)
				{
					_currentIState = value;
					_currentIState.Initialize();
				}
				if (Platform.Gui.MainForm.InvokeRequired)
				{
					Platform.Gui.MainForm.BeginInvoke(new Action<States>(OnStateChanged), _currentIState.State);
				}
				else
				{
					OnStateChanged(_currentIState.State);
				}
			}
		}

		/// <summary>
		/// Gets or sets the Controller state.
		/// </summary>
		public virtual States State
		{
			get
			{
				if (CurrentIState == null) return States.NotLoaded;
				return CurrentIState.State;
			}
			set
			{
				if (CurrentIState != null && CurrentIState.State == value) return;

				switch (value)
				{
					case States.NotLoaded:
						CurrentIState = NotLoadedIState;
						break;
					case States.Suspended:
						CurrentIState = SuspendedIState;
						break;
					case States.Running:
						CurrentIState = RunningIState;
						break;
				}
			}
		}

		/// <summary>
		/// Occurs when the controller state is changed.
		/// </summary>
		public event StateChangedHandler StateChanged;
		protected virtual void OnStateChanged(States state)
		{ if (StateChanged != null) StateChanged(state); }
		#endregion
	}
}
