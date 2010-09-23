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
	/// Holds all the data of the picoComputer. The state of the machine is
	/// fully represented by this object. Implements a history functionality
	/// required to implement a Historic Debugger.
	/// </summary>
	public class Data
	{
		/// <summary>
		/// Creates a Data object with the History Size of 100.000.
		/// </summary>
		public Data()
		{
			HistorySize = 100000;
		}

		/// <summary>
		/// Creates a Data object with the specified History Size.
		/// </summary>
		/// <param name="historySize">The count of available history entries.</param>
		public Data(int historySize)
		{
			HistorySize = historySize;
		}

		/// <summary>
		/// The count of available history entries.
		/// </summary>
		protected int HistorySize;

		/// <summary>
		/// An array holding the content of the VM memory.
		/// </summary>
		private ushort[] _memory = new ushort[Size];
	
		/// <summary>
		/// Program Counter Register
		/// </summary>
		private ushort _pc;
		/// <summary>
		/// Stack Pointer Register
		/// </summary>
		private ushort _sp;

		private List<DataDelta> _history = new List<DataDelta>();

		/// <summary>
		/// A Collection of DataDelta object used to step back.
		/// </summary>
		public List<DataDelta> History { get { return _history; } }

		private DataDelta _delta = new DataDelta();

		/// <summary>
		/// Holds actions done by the current instruction.
		/// </summary>
		/// <remarks>
		/// Use Commit() to insert this object to history
		/// apply the changes to _data, _pc and _sp.
		/// Use Rollback() to discard changes.
		/// </remarks>
		public DataDelta Delta { get { return _delta; } }

		/// <summary>
		/// Allows accress to the memory.
		/// </summary>
		/// <remarks>
		/// Changes are stored to and retrieved from
		/// the Delta object.
		/// </remarks>
		/// <param name="address">The address of the memory location</param>
		/// <returns>The value of the addressed memory location</returns>
		public ushort this[ushort address]
		{
			get
			{
				ushort value = _memory[(int)address];
				Delta.ReadLocations.Add(address);
				
				DataDelta.WrittenLocation location;
				if (Delta.WrittenLocations.TryGetValue(address, out location))
				{
					value = location.Value;
				}
				return value;
			}
			set 
			{
				if (Delta.WrittenLocations.ContainsKey(address))
				{
					Delta.WrittenLocations[address].Value = value;
				}
				else
				{
					DataDelta.WrittenLocation location = new DataDelta.WrittenLocation();
					location.Address = address;
					location.OldValue = _memory[address];
					location.Value = value;
					Delta.WrittenLocations.Add(address, location);
				}
			}
		}

		/// <summary>
		/// Reads memory without modifying the Delta object.
		/// </summary>
		/// <remarks>
		/// This method should be used by debuggers instead of the default indexer.
		/// Because there is no log of this activity, memory breakpoints will not
		/// be triggered.
		/// </remarks>
		/// <param name="address">The address of the memory location</param>
		/// <returns>The value of the addressed memory location</returns>
		public ushort DirectMemoryRead(ushort address)
		{
			ushort value = _memory[(int)address];

			DataDelta.WrittenLocation location;
			if (Delta.WrittenLocations.TryGetValue(address, out location))
			{
				value = location.Value;
			}
			return value;
		}

		/// <summary>
		/// Writes to memory without modifying the Delta object when possible.
		/// </summary>
		/// <remarks>
		/// This method either modifies memory directly or updates the Delta object
		/// if the target location was already modified since last Commit.
		/// The method should be used by debuggers instead of the default indexer.
		/// Because there is no log of this activity, memory breakpoints will not
		/// be triggered.
		/// </remarks>
		/// <param name="address">The address of the memory location</param>
		/// <param name="value">The value to be written</param>
		public void DirectMemoryWrite(ushort address, ushort value)
		{
			if (Delta.WrittenLocations.ContainsKey(address))
			{
				Delta.WrittenLocations[address].Value = value;
			}
			else
			{
				_memory[address] = value;
			}
		}

		/// <summary>
		/// Program Counter Register
		/// </summary>
		/// <remarks>
		/// Changes are stored to and retrieved from
		/// the Delta object.
		/// </remarks>
		public ushort PC
		{
			get
			{
				if (Delta.PCWritten)
					return Delta.PC;
				else
					return _pc;
			}
			set
			{
				if (!Delta.PCWritten)
				{
					Delta.PCWritten = true;
					Delta.OldPC = _pc;
				}
				Delta.PC = value;
			}
		}

		/// <summary>
		/// Provides direct access to PC register.
		/// </summary>
		/// <remarks>
		/// For further information refer to the documentation for
		/// DirectMemoryRead and DirectMemoryWrite methods.
		/// </remarks>
		public ushort DirectPC 
		{
			get
			{
				if (Delta.PCWritten)
					return Delta.PC;
				else
					return _pc;
			}
			set
			{
				if (Delta.PCWritten)
					Delta.PC = value;
				else
					_pc = value;
			}
		}

		/// <summary>
		/// Stack Pointer Register
		/// </summary>
		/// <remarks>
		/// Changes are stored to and retrieved from
		/// the Delta object.
		/// </remarks>
		public ushort SP
		{
			get
			{
				if (Delta.SPWritten)
					return Delta.SP;
				else
					return _sp;
			}
			set
			{
				if (!Delta.SPWritten)
				{
					Delta.SPWritten = true;
					Delta.OldSP = _sp;
				}
				Delta.SP = value;
			}
		}

		/// <summary>
		/// Provides direct access to SP register.
		/// </summary>
		/// <remarks>
		/// For further information refer to the documentation for
		/// DirectMemoryRead and DirectMemoryWrite methods.
		/// </remarks>
		public ushort DirectSP
		{
			get
			{
				if (Delta.SPWritten)
					return Delta.SP;
				else
					return _sp;
			}
			set
			{
				if (Delta.SPWritten)
					Delta.SP = value;
				else
					_sp = value;
			}
		}

		/// <summary>
		/// Commits changes stored in the Delta object.
		/// </summary>
		/// <remarks>
		/// The Delta object is added to the History, and a new
		/// one is allocated.
		/// </remarks>
		/// <returns>Commited Delta object</returns>
		public DataDelta Commit()
		{
			if (Delta.PCWritten) _pc = Delta.PC;
			if (Delta.SPWritten) _sp = Delta.SP;
			foreach (DataDelta.WrittenLocation location in Delta.WrittenLocations.Values)
			{
				_memory[location.Address] = location.Value;
			}
			if (HistorySize > 0)
			{
				if (HistorySize == _history.Count)
					_history.RemoveAt(0);
				_history.Add(_delta);
			}
			DataDelta result = _delta;
			_delta = new DataDelta();
			return result;
		}

		/// <summary>
		/// Discards the Delta object.
		/// </summary>
		public void Rollback()
		{
			_delta = new DataDelta();
		}

		/// <summary>
		/// Uncommits the last DataDelta object from history. (Currect Delta is discarded.)
		/// </summary>
		/// <remarks>
		/// Assuming that Commit() is called after every instruction, calling Uncommit() and
		/// Rollback() after that undoes last executed instruction.
		/// 
		/// Uncommit fails if the History is empty.
		/// </remarks>
		/// <returns>A value indicating whether Uncommit was successful.</returns>
		public bool Uncommit()
		{
			if (History.Count > 0)
			{
				_delta = History[History.Count - 1];
				History.RemoveAt(History.Count - 1);
				
				if (Delta.PCWritten) _pc = Delta.OldPC;
				if (Delta.SPWritten) _sp = Delta.OldSP;
				foreach (DataDelta.WrittenLocation location in Delta.WrittenLocations.Values)
				{
					_memory[location.Address] = location.OldValue;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// The Memory Size.
		/// </summary>
		public const int Size = 65536;

		/// <summary>
		/// Clears the Data object.
		/// </summary>
		/// <remarks>
		/// Contents of the memory, registers and the currect Delta object is
		/// cleared.
		/// </remarks>
		public void Clear()
		{
			_memory = new ushort[Size];
			_pc = 0;
			_sp = (ushort)(Size - 1);
			_history.Clear();
			_delta = new DataDelta();
		}
	}

	/// <summary>
	/// Holds changes made to the Data object since the last Commit.
	/// </summary>
	public class DataDelta
	{
		/// <summary>
		/// Holds information about a single memory location that has been overwritten.
		/// </summary>
		public class WrittenLocation
		{
			public ushort Address;
			public ushort Value;
			public ushort OldValue;
		}

		private Dictionary<ushort, WrittenLocation> _writtenLocations = new Dictionary<ushort, WrittenLocation>();
		private HashSet<ushort> _readLocations = new HashSet<ushort>();

		/// <summary>
		/// A Dictionary holding information about the memory locations that have been written to.
		/// </summary>
		public Dictionary<ushort, WrittenLocation> WrittenLocations { get { return _writtenLocations; } }
		
		/// <summary>
		/// A set of Read memory location.
		/// </summary>
		public HashSet<ushort> ReadLocations { get { return _readLocations; } }

		/// <summary>
		/// A value indicating whether PC is changed.
		/// </summary>
		public bool PCWritten { get; set; }
		/// <summary>
		/// A value indicating whether SP is changed.
		/// </summary>
		public bool SPWritten { get; set; }

		/// <summary>
		/// Currect PC value.
		/// </summary>
		public ushort PC { get; set; }
		/// <summary>
		/// Original PC value.
		/// </summary>
		public ushort OldPC { get; set; }

		/// <summary>
		/// Current SP value.
		/// </summary>
		public ushort SP { get; set; }
		/// <summary>
		/// Original SP value.
		/// </summary>
		public ushort OldSP { get; set; }
	}
}
