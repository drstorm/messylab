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
using System.IO;

namespace MessyLab.Platforms
{
	/// <summary>
	/// Platform represents a set of features.
	/// </summary>
	/// <remarks>
	/// You can think of a platform as a type of a project.
	/// Platform encapsulates a computer architecture, e.g. picoComputer.
	/// </remarks>
	public abstract class Platform
	{
		#region GUI
		/// <summary>
		/// Provies and manages platform-specific GUI elements.
		/// </summary>
		public abstract class GuiProvider
		{
			public GuiProvider(Platform platform)
			{
				Platform = platform;
				MenuItems = new List<ToolStripMenuItem>();
				ToolbarItems = new List<ToolStripItem>();
				Pads = new List<Pad>();
			}

			/// <summary>
			/// Parent platform.
			/// </summary>
			public Platform Platform { get; protected set; }
			/// <summary>
			/// Gets the MainForm reference from the platform.
			/// </summary>
			public MainForm MainForm
			{ get { return Platform != null ? Platform.MainForm : null; } }

			/// <summary>
			/// Initializes the GUI elements.
			/// </summary>
			public virtual void Initialize()
			{
				CreatePads();
				CreateMenuItems();
				CreateToolbarItems();
			}

			#region Event Handlers
			/// <summary>
			/// Adds the specified existing project item or
			/// reports an error if any.
			/// </summary>
			/// <param name="path">The path to the item.</param>
			protected virtual void AddExistingProjectItem(string path)
			{
				try
				{
					Platform.AddExistingProjectItem(path);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}

			/// <summary>
			/// Handles the Add Existing Project Item command click.
			/// </summary>
			protected abstract void AddExistingProjectItem();

			/// <summary>
			/// Adds a new project item at the specified path or
			/// reports an error if any.
			/// </summary>
			/// <param name="path">The path to the item.</param>
			protected virtual void AddNewProjectItem(string path)
			{
				try
				{
					Platform.AddNewProjectItem(path);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}

			/// <summary>
			/// Removes the specified project item or reports an
			/// error if any.
			/// </summary>
			/// <param name="item"></param>
			protected virtual void RemoveProjectItem(ProjectItem item)
			{
				try
				{
					Platform.RemoveProjectItem(item);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

			}

			/// <summary>
			/// Handles the Add New Project Item command click.
			/// </summary>
			protected abstract void AddNewProjectItem();

			/// <summary>
			/// Handles the Show Project Properties command click.
			/// </summary>
			protected abstract void ShowProjectProperties();
			#endregion

			#region Menus and Toolbars

			#region Menu Creation
			/// <summary>
			/// Creates View menu and its items.
			/// </summary>
			/// <returns>The View menu.</returns>
			protected virtual ToolStripMenuItem CreateViewMenu()
			{
				var view = new ToolStripMenuItem("&View");
				view.DropDownItems.Add(ProjectPad.MenuItem);
				view.DropDownItems.Add(ErrorsPad.MenuItem);
				view.DropDownItems.Add(TasksPad.MenuItem);
				return view;
			}

			/// <summary>
			/// Creates the Project menu and its items.
			/// </summary>
			/// <returns>The project menu.</returns>
			protected virtual ToolStripMenuItem CreateProjectMenu()
			{
				var project = new ToolStripMenuItem("&Project");

				var item = new ToolStripMenuItem("Add Ne&w File...");
				item.Click += (sender, e) => AddNewProjectItem();
				project.DropDownItems.Add(item);

				item = new ToolStripMenuItem("Add Existin&g File...");
				item.Click += (sender, e) => AddExistingProjectItem();
				project.DropDownItems.Add(item);

				project.DropDownItems.Add(new ToolStripSeparator());

				item = new ToolStripMenuItem("&Build", MessyLab.Properties.Resources.Build);
				item.ShortcutKeys = Keys.F6;
				item.Click += (sender, e) => Platform.Build();
				project.DropDownItems.Add(item);

				item = new ToolStripMenuItem("&Run", MessyLab.Properties.Resources.Run);
				item.ShortcutKeys = Keys.Control | Keys.F5;
				item.Click += (sender, e) => Platform.Run();
				project.DropDownItems.Add(item);

				project.DropDownItems.Add(new ToolStripSeparator());

				item = new ToolStripMenuItem("&Properties");
				item.Click += (sender, e) => ShowProjectProperties();
				project.DropDownItems.Add(item);

				return project;
			}

			/// <summary>
			/// Creates the menu items and populates the MenuItems list.
			/// </summary>
			protected virtual void CreateMenuItems()
			{
				MenuItems.Clear();
				MenuItems.Add(CreateViewMenu());
				MenuItems.Add(CreateProjectMenu());
			}
			#endregion

			#region Toolbar Creation
			/// <summary>
			/// Creates the View toolbar items and populates the ToolbarItems list.
			/// </summary>
			protected virtual void CreateViewToolbar()
			{
				ToolbarItems.Add(ProjectPad.ToolbarItem);
				ToolbarItems.Add(ErrorsPad.ToolbarItem);
				ToolbarItems.Add(TasksPad.ToolbarItem);
			}

			/// <summary>
			/// Creates the Run toolbar items and populates the ToolbarItems list.
			/// </summary>
			protected virtual void CreateRunToolbar()
			{
				var run = new ToolStripButton("Run", MessyLab.Properties.Resources.Run);
				run.DisplayStyle = ToolStripItemDisplayStyle.Image;
				run.Click += (sender, e) => Platform.Run();
				ToolbarItems.Add(run);
			}

			/// <summary>
			/// Creates the toolbar items and populates the ToolbarItems list.
			/// </summary>
			protected virtual void CreateToolbarItems()
			{
				ToolbarItems.Clear();
				ToolbarItems.Add(new ToolStripSeparator());
				CreateRunToolbar();
				ToolbarItems.Add(new ToolStripSeparator());
				CreateViewToolbar();
			}
			#endregion

			public List<ToolStripMenuItem> MenuItems { get; protected set; }
			public List<ToolStripItem> ToolbarItems { get; protected set; }
			#endregion

			#region Pads
			public ProjectPad ProjectPad { get; protected set; }
			public ErrorListPad ErrorsPad { get; protected set; }
			public TaskListPad TasksPad { get; protected set; }

			#region Pad Save/Restore

			/// <summary>
			/// Gets the Pad by the type name.
			/// </summary>
			/// <param name="typeName">Type name of the pad.</param>
			/// <returns>The pad for the specified name or null if the name is invalid.</returns>
			protected virtual Pad GetPadByTypeName(string typeName)
			{
				switch (typeName)
				{
					case "MessyLab.ProjectPad":
						return ProjectPad;
					case "MessyLab.TaskListPad":
						return TasksPad;
					case "MessyLab.ErrorListPad":
						return ErrorsPad;
					default:
						return null;
				}
			}

			/// <summary>
			/// Saves the open pad positions.
			/// </summary>
			protected internal virtual void SaveOpenPads()
			{
				string dir = GetSettingsPath();
				string path = Path.Combine(dir, Platform.Name + ".dock");
				MainForm.DockPanel.SaveAsXml(path);
			}

			/// <summary>
			/// Opens the saved pads on the main form.
			/// </summary>
			/// <returns></returns>
			protected virtual bool OpenSavedPads()
			{
				// User-specific
				string dir = GetSettingsPath();
				string path = Path.Combine(dir, Platform.Name + ".dock");
				if (File.Exists(path))
				{
					try
					{
						MainForm.DockPanel.LoadFromXml(path, new WeifenLuo.WinFormsUI.Docking.DeserializeDockContent(GetPadByTypeName));
						return true;
					}
					catch { }
				}

				// Platform Default
				path = Path.Combine(Platform.GetPlatformsDirectory(Platform.PlatformSubDir), "DefaultDock.xml");
				try
				{
					MainForm.DockPanel.LoadFromXml(path, new WeifenLuo.WinFormsUI.Docking.DeserializeDockContent(GetPadByTypeName));
					return true;
				}
				catch { }

				// Fail.
				return false;
			}

			/// <summary>
			/// Open pads on their default positions.
			/// </summary>
			protected virtual void OpenDefaultPads()
			{
				ProjectPad.ShowOnMainForm();
				TasksPad.ShowOnMainForm();
				ErrorsPad.ShowOnMainForm();
			}

			/// <summary>
			/// Opens pads on the Main form.
			/// </summary>
			public virtual void OpenPads()
			{
				if (!OpenSavedPads())
					OpenDefaultPads();
			}
			#endregion

			/// <summary>
			/// Creates, initializes pads and populates the Pads list.
			/// </summary>
			protected virtual void CreatePads()
			{
				Pads.Clear();
				ErrorsPad = new ErrorListPad(Platform.Project);
				Pads.Add(ErrorsPad);
				TasksPad = new TaskListPad(Platform.Project);
				Pads.Add(TasksPad);

				ProjectPad = new ProjectPad(Platform.Project);
				ProjectPad.ItemDoubleClicked += () => MainForm.ShowEditorFor(ProjectPad.SelectedItem);
				ProjectPad.AddNewItemClicked += () => AddNewProjectItem();
				ProjectPad.AddExistingItemClicked += () => AddExistingProjectItem();
				ProjectPad.RemoveClicked += () => RemoveProjectItem(ProjectPad.SelectedItem);
				Pads.Add(ProjectPad);
			}

			public List<Pad> Pads { get; set; }

			#region Pad Updates
			/// <summary>
			/// Updates the content of the Tasks pad.
			/// </summary>
			public virtual void UpdateTasksPad()
			{
				TasksPad.ClearItems();
				foreach (var item in Platform.Project.Items)
				{
					foreach (var i in item.GetTasks())
					{
						TasksPad.AddItem(i);
					}
				}
			}
			#endregion

			#endregion
		}
		#endregion

		#region Editor
		/// <summary>
		/// Provides and manages editors for project items.
		/// </summary>
		public abstract class EditorProvider
		{
			public EditorProvider(Platform platform)
			{
				Platform = platform;
				EditorForms = new Dictionary<string, AbstractEditorForm>();
			}

			/// <summary>
			/// Parent platform.
			/// </summary>
			public Platform Platform { get; protected set; }
			
			/// <summary>
			/// Instanced editor forms.
			/// </summary>
			protected internal Dictionary<string, AbstractEditorForm> EditorForms { get; set; }

			/// <summary>
			/// Creates an item specific editor form.
			/// </summary>
			/// <param name="item">The project item to create an editor for.</param>
			/// <returns>The created editor form.</returns>
			protected abstract AbstractEditorForm CreateEditorForm(ProjectItem item);

			/// <summary>
			/// Gets the editor for the specified item if it is already instanced.
			/// </summary>
			/// <param name="item">The project item to get an editor for.</param>
			/// <returns>The editor for the specified item if it exists, otherwise null.</returns>
			public AbstractEditorForm GetEditorFormIfExists(ProjectItem item)
			{
				AbstractEditorForm ed = null;
				EditorForms.TryGetValue(item.Path, out ed);
				return ed;
			}

			/// <summary>
			/// Initializes the specified editor form.
			/// </summary>
			/// <param name="form">The editor form to initialize.</param>
			public virtual void InitializeEditor(AbstractEditorForm form)
			{
				form.IsReadOnly = _editorsReadOnly;
			}

			/// <summary>
			/// Gets the editor for the specified item if it is already instanced;
			/// otherwise, creates one.
			/// </summary>
			/// <param name="item">The project item to get an editor for.</param>
			/// <returns>The editor for the specified item.</returns>
			public virtual AbstractEditorForm GetEditorForm(ProjectItem item)
			{
				var form = GetEditorFormIfExists(item);
				if (form == null)
				{
					form = CreateEditorForm(item);
					InitializeEditor(form);
					EditorForms[item.Path] = form;
				}
				return form;
			}

			private bool _editorsReadOnly;
			/// <summary>
			/// Indicates whether the Editors are read-only.
			/// </summary>
			public bool EditorsReadOnly
			{
				get { return _editorsReadOnly; }
				set
				{
					if (_editorsReadOnly != value)
					{
						_editorsReadOnly = value;
						foreach (var editor in EditorForms.Values)
						{
							editor.IsReadOnly = value;
						}
					}
				}
			}
		}
		#endregion

		public Platform(Project project, MainForm mainForm)
		{
			if (project == null) throw new ArgumentNullException("project");
			if (mainForm == null) throw new ArgumentNullException("mainForm");
			Project = project;
			MainForm = mainForm;
		}

		/// <summary>
		/// The platform name.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// A reference to the Main form.
		/// </summary>
		protected MainForm MainForm { get; set; }

		/// <summary>
		/// A GUI provider.
		/// </summary>
		public abstract GuiProvider Gui { get; }

		/// <summary>
		/// An Editor provider.
		/// </summary>
		public abstract EditorProvider Editors { get; }

		/// <summary>
		/// Initializes the Platform.
		/// </summary>
		public virtual void Initialize()
		{
			Gui.Initialize();
		}

		#region Project Management
		/// <summary>
		/// Parent project.
		/// </summary>
		public Project Project { get; set; }

		/// <summary>
		/// Platform specific ProjectItem factory.
		/// </summary>
		public abstract ProjectItemAbstractFactory ProjectItemFactory { get; }

		/// <summary>
		/// Adds a new project item at the specified path.
		/// </summary>
		/// <param name="path">The path of the new project item.</param>
		protected virtual void AddNewProjectItem(string path)
		{
			var item = ProjectItemFactory.CreateProjectItem(Project, path);
			if (Project.ItemExists(item.RelativePath))
			{
				throw new Exception(string.Format("File \"{0}\" already exists in the project.", item.RelativePath));
			}
			Project.Items.Add(item);
			Project.Save();
		}

		/// <summary>
		/// Adds an existing project item.
		/// </summary>
		/// <param name="path">The path to the item to add.</param>
		protected virtual void AddExistingProjectItem(string path)
		{
			string projectDir = Path.GetDirectoryName(Path.GetFullPath(Project.Path));
			string itemDir = Path.GetDirectoryName(Path.GetFullPath(path));

			if (!Program.IsSamePath(projectDir, itemDir))
			{
				string itemPath = Path.Combine(projectDir, Path.GetFileName(path));
				if (File.Exists(itemPath))
				{
					throw new Exception(string.Format("Cannot add \"{0}\" because the file already exists.", Path.GetFileName(path)));
				}
				File.Copy(path, itemPath);
			}

			if (Project.ItemExists(Path.GetFileName(path)))
			{
				throw new Exception(string.Format("Cannot add \"{0}\" because it is already added.", Path.GetFileName(path)));
			}

			ProjectItem item = new ProjectItem();
			item.Project = Project;
			item.RelativePath = Path.GetFileName(path);
			Project.Items.Add(item);
			Project.Save();
		}

		/// <summary>
		/// Removes the specified project item.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		protected virtual void RemoveProjectItem(ProjectItem item)
		{
			if (item == null) return;
			if (Project.Items.Count == 1)
			{
				throw new Exception("A project must contain at least one file, therefore, the file could not be removed.");
			}
			// Close the editor if any.
			var e = Editors.GetEditorFormIfExists(item);
			if (e != null)
			{
				e.Save();
				e.Close();
				Editors.EditorForms.Remove(item.Path);
			}

			Project.Items.Remove(item);
			
			// If the main item has been removed, set another one as main.
			if (ReferenceEquals(Project.MainItem, item))
			{
				Project.MainItem = Project.Items[0];
			}

			Project.Save();
		}
		#endregion

		#region Build/Run
		/// <summary>
		/// Builds the current project.
		/// </summary>
		/// <param name="binaryPath">A variable to store the path to the generated binary.</param>
		/// <returns>A value indicating whether the build was successful.</returns>
		protected internal abstract bool Build(out string binaryPath);

		/// <summary>
		/// Builds the current project.
		/// </summary>
		/// <returns>A value indicating whether the build was successful.</returns>
		protected internal virtual bool Build()
		{
			string dummy;
			return Build(out dummy);
		}

		/// <summary>
		/// Runs the current project.
		/// </summary>
		protected abstract void Run();
		#endregion

		#region Settings
		/// <summary>
		/// Saves the platform specific settings.
		/// </summary>
		public virtual void SaveSettings()
		{
			Gui.SaveOpenPads();
		}

		/// <summary>
		/// Loads the platform specidic settings.
		/// </summary>
		public virtual void LoadSettings() { }

		/// <summary>
		/// Gets the User specific Settings path.
		/// </summary>
		/// <returns>The path to the setting folder.</returns>
		public static string GetSettingsPath()
		{
			string dir = Application.UserAppDataPath;
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			return dir;
		}
		#endregion

		#region Platform Directory
		/// <summary>
		/// The platform specific sub-directory withing the application installation folder.
		/// </summary>
		public abstract string PlatformSubDir { get; }

		/// <summary>
		/// Gets the path to the platform specific folder within the application installation folder.
		/// </summary>
		/// <param name="platformSubDir">The name of the platform sub-directory.</param>
		/// <returns></returns>
		public static string GetPlatformsDirectory(string platformSubDir)
		{
			string dir = Application.ExecutablePath;
			dir = Path.GetFullPath(dir);
			dir = Path.GetDirectoryName(dir);
			dir = Path.Combine(dir, "Platforms");
			dir = Path.Combine(dir, platformSubDir);
			return dir;
		}
		#endregion
	}
}
