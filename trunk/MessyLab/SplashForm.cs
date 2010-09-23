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
using System.Threading;

namespace MessyLab
{
	/// <summary>
	/// The Splash screen form.
	/// </summary>
	public partial class SplashForm : Form
	{
		public SplashForm()
		{
			InitializeComponent();
			thread = new Thread(Start);
		}

		/// <summary>
		/// The separate message loop thread.
		/// </summary>
		private Thread thread;

		/// <summary>
		/// Activates the specified form and closes the
		/// splash screen by exiting its message loop.
		/// </summary>
		/// <param name="f">The form to activate.</param>
		private void Kill(Form f)
		{
			f.Invoke(new Action(() => f.Activate()));
			Application.ExitThread();
		}

		/// <summary>
		/// Starts the message loop on the current thread.
		/// </summary>
		private void Start()
		{
			Application.Run(this);
		}

		/// <summary>
		/// Splash form instance.
		/// </summary>
		private static SplashForm form = null;

		/// <summary>
		/// Creates the splash form and shows it in
		/// its own message loop.
		/// </summary>
		public static void ShowSplash()
		{
			if (form == null) form = new SplashForm();
			form.thread.Start();
		}

		/// <summary>
		/// Closes the splash and activates the specified form.
		/// </summary>
		/// <param name="f">The form to activate.</param>
		public static void CloseSplash(Form f)
		{
			if (form != null)
			{
				form.BeginInvoke(new Action<Form>(form.Kill), f);
				form = null;
			}
		}

		#region DropShadow Code http://davidkean.net/archive/2004/09/13/151.aspx
		private const int CS_DROPSHADOW = 0x00020000;

		protected override CreateParams CreateParams
		{
			[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, UnmanagedCode = true)]
			get
			{
				CreateParams parameters = base.CreateParams;

				if (DropShadowSupported)
				{
					parameters.ClassStyle = (parameters.ClassStyle | CS_DROPSHADOW);
				}

				return parameters;
			}
		}

		public static bool DropShadowSupported
		{
			get { return IsWindowsXPOrAbove; }
		}

		public static bool IsWindowsXPOrAbove
		{
			get
			{
				OperatingSystem system = Environment.OSVersion;
				bool runningNT = system.Platform == PlatformID.Win32NT;

				return runningNT && system.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0;
			}
		}
		#endregion
	}
}
