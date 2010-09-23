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
	/// A command line support implementation.
	/// </summary>
	class Program
	{
		static void Greet()
		{
			Console.WriteLine("Messy Lab picoComputer Virtual Machine, Version 1.0");
			Console.WriteLine("Copyright (c) 2010 by Milos Andjelkovic");
			Console.WriteLine();
		}

		static void Usage()
		{
			Console.WriteLine("Usage: PicoVM /? | [/hex] program_file");
			Console.WriteLine();
			Console.WriteLine("  /hex        Loads the machine code from a pCAS compatible HEX format.");
			Console.WriteLine("              The default is binary.");
			Console.WriteLine();
			Console.WriteLine("  /?          Displays this usage message.");
			Console.WriteLine();
		}

		static void Main(string[] args)
		{
			Greet();
			if (args.Length == 0 || args.Length > 2)
			{
				Usage();
				return;
			}
			bool hex = false;
			int fileIndex = 0;

			if (args[0].StartsWith("/") || args[0].StartsWith("-"))
			{
				fileIndex = 1;
				string ar = args[0].ToLower();
				if (ar == "/hex" || ar == "-hex")
				{
					hex = true;
				}
				else
				{
					Usage();
					return;
				}
			}

			VirtualMachine vm = new VirtualMachine();
			try
			{
				if (hex)
				{
					vm.LoadFromHexFile(args[fileIndex]);
				}
				else
				{
					vm.LoadFromBinaryFile(args[fileIndex]);
				}
			}
			catch (Exception)
			{
				Console.WriteLine(string.Format("Could not load file '{0}'", args[fileIndex]));
				return;
			}

			RuntimeException e = vm.Run();
			Console.WriteLine(e.ToString());

			Console.WriteLine();
			Console.WriteLine("Press Enter to exit...");
			Console.ReadLine();
		}
	}
}
