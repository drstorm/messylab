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
using System.Threading;
using System.IO;

namespace MessyLab.Debugger.CommandLine
{
	/// <summary>
	/// Command Line User Interface.
	/// </summary>
	public class Cli
	{
		public Debugger Debugger { get; set; }
		protected SourceFatcher SourceFatcher { get; set; }

		/// <summary>
		/// Creates a Command line interface for the specified debugger.
		/// </summary>
		/// <param name="debugger"></param>
		public Cli(Debugger debugger)
		{
			if (debugger == null) throw new ArgumentNullException("debugger");

			Debugger = debugger;
			Debugger.ExecutionInterrupt += new ExecutionInterruptHandler(ExecutionInterrupted);
			
			Done = new AutoResetEvent(false);
			SourceFatcher = new SourceFatcher();
		}

		/// <summary>
		/// The event signalizes the completion of a Debugger request.
		/// </summary>
		protected AutoResetEvent Done { get; set; }

		/// <summary>
		/// Writes current program location (address and source code line)
		/// to the Console.
		/// </summary>
		protected void WriteLocation()
		{
			long address = Debugger.Target.ProgramLocation.CurrentInstructionAddress;
			try
			{
				DebugSymbol s = Debugger.DebugInformation.ByValue[address];
				Console.WriteLine("Address: " + address + "; Location: " + s.Location.ToString());
				Console.WriteLine(s.Location.Line.ToString() + " " + SourceFatcher.GetLine(s.Location));
			}
			catch (KeyNotFoundException)
			{
				Console.WriteLine("Address: " + address);
				Console.WriteLine("No debug information available for this address.");
			}
		}

		/// <summary>
		/// Handles the <c>ExecutionInterrupt</c> event of the debugger
		/// by displaying the interrupt reason and setting <c>Done</c> event.
		/// </summary>
		/// <param name="reason">The reason of the interrupt.</param>
		protected void ExecutionInterrupted(ExecutionInterruptReason reason)
		{
			// If (program loaded)
			if (reason.Exception == null || !(reason.Exception is NoProgramLoadedException))
			{
				if (reason.Exception != null)
				{
					Console.WriteLine("EXCEPTION: " + reason.Exception.Message);
				}
				StringBuilder sb = new StringBuilder();
				foreach (Breakpoint bp in reason.HitBreakpoints)
				{
					// Step breakpoints are internal and should not be displayed.
					if (bp is StepBreakpoint) continue;

					sb.Append(", ");
					sb.Append(Debugger.Breakpoints.IndexOf(bp));
				}
				string bps = sb.ToString();
				if (!string.IsNullOrEmpty(bps))
				{
					Console.Write("BREAKPOINT(S):");
					Console.WriteLine(bps.Remove(0, 1));
				}
				WriteLocation();
			}
			else
			{
				Console.WriteLine("Program unloaded.");
			}
			Done.Set();
		}

		/// <summary>
		/// Executes "load" command.
		/// </summary>
		/// <param name="parameters">Filename to load; or Filenames of Code and Debug Information</param>
		protected void Load(string[] parameters)
		{
			if (parameters.Length == 1)
			{
				if (File.Exists(parameters[0]))
				{
					Debugger.Load(parameters[0]);
					Console.WriteLine("Program loaded: " + parameters[0]);
					Console.WriteLine();
					WriteLocation();
				}
				else
				{
					Console.WriteLine("File not found: " + parameters[0]);
				}
			}
			else if (parameters.Length == 2)
			{
				if (File.Exists(parameters[0]) && File.Exists(parameters[1]))
				{
					Debugger.Load(parameters[0], parameters[1]);
					Console.WriteLine("Program loaded: " + parameters[0]);
					Console.WriteLine();
					WriteLocation();
				}
				else
				{
					Console.WriteLine("File(s) not found.");
				}
			}
			else
			{
				Console.WriteLine("Invalid parameters for Load command.");
			}
		}

		/// <summary>
		/// Toggles the backward execution and displays status.
		/// </summary>
		/// <param name="parameters">Not used.</param>
		protected void Backwards(string[] parameters)
		{
			Debugger.ExecuteBackwards = !Debugger.ExecuteBackwards;
			Console.WriteLine("Backward Execution is " + (Debugger.ExecuteBackwards ? "ON." : "OFF."));
		}

		/// <summary>
		/// Executes "run" command.
		/// </summary>
		/// <param name="parameters">Not used.</param>
		protected void Run(string[] parameters)
		{
			try
			{
				Debugger.Run();
				Done.WaitOne();
			}
			catch (NoProgramLoadedException ex) { Console.WriteLine(ex.Message); }
		}

		/// <summary>
		/// Executes "step" command.
		/// </summary>
		/// <param name="parameters">Not used.</param>
		protected void StepInto(string[] parameters)
		{
			try
			{
				Debugger.StepInto();
				Done.WaitOne();
			}
			catch (NoProgramLoadedException ex) { Console.WriteLine(ex.Message); }
		}

		/// <summary>
		/// Executes "next" command.
		/// </summary>
		/// <param name="parameters">Not used.</param>
		protected void StepOver(string[] parameters)
		{
			try
			{
				Debugger.StepOver();
				Done.WaitOne();
			}
			catch (NoProgramLoadedException ex) { Console.WriteLine(ex.Message); }
		}

		/// <summary>
		/// Executes "finish" command.
		/// </summary>
		/// <param name="parameters">Not used.</param>
		protected void StepOut(string[] parameters)
		{
			try
			{
				Debugger.StepOut();
				Done.WaitOne();
			}
			catch (NoProgramLoadedException ex) { Console.WriteLine(ex.Message); }
		}

		/// <summary>
		/// Executes "kill" command.
		/// </summary>
		/// <param name="parameters">Not used.</param>
		protected void Unload(string[] parameters)
		{
			Debugger.Unload();
			Done.WaitOne();
		}

		/// <summary>
		/// Executes "print" command.
		/// </summary>
		/// <param name="parameters">A list of either Addresses or Symbols.</param>
		protected void Print(string[] parameters)
		{
			foreach (string s in parameters)
			{
				try
				{
					long address = long.Parse(s);
					Console.WriteLine("Memory at " + address + ": " + Debugger.Target.Memory[address]);
				}
				catch
				{
					DebugSymbol sym;
					if (Debugger.DebugInformation.ByName.TryGetValue(s, out sym))
					{
						long address = sym.Value;
						Console.WriteLine("Memory at " + s + " (" + address + "): " + Debugger.Target.Memory[address]);
					}
					else
					{
						Console.WriteLine("Symbol \"" + s + "\" not found.");
					}
				}
			}
		}

		/// <summary>
		/// Executes "set" command.
		/// </summary>
		/// <param name="parameters">Parameter formated as 'destination=value'.</param>
		protected void Set(string[] parameters)
		{
			if (parameters.Length > 3 || parameters.Length < 1)
			{
				Console.WriteLine("Invalid parameters for Set command.");
				return;
			}
			StringBuilder sb = new StringBuilder();
			foreach (string s in parameters)
			{ sb.Append(s); }

			string[] parts = sb.ToString().Split('=');
			if (parts.Length != 2)
			{
				Console.WriteLine("Invalid parameters for Set command.");
				return;
			}

			long destination;
			try
			{ destination = long.Parse(parts[0]); }
			catch
			{
				DebugSymbol sym;
				if (Debugger.DebugInformation.ByName.TryGetValue(parts[0], out sym))
				{ destination = sym.Value; }
				else
				{
					Console.WriteLine("Symbol \"" + parts[0] + "\" not found.");
					return;
				}
			}

			long value;
			try
			{ value = long.Parse(parts[1]); }
			catch
			{
				DebugSymbol sym;
				if (Debugger.DebugInformation.ByName.TryGetValue(parts[1], out sym))
				{ value = Debugger.Target.Memory[sym.Value]; }
				else
				{
					Console.WriteLine("Symbol \"" + parts[1] + "\" not found.");
					return;
				}
			}

			Debugger.Target.Memory[destination] = value;
			Console.WriteLine("Memory at " + destination + " = " + value);
		}

		/// <summary>
		/// Executes "break" command.
		/// </summary>
		/// <param name="parameters">Source code location; or Address or Symbol starting with '*'</param>
		protected void Break(string[] parameters)
		{
			if (parameters.Length != 1 || parameters[0].Length == 0)
			{
				Console.WriteLine("Invalid parameters for Break command.");
				return;
			}
			if (parameters[0][0] == '*') // Address or label
			{
				string s = parameters[0].Remove(0, 1);
				long address;
				try
				{ address = long.Parse(s); }
				catch
				{
					DebugSymbol sym;
					if (Debugger.DebugInformation.ByName.TryGetValue(s, out sym))
					{
						address = sym.Value;
						if (!sym.IsExecutable)
						{
							Console.WriteLine("Symbol \"" + s + "\" is not a label.");
							return;
						}
					}
					else
					{
						Console.WriteLine("Symbol \"" + s + "\" not found.");
						return;
					}
				}
				MemoryBreakpoint bp = new MemoryBreakpoint(address);
				Debugger.Breakpoints.Add(bp);
				Console.WriteLine("Breakpoint " + Debugger.Breakpoints.IndexOf(bp) + " at " + address + " on Execute.");
			}
			else // Source line
			{
				string s = parameters[0];
				int line;
				string file = null;
				try
				{ line = int.Parse(s); }
				catch
				{
					string[] parts = s.Split(':');
					if (parts.Length != 2)
					{
						Console.WriteLine("Invalid parameters for Break command.");
						return;
					}
					
					try
					{ line = int.Parse(parts[0]); }
					catch
					{
						Console.WriteLine("Invalid parameters for Break command.");
						return;
					}
					
					file = parts[1];
				}
				MemoryBreakpoint bp;
				try
				{
					if (file == null)
					{ bp = new MemoryBreakpoint(Debugger, line); }
					else
					{ bp = new MemoryBreakpoint(Debugger, file, line); }
				}
				catch (DebugSymbolNotFoundException)
				{
					Console.WriteLine("Debug Symbol not found for specified location.");
					return;
				}
				Debugger.Breakpoints.Add(bp);
				Console.WriteLine("Breakpoint " + Debugger.Breakpoints.IndexOf(bp) + " at " + bp.Address +
					(bp.OnExecute ? " on Execute." : " on Write."));
			}
		}

		/// <summary>
		/// Executes "watch" command.
		/// </summary>
		/// <param name="parameters">Address of Symbol to watch.</param>
		protected void Watch(string[] parameters)
		{
			if (parameters.Length != 1)
			{
				Console.WriteLine("Invalid parameters for Watch command.");
				return;
			}
			string s = parameters[0];
			long address;
			try
			{ address = long.Parse(s); }
			catch
			{
				DebugSymbol sym;
				if (Debugger.DebugInformation.ByName.TryGetValue(s, out sym))
				{ address = sym.Value; }
				else
				{
					Console.WriteLine("Symbol \"" + s + "\" not found.");
					return;
				}
			}
			MemoryBreakpoint bp = new MemoryBreakpoint(address);
			bp.OnExecute = false;
			bp.OnWrite = true;
			Debugger.Breakpoints.Add(bp);
			Console.WriteLine("Breakpoint " + Debugger.Breakpoints.IndexOf(bp) + " at " + address + " on Write.");
		}

		/// <summary>
		/// Executes "delete" command
		/// </summary>
		/// <param name="parameters">A list of breakpoint indexes to delete.</param>
		protected void Delete(string[] parameters)
		{
			List<int> toDel = new List<int>();
			foreach (string s in parameters)
			{
				int n;
				try
				{ n = int.Parse(s); }
				catch
				{
					Console.WriteLine("Invalid parameter: " + s);
					continue;
				}
				toDel.Add(n);
			}
			toDel.Sort();
			for (int i = toDel.Count - 1; i >= 0; i--)
			{
				try
				{
					Debugger.Breakpoints.RemoveAt(toDel[i]);
					Console.WriteLine("Breakpoint " + toDel[i] + " deleted.");
				}
				catch (ArgumentOutOfRangeException)
				{
					Console.WriteLine("Breakpoint " + toDel[i] + " not found.");
				}
			}
		}

		/// <summary>
		/// Executes "info" command.
		/// </summary>
		/// <param name="parameters">Either "reg", "r", "break", "breakpoints" or "b".</param>
		protected void Info(string[] parameters)
		{
			bool ok = true;
			if (parameters.Length != 1) { ok = false; }

			string command = parameters[0].ToLower();

			if (command != "reg" && command != "r"
				&& command != "break" && command != "breakpoints" && command != "b")
			{ ok = false; }

			if (!ok)
			{
				Console.WriteLine("Invalid parameters for Info command.");
				return;
			}
			if (command == "reg" || command == "r")
			{
				Console.WriteLine("+----------+----------+");
				Console.WriteLine("|Register  |Value     |");
				Console.WriteLine("+----------+----------+");
				foreach (string reg in Debugger.Target.Registers.Names)
				{
					Console.Write("|");
					Console.Write(reg.PadRight(10));
					Console.Write("|");
					Console.Write(Debugger.Target.Registers[reg].ToString().PadLeft(10));
					Console.WriteLine("|");
				}
				Console.WriteLine("+----------+----------+");
			}
			else // breakpoints
			{
				Console.WriteLine("+--------+----------------+------------+----------+");
				Console.WriteLine("|N       |Address         |On Execute  |On Write  |");
				Console.WriteLine("+--------+----------------+------------+----------+");
				for (int i = 0; i < Debugger.Breakpoints.Count; i++)
				{
					MemoryBreakpoint bp = Debugger.Breakpoints[i] as MemoryBreakpoint;
					if (bp == null) continue;
					Console.Write("|");
					Console.Write(i.ToString().PadRight(8));
					Console.Write("|");
					Console.Write(bp.Address.ToString().PadRight(16));
					Console.Write("|");
					Console.Write(bp.OnExecute ? "YES         " : "NO          ");
					Console.Write("|");
					Console.Write(bp.OnWrite ? "YES       " : "NO        ");
					Console.WriteLine("|");
				}
				Console.WriteLine("+--------+----------------+------------+----------+");
			}
		}

		/// <summary>
		/// Displayes Help message.
		/// </summary>
		/// <param name="parameters">Not used.</param>
		protected void Help(string[] parameters)
		{
			Console.WriteLine("Available commands:");
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("load        Loads a program from specified file.");
			Console.WriteLine("            Additional name(s): l");
			Console.WriteLine("            If two files are specified, the second one is the Debug Info file.");
			Console.WriteLine();
			Console.WriteLine("unload      Unloads current program.");
			Console.WriteLine("            Additional name(s): kill");
			Console.WriteLine();
			Console.WriteLine("back        Toggles backward execution.");
			Console.WriteLine("            Additional name(s): backwards");
			Console.WriteLine();
			Console.WriteLine("run         Runs or Continues loaded program.");
			Console.WriteLine("            Additional name(s): r, c");
			Console.WriteLine("            The execution is interrupted if an exception is raised or if a");
			Console.WriteLine("            breakpoint is triggered.");
			Console.WriteLine();
			Console.WriteLine("step        Executes a single instruction.");
			Console.WriteLine("            Additional name(s): stepinto, s");
			Console.WriteLine();
			Console.WriteLine("next        Executes a single source code line.");
			Console.WriteLine("            Additional name(s): stepover, n");
			Console.WriteLine();
			Console.WriteLine("finish      Runs until the current subroutine is finished.");
			Console.WriteLine("            Additional name(s): stepout, f");
			Console.WriteLine();
			Console.WriteLine("break       Creates a new breakpoint.");
			Console.WriteLine("            Additional name(s): b");
			Console.WriteLine("            Examples:");
			Console.WriteLine("            b 5          - Breakpoint at line 5.");
			Console.WriteLine("            b 5:test.src - Breakpoint at line 5 in 'test.src'.");
			Console.WriteLine("            b *5         - Breakpoint at memory address 5.");
			Console.WriteLine("            b *test      - Breakpoint at label 'test'.");
			Console.WriteLine();
			Console.WriteLine("watch       Creates a new watchpoint (memory breakpoint).");
			Console.WriteLine("            Additional name(s): w");
			Console.WriteLine();
			Console.WriteLine("delete      Deletes specified breakpoint(s).");
			Console.WriteLine("            Additional name(s): d");
			Console.WriteLine("            Example:");
			Console.WriteLine("            d 5          - Deletes Breakpoint 5.");
			Console.WriteLine("            Use 'info break' to list breakpoints");
			Console.WriteLine();
			Console.WriteLine("info reg    Displays register values.");
			Console.WriteLine("            Additional name(s): i r, i reg");
			Console.WriteLine();
			Console.WriteLine("info break  Displays Breakpoint List.");
			Console.WriteLine("            Additional name(s): i b, i break, i breakpoints");
			Console.WriteLine();
			Console.WriteLine("print       Displays specified memory location(s).");
			Console.WriteLine("            Additional name(s): p");
			Console.WriteLine();
			Console.WriteLine("set         Writes the specified value to the specified memory location.");
			Console.WriteLine("            Examples:");
			Console.WriteLine("            set 5=10 - Writes 10 to memory location at 5.");
			Console.WriteLine("            set a=10 - Writes 10 to memory location at symbol 'a'.");
			Console.WriteLine("            set b=a  - Writes value of memory location at 'a' to loc. at 'b'.");
			Console.WriteLine();
			Console.WriteLine("quit        Quits debugger and unloads the program if loaded.");
			Console.WriteLine("            Additional name(s): exit, q");
			Console.WriteLine();
			Console.WriteLine("help        Displays this help screen.");
			Console.WriteLine("            Additional name(s): ?");
		}

		/// <summary>
		/// Splits a command line into command name and parameters.
		/// </summary>
		/// <remarks>
		/// Supports parameters containing spaces by allowing quotes.
		/// </remarks>
		/// <example>load "Big Program.bin"</example>
		/// <param name="line"></param>
		/// <param name="command"></param>
		/// <param name="parameters"></param>
		private void SplitLine(string line, out string command, out string[] parameters)
		{
			command = line.Split(' ')[0];

			string[] split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			
			List<string> paramList = new List<string>();

			StringBuilder current = null;
			for (int i = 1; i < split.Length; i++)
			{
				string s = split[i];
				if (s.StartsWith("\""))
				{
					current = new StringBuilder();
					current.Append(s.Remove(0, 1));
					continue;
				}
				if (current != null)
				{
					current.Append(' ');
					if (s.EndsWith("\""))
					{
						current.Append(s.Remove(s.Length - 1, 1));
						paramList.Add(current.ToString());
						current = null;
						continue;
					}
					current.Append(s);
					continue;
				}
				paramList.Add(s);
			}

			parameters = paramList.ToArray();
		}

		/// <summary>
		/// Starts the Command Line Interpreter.
		/// </summary>
		public void Run()
		{
			while (true)
			{
				Console.WriteLine();
				Console.Write("(mldbg) ");
				string line = Console.ReadLine();
				if (string.IsNullOrEmpty(line)) continue;

				string command;
				string[] parameters;
				SplitLine(line, out command, out parameters);

				try
				{
					switch (command)
					{
						case "load":
						case "l":
							Load(parameters);
							break;
						case "back":
						case "backwards":
							Backwards(parameters);
							break;
						case "run":
						case "r":
						case "c":
							Run(parameters);
							break;
						case "step":
						case "stepinto":
						case "s":
							StepInto(parameters);
							break;
						case "next":
						case "stepover":
						case "n":
							StepOver(parameters);
							break;
						case "finish":
						case "stepout":
						case "f":
							StepOut(parameters);
							break;

						case "break":
						case "b":
							Break(parameters);
							break;
						case "watch":
						case "w":
							Watch(parameters);
							break;
						case "delete":
						case "d":
							Delete(parameters);
							break;

						case "info":
						case "i":
							Info(parameters);
							break;

						case "print":
						case "p":
							Print(parameters);
							break;
						case "set":
							Set(parameters);
							break;

						case "unload":
						case "kill":
							Unload(parameters);
							break;
						case "quit":
						case "exit":
						case "q":
							try { Unload(parameters); }
							catch { }
							return;

						case "help":
						case "?":
							Help(parameters);
							break;

						default:
							Console.WriteLine("Invalid command. Type 'help' for command list.");
							break;
					}
				}
				catch (NoProgramLoadedException ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}

	}
}
