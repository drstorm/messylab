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
using System.IO;

namespace MessyLab.PicoComputer
{
	/// <summary>
	/// PicoComputer Virtual Machine
	/// </summary>
	public class VirtualMachine
	{
		/// <summary>
		/// Data storage used by this VM.
		/// </summary>
		public Data Data { get; set; }

		/// <summary>
		/// I/O Device used by the processor of this VM.
		/// </summary>
		public IIODevice IODevice
		{
			get
			{
				return Processor == null
					? null
					: Processor.IODevice;
			}
			set
			{
				if (Processor != null)
					Processor.IODevice = value;
			}
		}

		/// <summary>
		/// The processor object used by this VM.
		/// </summary>
		public Processor Processor { get; set; }

		public Disassembler Disassembler { get; set; }

		/// <summary>
		/// Constructs the object using the I/O Device and
		/// Data object with specified History size.
		/// </summary>
		/// <param name="historySize">History size for Data object.</param>
		public VirtualMachine(int historySize)
		{
			Data = new Data(historySize);
			Processor = new Processor(Data, new ConsoleIODevice());
			Disassembler = new Disassembler(Data);
		}

		/// <summary>
		/// Constructs the object using the Console I/O Device and
		/// Data object with disabled history.
		/// </summary>
		public VirtualMachine() : this(0) { }

		/// <summary>
		/// Constructs the object using the specifyed custom Data storage and I/O device
		/// objects.
		/// </summary>
		/// <param name="data">The Data object that stores memory and registers</param>
		/// <param name="ioDevice">The I/O device to be used by the I/O instructions</param>
		public VirtualMachine(Data data, IIODevice ioDevice)
		{
			Data = data;
			Processor = new Processor(Data, ioDevice);
			Disassembler = new Disassembler(Data);
		}

		/// <summary>
		/// Runs the currently loaded program.
		/// </summary>
		/// <returns>RuntimeException object representing the cause of the program termination.</returns>
		public RuntimeException Run()
		{
			while (true)
			{
				try
				{
					Processor.Step();
				}
				catch (RuntimeException e)
				{
					return e;
				}
				Data.Commit();
			}
		}

		/// <summary>
		/// Loads the machine program from a pCAS compatible hex file.
		/// </summary>
		/// <param name="filename">Full path to the file</param>
		public void LoadFromHexFile(string filename)
		{
			Data.Clear();
			string[] lines = File.ReadAllLines(filename);
			bool org = true;
			ushort currentAddress = 0;
			foreach (string s in lines)
			{
				ushort v = ushort.Parse(s, System.Globalization.NumberStyles.HexNumber);
				if (org)
				{
					org = false;
					currentAddress = v;
					Data.PC = v;
					continue;
				}
				Data[currentAddress++] = v;
			}
			Data.Commit();
			Data.History.Clear();
		}

		/// <summary>
		/// Loads the machine program from a binary file.
		/// </summary>
		/// <remarks>The first word represents the origin.</remarks>
		/// <param name="filename">Full path to the file</param>
		public void LoadFromBinaryFile(string filename)
		{
			Data.Clear();
			FileStream fs = File.OpenRead(filename);
			BinaryReader r = new BinaryReader(fs);

			bool org = true;
			ushort currentAddress = 0;
			while (fs.Position < fs.Length)
			{
				ushort v = r.ReadUInt16();
				if (org)
				{
					org = false;
					currentAddress = v;
					Data.PC = v;
					continue;
				}
				Data[currentAddress++] = v;
			}

			r.Close();
			fs.Close();
			Data.Commit();
			Data.History.Clear();
		}
	}
}
