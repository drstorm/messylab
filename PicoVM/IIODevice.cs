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

namespace MessyLab.PicoComputer
{
	/// <summary>
	/// Specifies the interface of a picoComputer I/O Device.
	/// </summary>
	public interface IIODevice
	{
		/// <summary>
		/// Reads integer values from the input medium.
		/// </summary>
		/// <param name="address">The destination address</param>
		/// <param name="count">The number of values to read</param>
		/// <param name="data">The destination Data object</param>
		void Read(ushort address, int count, Data data);

		/// <summary>
		/// Writes integer values to the output medium.
		/// </summary>
		/// <param name="address">The source address</param>
		/// <param name="count">The number of values to write</param>
		/// <param name="data">The source Data object</param>
		void Write(ushort address, int count, Data data);

		/// <summary>
		/// Abort waiting for user input.
		/// </summary>
		void Abort();

		/// <summary>
		/// Clears the device, e.g. the screen.
		/// </summary>
		void Clear();
	}
}
