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

namespace MessyLab.Platforms
{
	/// <summary>
	/// Keeps track of breakpoints.
	/// </summary>
	public class BreakpointController
	{
		#region Breakpoint Definitions

		#region BreakpointDefinition
		/// <summary>
		/// Defines a generic breakpoint.
		/// </summary>
		public abstract class BreakpointDefinition
		{
			/// <summary>
			/// Initializes a new instance.
			/// </summary>
			/// <param name="controller">The parent Breakpoint Controller.</param>
			public BreakpointDefinition(BreakpointController controller)
			{
				Controller = controller;
			}

			/// <summary>
			/// Parent breakpoint controller.
			/// </summary>
			public BreakpointController Controller { get; set; }

			/// <summary>
			/// Creates the corresponding debugger breakpoint.
			/// </summary>
			/// <returns></returns>
			protected abstract MessyLab.Debugger.Breakpoint CreateBreakpoint();

			private MessyLab.Debugger.Breakpoint _breakpoint;

			/// <summary>
			/// Gets or sets the debugger breakpoint. Returns the cached object if available.
			/// </summary>
			public MessyLab.Debugger.Breakpoint Breakpoint
			{
				get
				{
					if (_breakpoint == null)
					{
						_breakpoint = CreateBreakpoint();
					}
					return _breakpoint;
				}
				protected set
				{
					_breakpoint = value;
				}
			}

			/// <summary>
			/// The breakpoint textual representation.
			/// </summary>
			public abstract string Description { get; }

			/// <summary>
			/// The textual representation of breakpoint flags, e.g. rwx
			/// </summary>
			public abstract string Flags { get; }

			/// <summary>
			/// Creates a memento object that hold the definition data.
			/// </summary>
			/// <returns>A memento holding the breakpoint data.</returns>
			public abstract BreakpointMemento.Breakpoint ToMemento();
		}
		#endregion

		#region LineBreakpointDefinition
		/// <summary>
		/// Defines a source code line breakpoint.
		/// </summary>
		public class LineBreakpointDefinition : BreakpointDefinition
		{
			public LineBreakpointDefinition(BreakpointController controller) : base(controller) { }

			private ProjectItem _projectItem;
			/// <summary>
			/// Project item containing the breakpoint.
			/// </summary>
			public ProjectItem ProjectItem
			{
				get { return _projectItem; }
				set { _projectItem = value; Breakpoint = null; }
			}
			
			private int _line;
			/// <summary>
			/// Line of code the breakpoint is on.
			/// </summary>
			public int Line
			{
				get { return _line; }
				set { _line = value; Breakpoint = null; }
			}

			protected override MessyLab.Debugger.Breakpoint CreateBreakpoint()
			{
				try
				{
					var bp = new MessyLab.Debugger.MemoryBreakpoint(Controller.DebuggerController.Debugger, ProjectItem.Filename, Line);
					return bp;
				}
				catch { return null; }
			}

			public override string Description
			{
				get { return ProjectItem.Filename + ":" + Line; }
			}

			public override string Flags { get { return string.Empty; } }

			public override BreakpointMemento.Breakpoint ToMemento()
			{
				return new BreakpointMemento.Breakpoint
				{
					Type = BreakpointMemento.BreakpointType.Line,
					File = ProjectItem.Filename,
					Line = Line
				};
			}
		}
		#endregion

		#region MemoryBreakpointDefinition
		/// <summary>
		/// Defines a memory breakpoint.
		/// </summary>
		public class MemoryBreakpointDefinition : BreakpointDefinition
		{
			public MemoryBreakpointDefinition(BreakpointController controller) : base(controller) { }

			private long _address;
			private int _count;
			private bool _onRead;
			private bool _onWrite;
			private bool _onExecute;

			/// <summary>
			/// The start address.
			/// </summary>
			public long Address
			{
				get { return _address; }
				set { _address = value; Breakpoint = null; }
			}

			/// <summary>
			/// The count of memory locations covered by the breakpoint.
			/// </summary>
			public int Count
			{
				get { return _count; }
				set { _count = value; Breakpoint = null; }
			}

			/// <summary>
			/// Break on read.
			/// </summary>
			public bool OnRead
			{
				get { return _onRead; }
				set { _onRead = value; Breakpoint = null; }
			}

			/// <summary>
			/// Break on write.
			/// </summary>
			public bool OnWrite
			{
				get { return _onWrite; }
				set { _onWrite = value; Breakpoint = null; }
			}

			/// <summary>
			/// Break on Execute.
			/// </summary>
			public bool OnExecute
			{
				get { return _onExecute; }
				set { _onExecute = value; Breakpoint = null; }
			}

			protected override MessyLab.Debugger.Breakpoint CreateBreakpoint()
			{
				var bp = new MessyLab.Debugger.MemoryBreakpoint(Address);
				bp.Count = Count;
				bp.OnRead = OnRead;
				bp.OnWrite = OnWrite;
				bp.OnExecute = OnExecute;
				return bp;
			}

			public override string Description
			{
				get 
				{
					var sb = new StringBuilder();
					sb.Append("Address: ");
					sb.Append(Address);
					sb.Append(", Count: ");
					sb.Append(Count);
					return sb.ToString();
				}
			}

			public override string Flags
			{
				get
				{
					var sb = new StringBuilder();
					if (OnRead) sb.Append('r');
					if (OnWrite) sb.Append('w');
					if (OnExecute) sb.Append('x');
					return sb.ToString();
				}
			}

			public override BreakpointMemento.Breakpoint ToMemento()
			{
				return new BreakpointMemento.Breakpoint
				{
					Type = BreakpointMemento.BreakpointType.Memory,
					Address = Address,
					Count = Count,
					Read = OnRead,
					Write = OnWrite,
					Execute = OnExecute
				};
			}
		}
		#endregion

		#region RegisterBreakpointDefinition
		/// <summary>
		/// Defines a register breakpoint.
		/// </summary>
		public class RegisterBreakpointDefinition : BreakpointDefinition
		{
			public RegisterBreakpointDefinition(BreakpointController controller) : base(controller) { }

			private string _name;
			private bool _onRead;
			private bool _onWrite;

			/// <summary>
			/// The register name.
			/// </summary>
			public string Name
			{
				get { return _name; }
				set { _name = value; Breakpoint = null; }
			}

			/// <summary>
			/// Break on Read.
			/// </summary>
			public bool OnRead
			{
				get { return _onRead; }
				set { _onRead = value; Breakpoint = null; }
			}
			
			/// <summary>
			/// Break on Write.
			/// </summary>
			public bool OnWrite
			{
				get { return _onWrite; }
				set { _onWrite = value; Breakpoint = null; }
			}

			protected override MessyLab.Debugger.Breakpoint CreateBreakpoint()
			{
				var bp = new MessyLab.Debugger.RegisterBreakpoint(Name);
				bp.OnRead = OnRead;
				bp.OnWrite = OnWrite;
				return bp;
			}

			public override string Description
			{
				get { return Name; }
			}

			public override string Flags
			{
				get
				{
					var sb = new StringBuilder();
					if (OnRead) sb.Append('r');
					if (OnWrite) sb.Append('w');
					return sb.ToString();
				}
			}

			public override BreakpointMemento.Breakpoint ToMemento()
			{
				return new BreakpointMemento.Breakpoint
				{
					Type = BreakpointMemento.BreakpointType.Register,
					Name = Name,
					Read = OnRead,
					Write = OnWrite
				};
			}
		}
		#endregion

		#region IOBreakpointDefinition
		/// <summary>
		/// Defines an I/O breakpoint.
		/// </summary>
		public class IOBreakpointDefinition : BreakpointDefinition
		{
			public IOBreakpointDefinition(BreakpointController controller) : base(controller) { }

			long _address;
			bool _input;
			bool _output;

			/// <summary>
			/// The device address.
			/// </summary>
			public long Address
			{
				get { return _address; }
				set { _address = value; Breakpoint = null; }
			}
			
			/// <summary>
			/// Break on Input.
			/// </summary>
			public bool Input
			{
				get { return _input; }
				set { _input = value; Breakpoint = null; }
			}

			/// <summary>
			/// Break on Output.
			/// </summary>
			public bool Output
			{
				get { return _output; }
				set { _output = value; Breakpoint = null; }
			}

			protected override MessyLab.Debugger.Breakpoint CreateBreakpoint()
			{
				var bp = new MessyLab.Debugger.IOBreakpoint(Input, Output, Address);
				return bp;
			}

			public override string Description
			{
				get { return "Device " + Address; }
			}

			public override string Flags
			{
				get
				{
					var sb = new StringBuilder();
					if (Input) sb.Append('i');
					if (Output) sb.Append('o');
					return sb.ToString();
				}
			}

			public override BreakpointMemento.Breakpoint ToMemento()
			{
				return new BreakpointMemento.Breakpoint
				{
					Type = BreakpointMemento.BreakpointType.IO,
					Address = Address,
					Input = Input,
					Output = Output
				};
			}
		}
		#endregion

		#endregion

		/// <summary>
		/// Contains the defined breakpoints data.
		/// </summary>
		[Serializable]
		public class BreakpointMemento
		{
			/// <summary>
			/// Type of the breakpoint.
			/// </summary>
			public enum BreakpointType { Line, Memory, Register, IO };

			/// <summary>
			/// An universan breakpoint memento object.
			/// </summary>
			public struct Breakpoint
			{
				public BreakpointType Type;
				
				public string File;
				public int Line;

				public long Address;
				public int Count;

				public string Name;

				public bool Read;
				public bool Write;
				public bool Execute;

				public bool Input;
				public bool Output;
			}

			/// <summary>
			/// A list of breakpoints to store.
			/// </summary>
			public Breakpoint[] Breakpoints = new Breakpoint[0];
		}

		/// <summary>
		/// Parent Debugger Controller.
		/// </summary>
		public DebuggerController DebuggerController { get; set; }

		/// <summary>
		/// List of defined breakpoints.
		/// </summary>
		public List<BreakpointDefinition> Breakpoints { get; private set; }

		/// <summary>
		/// Intializes a new instance.
		/// </summary>
		/// <param name="debuggerController">The parent Debugger Controller.</param>
		public BreakpointController(DebuggerController debuggerController)
		{
			Breakpoints = new List<BreakpointDefinition>();

			DebuggerController = debuggerController;
			DebuggerController.StateChanged += new DebuggerController.StateChangedHandler(DebuggerController_StateChanged);
		}

		/// <summary>
		/// Stores the last debugger state.
		/// </summary>
		private DebuggerController.States _debuggerControllerState = DebuggerController.States.NotLoaded;

		/// <summary>
		/// Loads breakpoints to debugger on program start.
		/// </summary>
		/// <param name="state">The current debugger state.</param>
		protected void DebuggerController_StateChanged(DebuggerController.States state)
		{
			if (state != DebuggerController.States.NotLoaded)
			{
				if (_debuggerControllerState == DebuggerController.States.NotLoaded)
				{
					LoadLineBreakpointsFromOpenEditors();
					LoadBreakpointsToDebugger();
				}
			}
			_debuggerControllerState = state;
		}

		#region Debugger Update methods
		/// <summary>
		/// Loads the breakpoints to the debugger.
		/// </summary>
		protected virtual void LoadBreakpointsToDebugger()
		{
			DebuggerController.Debugger.Breakpoints.Clear();
			foreach (var bp in Breakpoints)
			{
				AddBreakpointToDebugger(bp);
			}
		}

		/// <summary>
		/// Adds the specified breakpoint to the debugger.
		/// </summary>
		/// <param name="breakpoint">The breakpoint to add.</param>
		protected virtual void AddBreakpointToDebugger(BreakpointDefinition breakpoint)
		{
			if (DebuggerController.State == DebuggerController.States.NotLoaded) return;
			var bp = breakpoint.Breakpoint;
			if (bp != null)
			{
				DebuggerController.Debugger.Breakpoints.Add(bp);
			}
		}

		/// <summary>
		/// Removes the specified breakpoint from the debugger.
		/// </summary>
		/// <param name="breakpoint">The breakpoint to remove.</param>
		protected virtual void RemoveBreakpointFromDebugger(BreakpointDefinition breakpoint)
		{
			if (DebuggerController.State == DebuggerController.States.NotLoaded) return;

			MessyLab.Debugger.Breakpoint bp = breakpoint.Breakpoint;

			if (bp != null)
			{
				DebuggerController.Debugger.Breakpoints.Remove(bp);
			}
		}
		#endregion

		#region Memento Save/Restore
		/// <summary>
		/// Creates a breakpoint memento object.
		/// </summary>
		/// <returns>The memento representing current breakpoints.</returns>
		public BreakpointMemento CreateMemento()
		{
			LoadLineBreakpointsFromOpenEditors();
			var l = new List<BreakpointMemento.Breakpoint>();
			foreach (var bp in Breakpoints)
			{
				l.Add(bp.ToMemento());
			}
			return new BreakpointMemento { Breakpoints = l.ToArray() };
		}

		/// <summary>
		/// Sets the specified memento object.
		/// </summary>
		/// <param name="memento">The memento to set.</param>
		public void SetMemento(BreakpointMemento memento)
		{
			// Clear all breakpoints.
			Breakpoints.Clear();
			var pl = DebuggerController.Platform;
			foreach (var item in pl.Project.Items)
			{
				var ed = pl.Editors.GetEditorFormIfExists(item);
				if (ed != null)
				{
					ed.ClearBreakpoints();
				}
			}

			// Add breakpoints from memento.
			foreach (var bp in memento.Breakpoints)
			{
				switch (bp.Type)
				{
					case BreakpointMemento.BreakpointType.Line:
						var b = new LineBreakpointDefinition(this);
						b.Line = bp.Line;
						foreach (var item in pl.Project.Items)
						{
							if (item.Filename == bp.File)
							{
								b.ProjectItem = item;
								Breakpoints.Add(b);
								var ed = pl.Editors.GetEditorFormIfExists(item);
								if (ed != null)
								{
									ed.ToggleBreakpointAt(bp.Line);
								}
								// If the editor does not exist yet, breakpoints
								// will be added on creation.
								break;
							}
						}
						break;
					case BreakpointMemento.BreakpointType.Memory:
						Breakpoints.Add(new MemoryBreakpointDefinition(this)
						{
							Address = bp.Address,
							Count = bp.Count,
							OnRead = bp.Read,
							OnWrite = bp.Write,
							OnExecute = bp.Execute
						});
						break;
					case BreakpointMemento.BreakpointType.Register:
						Breakpoints.Add(new RegisterBreakpointDefinition(this)
						{
							Name = bp.Name,
							OnRead = bp.Read,
							OnWrite = bp.Write
						});
						break;
					case BreakpointMemento.BreakpointType.IO:
						Breakpoints.Add(new IOBreakpointDefinition(this)
						{
							Address = bp.Address,
							Input = bp.Input,
							Output = bp.Output
						});
						break;
				}
			}
		}
		#endregion

		/// <summary>
		/// Adds the specified breakpoint definition and updates the debugger.
		/// </summary>
		/// <param name="breakpoint">The breakpoint to add.</param>
		public virtual void AddBreakpoint(BreakpointDefinition breakpoint)
		{
			Breakpoints.Add(breakpoint);
			AddBreakpointToDebugger(breakpoint);
		}

		/// <summary>
		/// Removes the speficified breakpoint definition and updates the debugger.
		/// </summary>
		/// <param name="breakpoint">The breakpoint to remove.</param>
		public virtual void RemoveBreakpoint(BreakpointDefinition breakpoint)
		{
			Breakpoints.Remove(breakpoint);
			RemoveBreakpointFromDebugger(breakpoint);
		}

		#region Line Breakpoints methods
		/// <summary>
		/// Loads the breakpoint definitions from open editors.
		/// </summary>
		protected virtual void LoadLineBreakpointsFromOpenEditors()
		{
			var pl = DebuggerController.Platform;

			// Remove all breakpoints for open editors.
			for (int i = Breakpoints.Count - 1; i >= 0; i--)
			{
				var bp = Breakpoints[i] as LineBreakpointDefinition;
				if (bp != null)
				{
					var ed = pl.Editors.GetEditorFormIfExists(bp.ProjectItem);
					if (ed != null)
					{
						Breakpoints.RemoveAt(i);
					}
				}
			}

			// Load all breakpoints from open editors.
			var items = pl.Project.Items;
			foreach (var item in items)
			{
				var ed = pl.Editors.GetEditorFormIfExists(item);
				if (ed != null)
				{
					foreach (var line in ed.Breakpoints)
					{
						AddLineBreakpoint(item, line);
					}
				}
			}
		}

		/// <summary>
		/// Adds a line breakpoint definition for the specified project item and line.
		/// </summary>
		/// <param name="item">The project item.</param>
		/// <param name="line">The code line.</param>
		public virtual void AddLineBreakpoint(ProjectItem item, int line)
		{
			var b = new LineBreakpointDefinition(this)
			{
				ProjectItem = item,
				Line = line
			};
			AddBreakpoint(b);
		}

		/// <summary>
		/// Removes the line breakpoint definition for the specified project item and line if any.
		/// </summary>
		/// <param name="item">The project item.</param>
		/// <param name="line">The code line.</param>
		public virtual void RemoveLineBreakpoint(ProjectItem item, int line)
		{
			for (int i = Breakpoints.Count - 1; i >= 0; i--)
			{
				var bp = Breakpoints[i] as LineBreakpointDefinition;
				if (bp != null && bp.ProjectItem == item && bp.Line == line)
				{
					RemoveBreakpoint(bp);
					return;
				}
			}
		}
		#endregion
	}
}
