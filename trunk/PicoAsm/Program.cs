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
	/// A minimal command line support implementation.
	/// </summary>
	class Program
	{
		static void Greet()
		{
			Console.WriteLine("Messy Lab picoComputer Assembler, Version 1.0");
			Console.WriteLine("Copyright (c) 2010 by Milos Andjelkovic");
			Console.WriteLine();
		}

		static void Usage()
		{
			Console.WriteLine("Usage: PicoAsm /? | [/hex | /txt] input_file [output_file]");
			Console.WriteLine();
			Console.WriteLine("  /hex        Writes the machine code to pCAS compatible HEX format.");
			Console.WriteLine("              The default is binary.");
			Console.WriteLine();
			Console.WriteLine("  /txt        Writes the result to a human readable text file.");
			Console.WriteLine();
			Console.WriteLine("  /?          Displays this usage message.");
			Console.WriteLine();
		}

		static void Main(string[] args)
		{
			Greet();
			if (args.Length == 0 || args.Length > 3)
			{
				Usage();
				return;
			}
			bool hex = false;
			bool txt = false;
			int firstFileIndex = 0;

			if (args[0].StartsWith("/") || args[0].StartsWith("-"))
			{
				firstFileIndex = 1;
				string ar = args[0].ToLower();
				if (ar == "/hex" || ar == "-hex")
				{
					hex = true;
				}
				else if (ar == "/txt" || ar == "-txt")
				{
					txt = true;
				}
				else
				{
					Usage();
					return;
				}
			}

			Assembler a = new Assembler();
			try
			{
				a.LoadFromFile(args[firstFileIndex]);
			}
			catch (Exception)
			{
				Console.WriteLine(string.Format("Could not load file '{0}'", args[firstFileIndex]));
				return;
			}
			if (!a.Process())
			{
				Console.WriteLine(string.Format("Could not assemble '{0}'", args[firstFileIndex]));
				Console.WriteLine();
				foreach (Error e in a.Errors)
				{
					Console.WriteLine(e.ToString());
				}
				return;
			}
			string outFile;
			if (args.Length == firstFileIndex + 2) // outFile specified in an argument
			{
				outFile = args[firstFileIndex + 1];
			}
			else
			{
				outFile = args[firstFileIndex].ToLower();
				string extension = ".bin";
				if (hex) extension = ".hex";
				if (txt) extension = ".txt";
				outFile = outFile.Replace(".pca", extension);
				if (outFile == args[firstFileIndex].ToLower())
				{
					outFile += extension;
				}
			}

			try
			{
				if (hex)
				{ a.SaveAsHex(outFile); }
				else if (txt)
				{ a.SaveAsText(outFile); }
				else // bin
				{ a.SaveAsBinary(outFile); }
				
				a.DebugInformation.SaveToFile(outFile + ".mldbg");
			}
			catch (Exception)
			{
				Console.WriteLine(string.Format("Could no write file '{0}'", outFile));
				return;
			}
			Console.WriteLine(string.Format("File '{0}' successfully assembled. Result written to '{1}'", args[firstFileIndex], outFile));
		}
	}
}
