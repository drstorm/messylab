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
	/// Implements a Console IODevice for picoComputer.
	/// </summary>
	public class ConsoleIODevice : IIODevice
	{
		/// <summary>
		/// Reads integer values from the Console.
		/// </summary>
		/// <param name="address">The destination address</param>
		/// <param name="count">The number of values to read</param>
		/// <param name="data">The destination Data object</param>
		public void Read(ushort address, int count, Data data)
		{
			for (int i = 0; i < count; i++)
			{
				Console.Write(string.Format("An integer value for location #{0} (other values cause interrupt): ", address));
				string input = Console.ReadLine();
				data[address++] = (ushort)short.Parse(input);
			}
		}

		/// <summary>
		/// Writes integer values to the Console.
		/// </summary>
		/// <param name="address">The source address</param>
		/// <param name="count">The number of values to write</param>
		/// <param name="data">The source Data object</param>
		public void Write(ushort address, int count, Data data)
		{
			for (int i = 0; i < count; i++)
			{
				short value = (short)data[address];
				Console.WriteLine(string.Format("The contents of memory location #{0} = {1}", address, value));
				address++;
			}
		}

		/// <summary>
		/// Clears the screen.
		/// </summary>
		public void Clear()
		{
			Console.Clear();
		}

		/// <summary>
		/// Calling Abort for a ConsoleIODevice is not supported.
		/// </summary>
		public void Abort()
		{
			throw new NotSupportedException("Calling Abort for a ConsoleIODevice is not supported.");
		}
	}
}
