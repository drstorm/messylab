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
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace MessyLab.Platforms
{
	/// <summary>
	/// A Platform that supports Debugging.
	/// </summary>
	public abstract class DebuggablePlatform : Platform
	{
		#region GUI
		/// <summary>
		/// Provies and manages platform-specific GUI elements with debugging features.
		/// </summary>
		public abstract class DebuggableGuiProvider : GuiProvider
		{
			public DebuggableGuiProvider(DebuggablePlatform platform) : base(platform) { }

			/// <summary>
			/// Parent platform redefined as DebuggablePlatform for convenience.
			/// </summary>
			public new DebuggablePlatform Platform
			{
				get { return base.Platform as DebuggablePlatform; }
				protected set { base.Platform = value; }
			}

			#region Menus and Toolbars

			/// <summary>
			/// Updates Debug Menus and Toolbars according to the specified debugger state.
			/// </summary>
			/// <param name="state">The debugger state.</param>
			protected virtual void UpdateDebugMenusAndToolbars(DebuggerController.States state)
			{
				DebugToolStripMenuItem.Enabled = DebugToolStripButton.Enabled =
					state != DebuggerController.States.Running;

				PauseToolStripMenuItem.Enabled = PauseToolStripButton.Enabled =
					state == DebuggerController.States.Running;

				StopToolStripMenuItem.Enabled = StopToolStripButton.Enabled =
					state != DebuggerController.States.NotLoaded;

				RestartToolStripMenuItem.Enabled = RestartToolStripButton.Enabled =
					state != DebuggerController.States.NotLoaded;

				StepIntoToolStripMenuItem.Enabled =
					state == DebuggerController.States.NotLoaded ||
					state == DebuggerController.States.Suspended;

				bool suspended = state == DebuggerController.States.Suspended;
				StepOverToolStripMenuItem.Enabled = suspended;
				StepOutToolStripMenuItem.Enabled = suspended;
				StepBackIntoToolStripMenuItem.Enabled = suspended;
				StepBackOverToolStripMenuItem.Enabled = suspended;
				StepBackOutToolStripMenuItem.Enabled = suspended;
			}

			#region Menu
			public ToolStripMenuItem DebugToolStripMenuItem { get; protected set; }
			public ToolStripMenuItem PauseToolStripMenuItem { get; protected set; }
			public ToolStripMenuItem StopToolStripMenuItem { get; protected set; }
			public ToolStripMenuItem RestartToolStripMenuItem { get; protected set; }

			public ToolStripMenuItem StepIntoToolStripMenuItem { get; protected set; }
			public ToolStripMenuItem StepOverToolStripMenuItem { get; protected set; }
			public ToolStripMenuItem StepOutToolStripMenuItem { get; protected set; }

			public ToolStripMenuItem StepBackIntoToolStripMenuItem { get; protected set; }
			public ToolStripMenuItem StepBackOverToolStripMenuItem { get; protected set; }
			public ToolStripMenuItem StepBackOutToolStripMenuItem { get; protected set; }

			public ToolStripMenuItem ToggleBreakpointToolStripMenuItem { get; protected set; }

			/// <summary>
			/// Creates Debug menu and its items.
			/// </summary>
			/// <returns>The Debug menu.</returns>
			protected virtual ToolStripMenuItem CreateDebugMenu()
			{
				var debug = new ToolStripMenuItem("&Debug");

				DebugToolStripMenuItem = new ToolStripMenuItem("&Debug", MessyLab.Properties.Resources.Debug);
				DebugToolStripMenuItem.ShortcutKeys = Keys.F5;
				DebugToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.Run();
				debug.DropDownItems.Add(DebugToolStripMenuItem);

				PauseToolStripMenuItem = new ToolStripMenuItem("&Pause", MessyLab.Properties.Resources.Pause);
				PauseToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Alt | Keys.Pause;
				PauseToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.Pause();
				debug.DropDownItems.Add(PauseToolStripMenuItem);

				StopToolStripMenuItem = new ToolStripMenuItem("&Stop", MessyLab.Properties.Resources.Stop);
				StopToolStripMenuItem.ShortcutKeys = Keys.Shift | Keys.F5;
				StopToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.Stop();
				debug.DropDownItems.Add(StopToolStripMenuItem);

				RestartToolStripMenuItem = new ToolStripMenuItem("Res&tart", MessyLab.Properties.Resources.Restart);
				RestartToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F5;
				RestartToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.Restart();
				debug.DropDownItems.Add(RestartToolStripMenuItem);

				debug.DropDownItems.Add(new ToolStripSeparator());

				StepIntoToolStripMenuItem = new ToolStripMenuItem("Step &Into", MessyLab.Properties.Resources.StepInto);
				StepIntoToolStripMenuItem.ShortcutKeys = Keys.F11;
				StepIntoToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.StepInto(false);
				debug.DropDownItems.Add(StepIntoToolStripMenuItem);

				StepOverToolStripMenuItem = new ToolStripMenuItem("Step &Over", MessyLab.Properties.Resources.StepOver);
				StepOverToolStripMenuItem.ShortcutKeys = Keys.F10;
				StepOverToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.StepOver(false);
				debug.DropDownItems.Add(StepOverToolStripMenuItem);

				StepOutToolStripMenuItem = new ToolStripMenuItem("Step O&ut", MessyLab.Properties.Resources.StepOut);
				StepOutToolStripMenuItem.ShortcutKeys = Keys.Shift | Keys.F11;
				StepOutToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.StepOut(false);
				debug.DropDownItems.Add(StepOutToolStripMenuItem);

				if (Platform.DebuggerController.IsBackwardsExecutionSupported)
				{
					debug.DropDownItems.Add(new ToolStripSeparator());

					StepBackIntoToolStripMenuItem = new ToolStripMenuItem("Step Back I&nto", MessyLab.Properties.Resources.StepBackInto);
					StepBackIntoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F11;
					StepBackIntoToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.StepInto(true);
					debug.DropDownItems.Add(StepBackIntoToolStripMenuItem);

					StepBackOverToolStripMenuItem = new ToolStripMenuItem("Step Back O&ver", MessyLab.Properties.Resources.StepBackOver);
					StepBackOverToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F10;
					StepBackOverToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.StepOver(true);
					debug.DropDownItems.Add(StepBackOverToolStripMenuItem);

					StepBackOutToolStripMenuItem = new ToolStripMenuItem("Step Back Ou&t", MessyLab.Properties.Resources.StepBackOut);
					StepBackOutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F11;
					StepBackOutToolStripMenuItem.Click += (sender, e) => Platform.DebuggerController.StepOut(true);
					debug.DropDownItems.Add(StepBackOutToolStripMenuItem);
				}

				debug.DropDownItems.Add(new ToolStripSeparator());

				ToggleBreakpointToolStripMenuItem = new ToolStripMenuItem("Toggle &Breakpoint");
				ToggleBreakpointToolStripMenuItem.ShortcutKeys = Keys.F9;
				debug.DropDownItems.Add(ToggleBreakpointToolStripMenuItem);

				return debug;
			}

			/// <summary>
			/// Creates View menu and its items.
			/// </summary>
			/// <returns>The View menu.</returns>
			protected override ToolStripMenuItem CreateViewMenu()
			{
				var view = base.CreateViewMenu();
				view.DropDownItems.Add(new ToolStripSeparator());
				view.DropDownItems.Add(WatchPad.MenuItem);
				view.DropDownItems.Add(HardwarePad.MenuItem);
				view.DropDownItems.Add(BreakpointsPad.MenuItem);
				return view;
			}

			/// <summary>
			/// Creates the menu items and populates the MenuItems list.
			/// </summary>
			protected override void CreateMenuItems()
			{
				base.CreateMenuItems();
				MenuItems.Add(CreateDebugMenu());
			}
			#endregion

			#region Toolbar
			public ToolStripButton DebugToolStripButton { get; protected set; }
			public ToolStripButton PauseToolStripButton { get; protected set; }
			public ToolStripButton StopToolStripButton { get; protected set; }
			public ToolStripButton RestartToolStripButton { get; protected set; }

			/// <summary>
			/// Creates the Run toolbar items and populates the ToolbarItems list.
			/// </summary>
			protected override void CreateRunToolbar()
			{
				DebugToolStripButton = new ToolStripButton("Debug", MessyLab.Properties.Resources.Debug);
				DebugToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
				DebugToolStripButton.Click += (sender, e) => Platform.DebuggerController.Run();
				ToolbarItems.Add(DebugToolStripButton);

				// Create the original Run button.
				base.CreateRunToolbar();

				PauseToolStripButton = new ToolStripButton("Pause", MessyLab.Properties.Resources.Pause);
				PauseToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
				PauseToolStripButton.Click += (sender, e) => Platform.DebuggerController.Pause();
				ToolbarItems.Add(PauseToolStripButton);

				StopToolStripButton = new ToolStripButton("Stop", MessyLab.Properties.Resources.Stop);
				StopToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
				StopToolStripButton.Click += (sender, e) => Platform.DebuggerController.Stop();
				ToolbarItems.Add(StopToolStripButton);

				RestartToolStripButton = new ToolStripButton("Restart", MessyLab.Properties.Resources.Restart);
				RestartToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
				RestartToolStripButton.Click += (sender, e) => Platform.DebuggerController.Restart();
				ToolbarItems.Add(RestartToolStripButton);
			}

			/// <summary>
			/// Creates the View toolbar items and populates the ToolbarItems list.
			/// </summary>
			protected override void CreateViewToolbar()
			{
				base.CreateViewToolbar();
				ToolbarItems.Add(WatchPad.ToolbarItem);
				ToolbarItems.Add(HardwarePad.ToolbarItem);
				ToolbarItems.Add(BreakpointsPad.ToolbarItem);
			}
			#endregion

			#endregion

			#region Pads
			public WatchPad WatchPad { get; protected set; }
			public BreakpointsPad BreakpointsPad { get; protected set; }
			public HardwarePad HardwarePad { get; protected set; }

			#region Pad Save/Restore
			
			/// <summary>
			/// Open pads on their default positions.
			/// </summary>
			protected override void OpenDefaultPads()
			{
				base.OpenDefaultPads();
				BreakpointsPad.ShowOnMainForm();
				HardwarePad.ShowOnMainForm();
				WatchPad.ShowOnMainForm();
			}

			/// <summary>
			/// Gets the Pad by the type name.
			/// </summary>
			/// <param name="typeName">Type name of the pad.</param>
			/// <returns>The pad for the specified name or null if the name is invalid.</returns>
			protected override Pad GetPadByTypeName(string typeName)
			{
				switch (typeName)
				{
					case "MessyLab.BreakpointsPad":
						return BreakpointsPad;
					case "MessyLab.HardwarePad":
						return HardwarePad;
					case "MessyLab.WatchPad":
						return WatchPad;
					default:
						return base.GetPadByTypeName(typeName);
				}
			}

			#region Watch
			/// <summary>
			/// Saves the content of the Watch pad.
			/// </summary>
			public virtual void SaveWatch()
			{
				var memento = WatchPad.CreateMemento();
				string path = Path.Combine(Platform.Project.Directory, "Watch.xml");
				XmlSerializer s = new XmlSerializer(memento.GetType());
				TextWriter w = new StreamWriter(path);
				s.Serialize(w, memento);
				w.Close();
			}

			/// <summary>
			/// Loads the content of the Watch pad.
			/// </summary>
			public virtual void LoadWatch()
			{
				string path = Path.Combine(Platform.Project.Directory, "Watch.xml");
				if (File.Exists(path))
				{
					XmlSerializer s = new XmlSerializer(typeof(WatchPad.WatchMemento));
					TextReader r = new StreamReader(path);
					var memento = s.Deserialize(r) as WatchPad.WatchMemento;
					r.Close();

					WatchPad.SetMemento(memento);
				}
			}
			#endregion

			#endregion

			/// <summary>
			/// Creates, initializes pads and populates the Pads list.
			/// </summary>
			protected override void CreatePads()
			{
				base.CreatePads();

				WatchPad = new WatchPad(Platform.Project);
				WatchPad.WatchAdded += new Action<WatchPad.WatchItem>(UpdateWatchItem);
				WatchPad.WatchValueEdited += new Action<WatchPad.WatchItem, long>(EditWatchValue);
				Pads.Add(WatchPad);

				BreakpointsPad = new BreakpointsPad(Platform.Project);
				BreakpointsPad.BreakpointController = Platform.DebuggerController.BreakpointController;
				Pads.Add(BreakpointsPad);

				HardwarePad = new HardwarePad(Platform.Project);
				Pads.Add(HardwarePad);
			}

			#region Pad Updates

			/// <summary>
			/// Updates the content of the specified Watch item.
			/// </summary>
			/// <param name="item">The Watch item to update.</param>
			protected abstract void UpdateWatchItem(MessyLab.WatchPad.WatchItem item);
			
			/// <summary>
			/// Sets the specified value to both the item and the debugger.
			/// </summary>
			/// <param name="item">The Watch item to edit.</param>
			/// <param name="value">The value to set.</param>
			protected abstract void EditWatchValue(MessyLab.WatchPad.WatchItem item, long value);

			/// <summary>
			/// Updates the content of the Watch pad according to the specified debugger state.
			/// </summary>
			/// <param name="state">The debugger state.</param>
			protected virtual void UpdateWatchPad(DebuggerController.States state)
			{
				WatchPad.CanEdit = state == DebuggerController.States.Suspended;
				foreach (var item in WatchPad.WatchItems)
				{
					UpdateWatchItem(item);
				}
			}

			/// <summary>
			/// Updates the content of the Breakpoints pad according to the specified debugger state.
			/// </summary>
			/// <param name="state">The debugger state.</param>
			protected virtual void UpdateBreakpointsPad(DebuggerController.States state)
			{
				if (state == DebuggerController.States.Suspended)
				{
					var reason = Platform.DebuggerController.ExecutionInterruptReason;
					if (reason != null && reason.HitBreakpoints != null)
					{
						BreakpointsPad.HighlightBreakpoints(reason.HitBreakpoints);
					}
				}
			}

			/// <summary>
			/// Updates the content of the Hardware pad according to the specified debugger state.
			/// </summary>
			/// <param name="state">The debugger state.</param>
			protected abstract void UpdateHardwarePad(DebuggerController.States state);
			#endregion

			#endregion

			/// <summary>
			/// Checks if the debugger has thrown an exception and handles it.
			/// </summary>
			protected void HandleDebuggerException()
			{
				var dc = Platform.DebuggerController;
				var reason = dc.ExecutionInterruptReason;

				if (reason != null && reason.Exception != null)
				{
					var res = MessageBox.Show(reason.Exception.Message + "\n\nWould you like to continue debugging?",
						"Exception occured.", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
					if (res == DialogResult.No)
					{
						dc.Stop();
					}
				}
			}

			/// <summary>
			/// Updates GUI elements according to the specified debugger state.
			/// </summary>
			/// <param name="state">The debugger state.</param>
			public virtual void Update(DebuggerController.States state)
			{
				UpdateDebugMenusAndToolbars(state);
				UpdateWatchPad(state);
				UpdateHardwarePad(state);
				UpdateBreakpointsPad(state);

				// Debugger Exception handling.
				if (state == DebuggerController.States.Suspended)
				{
					HandleDebuggerException();
				}
			}
		}
		#endregion

		#region Editor
		/// <summary>
		/// Provides and manages editors for project items with debugging features.
		/// </summary>
		public abstract class DebuggableEditorProvider : EditorProvider
		{
			public DebuggableEditorProvider(DebuggablePlatform platform) : base(platform) { }

			/// <summary>
			/// Parent platform redefined as DebuggablePlatform for convenience.
			/// </summary>
			public new DebuggablePlatform Platform
			{
				get { return base.Platform as DebuggablePlatform; }
				protected set { base.Platform = value; }
			}

			/// <summary>
			/// Initializes the specified editor form.
			/// </summary>
			/// <param name="form">The editor form to initialize.</param>
			public override void InitializeEditor(AbstractEditorForm form)
			{
				base.InitializeEditor(form);

				// Set breakpoints.
				var l = new List<int>();
				foreach (var b in Platform.DebuggerController.BreakpointController.Breakpoints)
				{
					var bp = b as BreakpointController.LineBreakpointDefinition;
					if (bp != null && bp.ProjectItem == form.Item)
					{
						l.Add(bp.Line);
					}
				}
				form.Breakpoints = l;

				// Connect breakpoint events.
				form.BreakpointAdded += new AbstractEditorForm.BreakpointToggledHandler(Platform.DebuggerController.BreakpointController.AddLineBreakpoint);
				form.BreadpointRemoved += new AbstractEditorForm.BreakpointToggledHandler(Platform.DebuggerController.BreakpointController.RemoveLineBreakpoint);
			}

			#region Highlight of the current line
			/// <summary>
			/// Highlights the specified line in the specified project item.
			/// </summary>
			/// <param name="itemName">The name of the file containing the line to highlight.</param>
			/// <param name="line">The line to highlight.</param>
			public void HighlightLine(ProjectItem item, int line)
			{
				if (item == null) throw new ArgumentNullException("item");
				
				ClearHighlightedLines();

				Platform.Gui.MainForm.ShowEditorFor(item);
				var editor = GetEditorForm(item);
				editor.HighlightLine(line);
			}

			/// <summary>
			/// Highlights the specified line in the specified file.
			/// </summary>
			/// <param name="itemName">The name of the file containing the line to highlight.</param>
			/// <param name="line">The line to highlight.</param>
			public void HighlightLine(string itemName, int line)
			{
				var items = from item in Platform.Project.Items
							where item.Filename == itemName
							select item;

				foreach (var item in items)
				{
					HighlightLine(item, line);

					// Only one matching item is expected.
					break;
				}
			}

			/// <summary>
			/// Clears Highlighted lines in all editors.
			/// </summary>
			public void ClearHighlightedLines()
			{
				foreach (var editor in EditorForms.Values)
				{
					// Clear highlight.
					editor.HighlightLine(-1);
				}
			}
			#endregion

			/// <summary>
			/// Updates editors according to the specified debugger state.
			/// </summary>
			/// <param name="state">The debugger state.</param>
			public virtual void Update(DebuggerController.States state)
			{
				switch (state)
				{
					case DebuggerController.States.NotLoaded:
						EditorsReadOnly = false;
						ClearHighlightedLines();
						break;
					case DebuggerController.States.Suspended:
						EditorsReadOnly = true;
						var dc = Platform.DebuggerController;
						HighlightLine(dc.HighlightedFile, dc.HighlightedLine);
						break;
					case DebuggerController.States.Running:
						EditorsReadOnly = true;
						ClearHighlightedLines();
						break;
				}
			}
		}
		#endregion

		public DebuggablePlatform(Project project, MainForm mainForm) : base(project, mainForm) { }

		public abstract DebuggerController DebuggerController { get; }

		/// <summary>
		/// Initializes the Platform.
		/// </summary>
		public override void Initialize()
		{
			// We initialize DebuggerController first, because BreakpointPad initialization uses it.
			DebuggerController.Initialize();

			// Initializes Gui elements.
			base.Initialize();

			// We set the State updating event.
			DebuggerController.StateChanged += new DebuggerController.StateChangedHandler(DebuggerController_StateChanged);
			DebuggerController_StateChanged(DebuggerController.State);
		}

		/// <summary>
		/// Handles the debugger state change.
		/// </summary>
		/// <param name="state"></param>
		protected void DebuggerController_StateChanged(DebuggerController.States state)
		{
			var editors = Editors as DebuggableEditorProvider;
			editors.Update(state);
			var gui = Gui as DebuggableGuiProvider;
			gui.Update(state);

			// Note: Using a reverse order, i.e. GUI, then Editors would cause
			// the editor not the highlight the current line at the prompt
			// displayed by the GUI update on exception.
			// It would not be a bug per se, but it would be an inconvenience
			// for the user..
		}

		#region Settings
		/// <summary>
		/// Saves breakpoint list to a file.
		/// </summary>
		public virtual void SaveBreakpoints()
		{
			var memento = DebuggerController.BreakpointController.CreateMemento();
			string path = Path.Combine(Project.Directory, "Breakpoints.xml");
			XmlSerializer s = new XmlSerializer(memento.GetType());
			TextWriter w = new StreamWriter(path);
			s.Serialize(w, memento);
			w.Close();
		}

		/// <summary>
		/// Loads breakpoint list from a file.
		/// </summary>
		public virtual void LoadBreakpoints()
		{
			string path = Path.Combine(Project.Directory, "Breakpoints.xml");
			if (File.Exists(path))
			{
				XmlSerializer s = new XmlSerializer(typeof(BreakpointController.BreakpointMemento));
				TextReader r = new StreamReader(path);
				var memento = s.Deserialize(r) as BreakpointController.BreakpointMemento;
				r.Close();

				DebuggerController.BreakpointController.SetMemento(memento);

				(Gui as DebuggableGuiProvider).BreakpointsPad.UpdateList();
			}
		}

		/// <summary>
		/// Saves the platform specific settings.
		/// </summary>
		public override void SaveSettings()
		{
			base.SaveSettings();
			var gui = Gui as DebuggableGuiProvider;
			gui.SaveWatch();
			SaveBreakpoints();
		}

		/// <summary>
		/// Loads the platform specidic settings.
		/// </summary>
		public override void LoadSettings()
		{
			base.LoadSettings();
			var gui = Gui as DebuggableGuiProvider;
			gui.LoadWatch();
			LoadBreakpoints();
		}
		#endregion
	}
}
