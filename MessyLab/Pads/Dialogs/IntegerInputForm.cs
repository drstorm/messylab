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

namespace MessyLab
{
	/// <summary>
	/// An integer input dialog.
	/// </summary>
	public partial class IntegerInputForm : Form
	{
		public IntegerInputForm(string title, string prompt, int minValue, int maxValue)
		{
			InitializeComponent();
			Text = title;
			promptLabel.Text = prompt;
			valueNumericUpDown.Minimum = minValue;
			valueNumericUpDown.Maximum = maxValue;
			valueNumericUpDown.Select(0, 100);
		}

		public IntegerInputForm(string title, string prompt) : this(title, prompt, int.MinValue, int.MaxValue) { }

		/// <summary>
		/// Sets or gets the value as Int32.
		/// </summary>
		public int Value
		{
			get { return (int)valueNumericUpDown.Value; }
			set { valueNumericUpDown.Value = value; }
		}

		/// <summary>
		/// Sets or gets the value as Int64.
		/// </summary>
		public long LongValue
		{
			get { return (long)valueNumericUpDown.Value; }
			set { valueNumericUpDown.Value = value; }
		}
	}
}
