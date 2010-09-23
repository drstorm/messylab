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
using System.Windows.Forms;
using MessyLab.Properties;
using System.IO;

namespace MessyLab
{
	/// <summary>
	/// Entry point class of the Application.
	/// </summary>
	static class Program
	{
		/// <summary>
		/// Check whether the specified path are the same, taking
		/// into the account the case (in)sensitivity of the
		/// current platform.
		/// </summary>
		/// <returns>A value indicating whether the paths are the same.</returns>
		public static bool IsSamePath(string path1, string path2)
		{
			if (Environment.OSVersion.Platform != PlatformID.Unix &&
				Environment.OSVersion.Platform != PlatformID.MacOSX)
			{
				path1 = path1.ToLower();
				path2 = path2.ToLower();
			}

			return path1 == path2;
		}

		private static MainForm _mainForm;

		/// <summary>
		/// Instance of the Main form.
		/// </summary>
		public static MainForm MainForm { get { return _mainForm; } }

		/// <summary>
		/// The entry point of the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			SplashForm.ShowSplash();

			// Create default projects folder on first run.
			if (string.IsNullOrEmpty(Settings.Default.ProjectRoot))
			{
				Settings.Default.ProjectRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Messy Lab Projects");
				Settings.Default.Save();
				Directory.CreateDirectory(Settings.Default.ProjectRoot);
			}

			_mainForm = new MainForm();
			
			// Close Splash when the main form is shown.
			_mainForm.Shown += (sender, e) => SplashForm.CloseSplash(_mainForm);

			// If a project to open is specified, pass it to the main form.
			if (args.Length == 1 && File.Exists(args[0]))
			{
				_mainForm.CommandLineParameter = args[0];
			}

			Application.Run(_mainForm);
		}
	}
}
