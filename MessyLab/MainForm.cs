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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MessyLab.Platforms;
using MessyLab.Properties;

namespace MessyLab
{
	/// <summary>
	/// The main form of Messy Lab.
	/// </summary>
	public sealed partial class MainForm : Form
	{
		/// <summary>
		/// Dock Panel used to access open pads.
		/// </summary>
		public DockPanel DockPanel { get { return dock; } }

		/// <summary>
		/// Shows the specified pad in the dock if it is not
		/// already shown; otherwise activates it.
		/// </summary>
		/// <param name="pad"></param>
		public void ShowPad(Pad pad)
		{
			if (pad.Visible) { pad.Activate(); }
			else { pad.Show(dock); }
		}

		/// <summary>
		/// Currently active editor form.
		/// </summary>
		public AbstractEditorForm ActiveEditorForm { get; set; }

		/// <summary>
		/// Currently open project.
		/// </summary>
		private Project OpenedProject;

		/// <summary>
		/// Shows the editor for the specified ProjectItem.
		/// </summary>
		/// <param name="item">The item to show the editor for.</param>
		public void ShowEditorFor(ProjectItem item)
		{
			if (item == null) return;

			var e = item.GetEditorForm();
			if (e.Visible)
			{
				e.Activate();
			}
			else
			{
				e.Show(dock, DockState.Document);
			}
		}

		#region Dynamic Menus and Toolbars

		#region Menu Enabler
		/// <summary>
		/// Updates menu items periodically.
		/// </summary>
		private void menuUpdateTimer_Tick(object sender, EventArgs e)
		{
			UpdateEditorToolStripItems();
			UpdateProjectToolStripItems();
		}

		/// <summary>
		/// Updates project related menu items.
		/// </summary>
		public void UpdateProjectToolStripItems()
		{
			bool notNull = OpenedProject != null;
			closeProjectToolStripMenuItem.Enabled = notNull;
			saveAllToolStripMenuItem.Enabled = saveAllToolStripButton.Enabled = notNull;
		}

		/// <summary>
		/// Updates editor related menu items.
		/// </summary>
		private void UpdateEditorToolStripItems()
		{
			bool undo = false;
			bool redo = false;
			bool cut = false;
			bool copy = false;
			bool paste = false;
			bool selectAll = false;
			bool find = false;
			bool goTo = false;
			bool advancedText = false;
			bool toggleBreakpoints = false;

			bool notNull = false;
			if (ActiveEditorForm != null)
			{
				notNull = true;
				undo = ActiveEditorForm.CanUndo;
				redo = ActiveEditorForm.CanRedo;
				cut = ActiveEditorForm.CanCut;
				copy = ActiveEditorForm.CanCopy;
				paste = ActiveEditorForm.CanPaste;
				selectAll = ActiveEditorForm.CanSelectAll;
				find = ActiveEditorForm.CanFindReplace;
				goTo = ActiveEditorForm.CanGoTo;
				advancedText = ActiveEditorForm.CanDoAdvancedTextEditing;
				toggleBreakpoints = ActiveEditorForm.AllowBreakpoints;
			}
			undoToolStripMenuItem.Enabled = undoToolStripButton.Enabled = undo;
			redoToolStripMenuItem.Enabled = redoToolStripButton.Enabled = redo;

			cutToolStripMenuItem.Enabled = cutToolStripButton.Enabled = cut;
			copyToolStripMenuItem.Enabled = copyToolStripButton.Enabled = copy;
			pasteToolStripMenuItem.Enabled = pasteToolStripButton.Enabled = paste;

			selectAllToolStripMenuItem.Enabled = selectAll;

			findToolStripMenuItem.Enabled = findNextToolStripMenuItem.Enabled = find;
			replaceToolStripMenuItem.Enabled = find;

			goToToolStripMenuItem.Enabled = goTo;

			commentSelectionToolStripMenuItem.Enabled = advancedText;
			uncommentSelectionToolStripMenuItem.Enabled = advancedText;
			increaseLindIndentToolStripMenuItem.Enabled = advancedText;
			decreaseLineIndentToolStripMenuItem.Enabled = advancedText;
			makeLowercaseToolStripMenuItem.Enabled = advancedText;
			makeUppercaseToolStripMenuItem.Enabled = advancedText;

			if (_toggleBreakpointToolStripMenuItem != null)
			{
				_toggleBreakpointToolStripMenuItem.Enabled = toggleBreakpoints;
			}

			printToolStripMenuItem.Enabled = notNull;
			saveToolStripMenuItem.Enabled = saveToolStripButton.Enabled = notNull;
			closeToolStripMenuItem.Enabled = notNull;
		}
		#endregion

		private ToolStripMenuItem _toggleBreakpointToolStripMenuItem;
		public ToolStripMenuItem ToggleBreakpointToolStripMenuItem
		{
			get { return _toggleBreakpointToolStripMenuItem; }
			set
			{
				if (_toggleBreakpointToolStripMenuItem != null)
				{
					try { _toggleBreakpointToolStripMenuItem.Click -= ToggleBreakpointToolStripMenuItem_Click; } catch { }
				}
				_toggleBreakpointToolStripMenuItem = value;
				if (value != null)
				{
					_toggleBreakpointToolStripMenuItem.Click += new EventHandler(ToggleBreakpointToolStripMenuItem_Click);
				}
			}
		}

		/// <summary>
		/// Removes project specific menu items.
		/// </summary>
		private void RemoveMenu()
		{
			// The first to items are File and Edit, and the last one (Help) has the Tag set to "last"
			// in the editor.
			while (menuStrip.Items[2].Tag == null || menuStrip.Items[2].Tag.ToString() != "last")
			{
				menuStrip.Items.RemoveAt(2);
			}
			ToggleBreakpointToolStripMenuItem = null;
		}

		/// <summary>
		/// Removes project specific toolbar items.
		/// </summary>
		private void RemoveToolbar()
		{
			// The first 11 items are not dynamic.
			while (toolStrip.Items.Count > 11)
			{
				toolStrip.Items.RemoveAt(11);
			}
		}

		/// <summary>
		/// Adds project specific menu items.
		/// </summary>
		/// <param name="platformGui">Gui object containing the items.</param>
		public void AddMenu(Platform.GuiProvider platformGui)
		{
			int i = 2;
			foreach (var item in platformGui.MenuItems)
			{
				menuStrip.Items.Insert(i++, item);
			}
			var p = platformGui as DebuggablePlatform.DebuggableGuiProvider;
			if (p != null)
			{
				ToggleBreakpointToolStripMenuItem = p.ToggleBreakpointToolStripMenuItem;
			}
		}

		/// <summary>
		/// Adds project specific toolbar items.
		/// </summary>
		/// <param name="platformGui">Gui object containing the items.</param>
		public void AddToolbar(Platform.GuiProvider platformGui)
		{
			int i = 11;
			foreach (var item in platformGui.ToolbarItems)
			{
				toolStrip.Items.Insert(i++, item);
			}
		}

		#endregion

		/// <summary>
		/// Closes the currently open project.
		/// </summary>
		public void CloseProject()
		{
			if (OpenedProject == null) return;
			RemoveMenu();
			RemoveToolbar();
			Text = "Messy Lab";

			// Close pads.
			foreach (var pad in OpenedProject.Platform.Gui.Pads)
			{
				pad.HideOnClose = false;
				pad.DockHandler.Close();
				pad.Dispose();
			}

			// Close editors.
			foreach (var item in OpenedProject.Items)
			{
				var ed = OpenedProject.Platform.Editors.GetEditorFormIfExists(item);
				if (ed != null)
				{
					ed.DockHandler.Close();
					ed.Dispose();
				}
			}

			// Stop debugging on an debuggable platform.
			var pl = OpenedProject.Platform as DebuggablePlatform;
			if (pl != null)
			{
				pl.DebuggerController.Stop();
			}

			OpenedProject = null;
		}

		/// <summary>
		/// Opens the specified project.
		/// </summary>
		/// <param name="project">The project to open.</param>
		private void OpenProject(Project project)
		{
			CloseStartPage();
			CloseProject();

			OpenedProject = project;
			var pl = project.Platform;
			AddMenu(pl.Gui);
			AddToolbar(pl.Gui);

			pl.Gui.OpenPads();

			ShowEditorFor(project.MainItem);

			Text = System.IO.Path.GetFileNameWithoutExtension(project.Filename) + " - Messy Lab";

			// Notofy the RecentManager that the project has been opened.
			RecentManager.Notify(project.Path);
		}

		/// <summary>
		/// Save the open project.
		/// </summary>
		public void SaveProject()
		{
			if (OpenedProject == null) return;
			OpenedProject.Save();
		}

		#region File Actions
		/// <summary>
		/// Displays the New Project Dialog.
		/// </summary>
		public void NewProjectClick()
		{
			using (NewProjectForm f = new NewProjectForm())
			{
				if (f.ShowDialog() == DialogResult.OK)
				{
					if (CloseProjectClick())
					{
						var p = ProjectFactory.Default.CreateProject(f.Platform, f.Path);
						OpenProject(p);
					}
				}
			}
		}

		/// <summary>
		/// Opens the project from the specified path.
		/// </summary>
		/// <param name="path">Path to the project to open.</param>
		public void OpenProject(string path)
		{
			// Close currently open project with saving prompt.
			if (!CloseProjectClick()) return;

			Project p;
			try
			{
				p = new Project();
				p.Open(path);
			}
			catch (Exception e)
			{
				MessageBox.Show("Could not load selected project.\n\nException:\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			OpenProject(p);
		}

		/// <summary>
		/// Shows the Open dialog using the specified parameters.
		/// </summary>
		/// <param name="title">Dialog title.</param>
		/// <param name="filter">Filter string.</param>
		/// <param name="defaultExt">The default extension, e.g. ".mlp"</param>
		/// <param name="initialDirectory">The initial directory for the dialog.</param>
		/// <param name="path">A variable to store the selected path.</param>
		/// <returns>A value indicating whether the operation was successful.</returns>
		public bool ShowOpenDialog(string title, string filter, string defaultExt, string initialDirectory, out string path)
		{
			using (var d = new OpenFileDialog())
			{
				d.Title = title;
				d.Filter = filter;
				if (string.IsNullOrEmpty(defaultExt)) d.DefaultExt = defaultExt;
				if (string.IsNullOrEmpty(initialDirectory)) d.InitialDirectory = initialDirectory;

				d.Multiselect = false;
				d.CheckFileExists = true;

				if (d.ShowDialog() == DialogResult.OK)
				{
					path = d.FileName;
					return true;
				}
				else
				{
					path = string.Empty;
					return false;
				}
			}
		}

		/// <summary>
		/// Displays the Open Project Dialog.
		/// </summary>
		public void OpenProjectClick()
		{
			string filename;
			bool ok = ShowOpenDialog(
				"Open Project...",
				"Messy Lab projects (*.mlp)|*.mlp",
				".mlp",
				Settings.Default.ProjectRoot,
				out filename);

			if (ok)
			{
				OpenProject(filename);
			}
		}

		/// <summary>
		/// Closes the open project with a confirmation if unsaved.
		/// </summary>
		/// <returns></returns>
		private bool CloseProjectClick()
		{
			if (OpenedProject == null) return true;

			if (OpenedProject.IsModified)
			{
				var result = MessageBox.Show(string.Format("Project \"{0}\" has been modified.\n\nWould you like to save these changes?", OpenedProject.Filename),
					"Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

				if (result == DialogResult.Yes)
				{ SaveProject(); }
				else if (result == DialogResult.No)
				{ OpenedProject.Revert(); }
				else // Cancel
				{ return false; }
			}

			CloseProject();
			return true;
		}
		#endregion

		#region Recent projects
		/// <summary>
		/// Clears the recent menu.
		/// </summary>
		private void ClearRecentMenu()
		{
			var drop = recentToolStripMenuItem.DropDownItems;
			while (drop.Count > 1)
			{
				drop.RemoveAt(0);
			}
		}

		/// <summary>
		/// Updates the recent menu using the data from the RecentManager
		/// </summary>
		private void UpdateRecent()
		{
			ClearRecentMenu();
			var drop = recentToolStripMenuItem.DropDownItems;

			int i = 0;
			foreach (var file in RecentManager.Recent)
			{
				i++;
				var item = new ToolStripMenuItem(string.Format("&{0} {1}", i, file));
				item.Tag = file;
				item.Click += new EventHandler(recent_Click);
				drop.Insert(i - 1, item);
			}
			if (i > 0) drop.Insert(i, new ToolStripSeparator());
		}

		/// <summary>
		/// Opens the recent menu item specific project.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void recent_Click(object sender, EventArgs e)
		{
			var item = sender as ToolStripMenuItem;
			string file = item.Tag as string;
			OpenProject(file);
		}
		#endregion

		#region File Event Handlers
		private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewProjectClick();
		}

		private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenProjectClick();
		}

		private void newProjectToolStripButton_Click(object sender, EventArgs e)
		{
			NewProjectClick();
		}

		private void openProjectToolStripButton_Click(object sender, EventArgs e)
		{
			OpenProjectClick();
		}

		private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseProjectClick();
			OpenStartPage();
		}

		private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveProject();
		}

		private void saveAllToolStripButton_Click(object sender, EventArgs e)
		{
			SaveProject();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void recentToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			UpdateRecent();
		}

		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RecentManager.Clear();
		}
		#endregion

		#region Editor Event Handlers
		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Cut();
		}

		private void cutToolStripButton_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Copy();
		}

		private void copyToolStripButton_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Paste();
		}

		private void pasteToolStripButton_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Paste();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.SelectAll();
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Undo();
		}

		private void undoToolStripButton_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Undo();
		}

		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Redo();
		}

		private void redoToolStripButton_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Redo();
		}

		private void goToToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.GoTo();
		}

		private void makeLowercaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.SelectionToLower();
		}

		private void makeUppercaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.SelectionToUpper();
		}

		private void increaseLindIndentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Ident(true);
		}

		private void decreaseLineIndentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Ident(false);
		}

		private void commentSelectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.CommentSelection(true);
		}

		private void uncommentSelectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.CommentSelection(false);
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Find();
		}

		private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.FindNext();
		}

		private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Replace();
		}

		void ToggleBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null && ActiveEditorForm.AllowBreakpoints)
			{ ActiveEditorForm.ToggleBreakpoint(); }
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Close();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Save();
		}

		private void saveToolStripButton_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Save();
		}

		private void printToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ActiveEditorForm != null) ActiveEditorForm.Print();
		}
		#endregion

		#region Help Event Handlers
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (AboutForm f = new AboutForm())
			{
				f.ShowDialog();
			}
		}

		private void docToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.messylab.com");
		}
		#endregion

		public RecentManager RecentManager { get; private set; }

		public MainForm()
		{
			InitializeComponents();
			RecentManager = new RecentManager();
		}

		/// <summary>
		/// Keeps track of the currently active editor form.
		/// </summary>
		private void dock_ActiveDocumentChanged(object sender, EventArgs e)
		{
			var ad = dock.ActiveDocument;
			ActiveEditorForm = ad != null ? ad.DockHandler.Content as AbstractEditorForm : null;
			UpdateEditorToolStripItems();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !CloseProjectClick();
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OptionsClick();
		}

		/// <summary>
		/// Displays the Options Dialog.
		/// </summary>
		public void OptionsClick()
		{
			using (var f = new OptionsForm())
			{
				if (f.ShowDialog() == DialogResult.OK && OpenedProject != null)
				{
					// Update editors to reflect changes.

					var eds = OpenedProject.Platform.Editors;
					foreach (var item in OpenedProject.Items)
					{
						var ed = eds.GetEditorFormIfExists(item);
						if (ed != null) { ed.LoadSettings(); }
					}
				}
			}
		}

		/// <summary>
		/// The project path to load on startup, as specified
		/// in the command line.
		/// </summary>
		public string CommandLineParameter { get; set; }

		/// <summary>
		/// Opens the project specified in the command line if any
		/// of shows the Start Page otherwise.
		/// </summary>
		private void MainForm_Shown(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(CommandLineParameter))
			{
				// In case of an error.
				SplashForm.CloseSplash(this);

				OpenProject(CommandLineParameter);
			}
			else
			{
				OpenStartPage();
			}
		}

		StartForm _start;

		/// <summary>
		/// Opens the Start Page.
		/// </summary>
		private void OpenStartPage()
		{
			CloseStartPage();
			_start = new StartForm() { MainForm = this };
			_start.Show(dock);
		}

		/// <summary>
		/// Closes the Start Page.
		/// </summary>
		private void CloseStartPage()
		{
			if (_start == null) return;
			_start.Close();
			_start.Dispose();
			_start = null;
		}
	}
}
