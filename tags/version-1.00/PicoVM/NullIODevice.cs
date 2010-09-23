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
	/// A null I/O device for picoComputer.
	/// </summary>
	/// <remarks>
	/// The input values are always Null, and the output is discarded.
	/// </remarks>
	public class NullIODevice : IIODevice
	{
		/// <summary>
		/// Writes zeroes to the destination locations.
		/// </summary>
		/// <param name="address">The destination address</param>
		/// <param name="count">The number of values to be zeroed</param>
		/// <param name="data">The destination Data object</param>
		public void Read(ushort address, int count, Data data)
		{
			for (int i = 0; i < count; i++)
			{
				data[address++] = 0;
			}
		}

		/// <summary>
		/// Writes integer values to the Null device.
		/// </summary>
		/// <remarks>
		/// In effect, the values are discarded, but they are read from
		/// the memory to trigger memory-based breakpoints.
		/// </remarks>
		/// <param name="address">The source address</param>
		/// <param name="count">The number of values to write to the null device</param>
		/// <param name="data">The source Data object</param>
		public void Write(ushort address, int count, Data data)
		{
			for (int i = 0; i < count; i++)
			{
				short value = (short)data[address];
				address++;
			}
		}

		/// <summary>
		/// Has no effect.
		/// </summary>
		public void Abort() { }

		/// <summary>
		/// Has no effect.
		/// </summary>
		public void Clear() { }
	}
}
