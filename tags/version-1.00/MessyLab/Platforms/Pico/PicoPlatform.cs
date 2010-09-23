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
using System.Diagnostics;
using System.Windows.Forms;
using MessyLab.Properties;

namespace MessyLab.Platforms.Pico
{
	/// <summary>
	/// The picoComputer platform.
	/// </summary>
	public class PicoPlatform : DebuggablePlatform
	{
		#region GUI
		/// <summary>
		/// Provides and manager picoComputer-specific GUI elements.
		/// </summary>
		public class PicoGuiProvider : DebuggableGuiProvider
		{
			public PicoGuiProvider(PicoPlatform platform) : base(platform) { }

			#region Event Handlers
			/// <summary>
			/// Handles the Add Existing Project Item command click.
			/// </summary>
			protected override void AddExistingProjectItem()
			{
				string path;
				bool ok = MainForm.ShowOpenDialog("Add Existing File...", "picoComputer Source Files (*.pca)|*.pca", ".pca", Platform.Project.Path, out path);
				if (ok)
				{
					AddExistingProjectItem(path);
				}
			}

			/// <summary>
			/// Handles the Add New Project Item command click.
			/// </summary>
			protected override void AddNewProjectItem()
			{
				using (var f = new NewProjectItemForm())
				{
					f.SetTypes(new string[] { "picoComputer Source File" });
					if (f.ShowDialog() == DialogResult.OK)
					{
						AddNewProjectItem(f.ItemName);
					}
				}
			}

			/// <summary>
			/// Handles the Show Project Properties command click.
			/// </summary>
			protected override void ShowProjectProperties()
			{
				using (var f = new PicoPropertiesForm())
				{
					f.ShowDialog();
				}
			}
			#endregion

			#region Menus and Toolbars
			/// <summary>
			/// Creates View menu and its items.
			/// </summary>
			/// <returns>The View menu.</returns>
			protected override ToolStripMenuItem CreateViewMenu()
			{
				var view = base.CreateViewMenu();
				view.DropDownItems.Add(new ToolStripSeparator());
				view.DropDownItems.Add(ConsolePad.MenuItem);
				return view;
			}

			/// <summary>
			/// Creates the View toolbar items and populates the ToolbarItems list.
			/// </summary>
			protected override void CreateViewToolbar()
			{
				base.CreateViewToolbar();
				ToolbarItems.Add(ConsolePad.ToolbarItem);
			}
			#endregion

			#region Pads
			public ConsolePad ConsolePad { get; protected set; }

			#region Pad Save/Restore
			/// <summary>
			/// Open pads on their default positions.
			/// </summary>
			protected override void OpenDefaultPads()
			{
				base.OpenDefaultPads();
				ConsolePad.ShowOnMainForm();
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
					case "MessyLab.Platforms.Pico.ConsolePad":
						return ConsolePad;
					default:
						return base.GetPadByTypeName(typeName);
				}
			}
			#endregion

			/// <summary>
			/// Creates, initializes pads and populates the Pads list.
			/// </summary>
			protected override void CreatePads()
			{
				base.CreatePads();
				WatchPad.Types = new string[] { "Signed", "Unsigned" };

				(BreakpointsPad.NewIOBreakpointDialog as NewIOBreakpointForm).SetAddress(0, false);
				var newRegBp = BreakpointsPad.NewRegisterBreakpointDialog as NewRegisterBreakpointForm;
				newRegBp.SetRegisterNames(new string[] { "SP", "PC" });
				newRegBp.SetOnRead(false, false);
				newRegBp.SetOnWrite(true, true);

				ConsolePad = new ConsolePad(Platform.Project);
				var dc = Platform.DebuggerController as PicoDebuggerController;
				dc.IODevice = ConsolePad;
				Pads.Add(ConsolePad);
			}

			#region Pad Updates

			#region WatchPad Update

			#region Helper Private methods
			/// <summary>
			/// Returns the variable name and a value indicating whether the
			/// addressing is indirect.
			/// </summary>
			/// <remarks>
			/// If found, parentheses are removed to get the variable name.
			/// Parentheses indicate the indirect addressing.
			/// </remarks>
			/// <param name="variable">The variable to decode.</param>
			/// <param name="name">The name of the variable.</param>
			/// <param name="indirect">A value indicating whether the addressing is indirect.</param>
			private static void DecodeVariable(string variable, out string name, out bool indirect)
			{
				if (variable.StartsWith("(") && variable.EndsWith(")"))
				{
					name = variable.Replace("(", string.Empty).Replace(")", string.Empty);
					indirect = true;
				}
				else
				{
					name = variable;
					indirect = false;
				}
			}

			/// <summary>
			/// Gets the address of for the specified variable.
			/// </summary>
			/// <param name="variable">The variable to get the address for.</param>
			/// <param name="address">The address.</param>
			/// <returns>A value indicating if the operation was successful.</returns>
			private bool GetAddress(string variable, out ushort address)
			{
				var dbg = Platform.DebuggerController.Debugger;

				bool indirect;
				string name;
				DecodeVariable(variable, out name, out indirect);

				try
				{
					address = ushort.Parse(name);
				}
				catch
				{
					// Symbol.
					try
					{ address = (ushort)dbg.DebugInformation.ByName[name.ToLower()].Value; }
					catch
					{
						// Not found.
						address = 0;
						return false;
					}
				}
				if (indirect)
				{
					address = (ushort)dbg.Target.Memory[address];
				}
				return true;
			}

			/// <summary>
			/// Updates the watch item value according to the watch type.
			/// </summary>
			/// <param name="item">The watch item to update.</param>
			/// <param name="value">The value to set.</param>
			private static void UpdateItemValue(WatchPad.WatchItem item, long value)
			{
				if (item.Type == "Signed")
				{ item.Value = ((short)value).ToString(); }
				else
				{ item.Value = ((ushort)value).ToString(); }
			}
			#endregion

			/// <summary>
			/// Updates the value of the specified watch item.
			/// </summary>
			/// <param name="item">The watch item to update.</param>
			protected override void UpdateWatchItem(WatchPad.WatchItem item)
			{
				if (Platform.DebuggerController.State == DebuggerController.States.Suspended)
				{
					ushort address;
					if (!GetAddress(item.Name, out address))
					{
						item.Value = "?";
						return;
					}

					long val = Platform.DebuggerController.Debugger.Target.Memory[address];

					UpdateItemValue(item, val);
				}
				else
				{
					item.Value = "?";
				}
			}

			/// <summary>
			/// Edits the specified watch item and the corresponding memory location
			/// by storing the specified value.
			/// </summary>
			/// <param name="item">The watch to edit.</param>
			/// <param name="value">The value to store.</param>
			protected override void EditWatchValue(WatchPad.WatchItem item, long value)
			{
				if (Platform.DebuggerController.State != DebuggerController.States.Suspended) return;

				ushort address;
				if (!GetAddress(item.Name, out address)) return;

				Platform.DebuggerController.Debugger.Target.Memory[address] = value;

				UpdateItemValue(item, value);
			}
			#endregion

			#region HardwarePad Update
			/// <summary>
			/// Updates the Hardware pad if the specified debugger state is Suspended.
			/// </summary>
			/// <param name="state">The debugger controller state.</param>
			protected override void UpdateHardwarePad(DebuggerController.States state)
			{
				var c = HardwarePad.Content;
				if (state == DebuggerController.States.Suspended)
				{
					c["Registers"]["PC"] = Platform.DebuggerController.Debugger.Target.Registers["PC"].ToString();
					c["Registers"]["SP"] = Platform.DebuggerController.Debugger.Target.Registers["SP"].ToString();
					var target = Platform.DebuggerController.Debugger.Target as MessyLab.Debugger.Target.Pico.PicoTarget;
					string asm;
					string bin;
					target.VirtualMachine.Disassembler.Disassemble(out asm, out bin);
					c["Current Instruction"]["Symbolic"] = asm;
					if (bin.Contains("  "))
					{
						string[] b = bin.Split(new string[] { "  " }, StringSplitOptions.None);
						c["Current Instruction"]["Instruction"] = b[0];
						c["Current Instruction"]["Constant"] = b[1];
					}
					else
					{
						c["Current Instruction"]["Instruction"] = bin;
						c["Current Instruction"]["Constant"] = string.Empty;
					}
				}
				else
				{
					c["Registers"]["PC"] = "?";
					c["Registers"]["SP"] = "?";
					c["Current Instruction"]["Symbolic"] = "?";
					c["Current Instruction"]["Instruction"] = "?";
					c["Current Instruction"]["Constant"] = "?";
				}
			}
			#endregion

			#endregion

			#endregion
		}
		#endregion

		#region Editor
		/// <summary>
		/// Provides and manages editors for the picoComputer project items.
		/// </summary>
		public class PicoEditorProvider : DebuggableEditorProvider
		{
			public PicoEditorProvider(PicoPlatform platform) : base(platform) { }

			/// <summary>
			/// Creates an item specific editor form.
			/// </summary>
			/// <param name="item">The project item to create an editor for.</param>
			/// <returns>The created editor form.</returns>
			protected override AbstractEditorForm CreateEditorForm(ProjectItem item)
			{
				var ed = new PicoTextEditorForm();
				ed.Open(item);
				ed.ErrorsClick += (sender, e) => Platform.Gui.ErrorsPad.ShowOnMainForm();
				return ed;
			}
		}
		#endregion

		public PicoPlatform(Project project, MainForm mainForm) : base(project, mainForm)
		{
			_gui = new PicoGuiProvider(this);
			_editors = new PicoEditorProvider(this);
			_debuggerController = new PicoDebuggerController(this);
		}

		public override string Name { get { return "picoComputer"; } }

		private PicoGuiProvider _gui;
		private PicoEditorProvider _editors;
		private PicoDebuggerController _debuggerController;

		public override Platform.GuiProvider Gui { get { return _gui; } }
		public PicoGuiProvider PicoGui { get { return _gui; } }

		public override Platform.EditorProvider Editors { get { return _editors; } }

		public override DebuggerController DebuggerController { get { return _debuggerController; } }

		#region Project Management
		private PicoProjectItemFactory _itemFactory = new PicoProjectItemFactory();
		public override ProjectItemAbstractFactory ProjectItemFactory
		{
			get { return _itemFactory; }
		}
		#endregion

		#region Build/Run
		/// <summary>
		/// Builds the current project.
		/// </summary>
		/// <param name="binaryPath">A variable to store the path to the generated binary.</param>
		/// <returns>A value indicating whether the build was successful.</returns>
		protected internal override bool Build(out string binaryPath)
		{
			Project.Save();
			Assembler a = new Assembler();
			a.LoadFromFile(Project.MainItem.Path);
			if (a.Process())
			{
				string path = Path.Combine(Path.GetDirectoryName(Project.Path), Path.GetFileNameWithoutExtension(Project.Filename));
				a.SaveAsBinary(path + ".bin");
				if (Settings.Default.Pico_GenerateHex) a.SaveAsHex(path + ".hex");
				if (Settings.Default.Pico_GenerateTxt) a.SaveAsText(path + ".txt");
				a.DebugInformation.SaveToFile(path + ".bin.mldbg");
				binaryPath = path + ".bin";
				return true;
			}
			else
			{
				// Report errors.
				Gui.ErrorsPad.ClearItems();
				foreach (var error in a.Errors)
				{
					string desc = string.Format("E{0:0000}: {1}.", error.ID, error.Description);
					Gui.ErrorsPad.AddItem(new ListItem(desc, Project.MainItem, error.Line, error.Column));
				}
				Gui.ErrorsPad.ShowOnMainForm();
				binaryPath = string.Empty;
				return false;
			}
		}

		/// <summary>
		/// Runs the current project without debugging.
		/// </summary>
		protected override void Run()
		{
			string path;
			if (Build(out path))
			{
				string vmExe = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "PicoVM.exe");
				path = "\"" + path + "\"";
				Process.Start(vmExe, path);
			}
		}
		#endregion

		#region Platform Directory
		public override string PlatformSubDir
		{
			get { return "Pico"; }
		}
		#endregion
	}
}
