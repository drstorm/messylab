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

namespace MessyLab.Platforms.Pico
{
	/// <summary>
	/// Project properties form for picoComputer.
	/// </summary>
	public partial class PicoPropertiesForm : Form
	{
		public PicoPropertiesForm()
		{
			InitializeComponent();
		}

		private void PicoPropertiesForm_Shown(object sender, EventArgs e)
		{
			LoadSettings();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			SaveSettings();
			DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// Loads the settings from the config file.
		/// </summary>
		private void LoadSettings()
		{
			var setting = MessyLab.Properties.Settings.Default;
			hexCheckBox.Checked = setting.Pico_GenerateHex;
			txtCheckBox.Checked = setting.Pico_GenerateTxt;
			autoAssembleCheckBox.Checked = setting.Pico_AutoAssemble;
		}

		/// <summary>
		/// Saves the settings to the config file.
		/// </summary>
		private void SaveSettings()
		{
			var setting = MessyLab.Properties.Settings.Default;
			setting.Pico_GenerateHex = hexCheckBox.Checked;
			setting.Pico_GenerateTxt = txtCheckBox.Checked;
			setting.Pico_AutoAssemble = autoAssembleCheckBox.Checked;
			setting.Save();
		}
	}
}
