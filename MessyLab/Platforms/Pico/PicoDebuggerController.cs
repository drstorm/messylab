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
using System.IO;
using MessyLab.Debugger;
using System.Threading;

namespace MessyLab.Platforms.Pico
{
	/// <summary>
	/// picoComputer-specific debugger integration.
	/// </summary>
	public class PicoDebuggerController : DebuggerController
	{
		#region IState Implementations

		#region PicoNotLoadedState
		public class PicoNotLoadedState : NotLoadedState
		{
			public PicoNotLoadedState(PicoDebuggerController controller) : base(controller) { }

			public override void Run()
			{
				base.Run();
				(Controller as PicoDebuggerController).IODevice.Clear();
			}

			public override void StepInto(bool back)
			{
				base.StepInto(back);
				(Controller as PicoDebuggerController).IODevice.Clear();
			}
		}
		#endregion

		#region PicoRunningState
		public class PicoRunningState : RunningState
		{
			public PicoRunningState(PicoDebuggerController controller) : base(controller) { }

			public override void Stop()
			{
				base.Stop();
				(Controller as PicoDebuggerController).IODevice.Abort();
			}

			public override void Restart()
			{
				base.Restart();
				(Controller as PicoDebuggerController).IODevice.Abort();
			}
		}
		#endregion

		#endregion

		public PicoDebuggerController(DebuggablePlatform platform)
			: base(platform)
		{
			_notLoadedIState = new PicoNotLoadedState(this);
			// No picoComputer specific stuff in suspended state.
			_suspendedIState = new SuspendedState(this);
			_runningIState = new PicoRunningState(this);
		}

		#region Debugger
		/// <summary>
		/// Creates a debugger for picoComputer and initializes the virtual machine.
		/// </summary>
		public override void CreateDebugger()
		{
			var vm = new VirtualMachine(new Data(), new NullIODevice());
			var target = new MessyLab.Debugger.Target.Pico.PicoTarget(vm);
			Debugger = new MessyLab.Debugger.Debugger(target);
			Debugger.ExecutionInterrupt += new ExecutionInterruptHandler(HandleExecutionInterrupt);
		}

		public override bool IsBackwardsExecutionSupported
		{
			get { return true; }
		}
		#endregion

		#region State
		private IState _notLoadedIState;
		protected override IState NotLoadedIState { get { return _notLoadedIState; } }

		private IState _suspendedIState;
		protected override IState SuspendedIState { get { return _suspendedIState; } }

		private IState _runningIState;
		protected override IState RunningIState { get { return _runningIState; } }
		#endregion

		/// <summary>
		/// Current picoComputer I/O Device.
		/// </summary>
		public IIODevice IODevice
		{
			get
			{
				if (Debugger == null) return null;
				var target = Debugger.Target as MessyLab.Debugger.Target.Pico.PicoTarget;
				return target.VirtualMachine.IODevice;
			}
			set
			{
				if (Debugger == null) return;
				var target = Debugger.Target as MessyLab.Debugger.Target.Pico.PicoTarget;
				target.VirtualMachine.IODevice = value;
			}
		}

	}
}
