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

namespace MessyLab.PicoComputer
{
	/// <summary>
	/// Represents an Error in the source code.
	/// </summary>
	public class Error
	{
		/// <summary>
		/// The Error ID
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Error Description (Message)
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Line in the source code where the error occured
		/// </summary>
		public int Line { get; set; }

		/// <summary>
		/// Column in the source code where the error occured
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// Forms a textual representation of the error, including the ID,
		/// the Description and the location in the source code.
		/// </summary>
		/// <returns>A textual representation of the error</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("E{0:0000}", ID));
			sb.Append(": ");
			sb.Append(Description);
			sb.Append(", ");
			sb.Append(Messages.AtLineLowerCase);
			sb.Append(": ");
			sb.Append(Line);
			sb.Append(", ");
			sb.Append(Messages.ColumnLowerCase);
			sb.Append(": ");
			sb.Append(Column);
			return sb.ToString();
		}
	}
}
