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
using PerCederberg.Grammatica.Runtime;
using System.Collections;
using System.IO;
using MessyLab.Debugger;

namespace MessyLab.PicoComputer
{
	/// <summary>
	/// An assembler implementation for the picoComputer designed by Jozo Dujmovic (c) 1989
	/// </summary>
	public class Assembler
	{
		#region Constructors
		public Assembler()
		{
			Code = string.Empty;
			Filename = string.Empty;
		}

		public Assembler(string code)
		{
			Code = code;
			Filename = string.Empty;
		}
		#endregion

		#region Private fields
		private List<Instruction> _instructions = new List<Instruction>();
		private List<Error> _errors = new List<Error>();
		private Dictionary<string, int> _symbols = new Dictionary<string, int>();
		private DebugInformation _debugInformation = new DebugInformation();
		#endregion

		#region Properties
		/// <summary>
		/// The code to assemble
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// The name of the loaded file.
		/// </summary>
		/// <remarks>
		/// Used in the Debug Information.
		/// </remarks>
		public string Filename { get; set; }

		/// <summary>
		/// A list of errors
		/// </summary>
		public List<Error> Errors { get { return _errors; } }

		/// <summary>
		/// Holds a list of instructions as read from the input code.
		/// </summary>
		public List<Instruction> Instructions { get { return _instructions; } }

		/// <summary>
		/// Symbol Table
		/// </summary>
		public Dictionary<string, int> Symbols { get { return _symbols; } }

		/// <summary>
		/// Debug Information for Messy Lab Debugger
		/// </summary>
		/// <remarks>
		/// Contains the defined symbolic constants and instruction lines and addresses.
		/// </remarks>
		public DebugInformation DebugInformation { get { return _debugInformation; } }

		/// <summary>
		/// The address of the next instruction. The initial value is specified in the code
		/// using the ORG directive. The value is used to calculate label values during
		/// parsing.
		/// </summary>
		protected ushort NextInstructionAddress { get; set; }
		#endregion

		/// <summary>
		/// Loads the assembler program from file or throws an exception.
		/// </summary>
		/// <param name="filename">Full path to the text file containing the source code</param>
		public void LoadFromFile(string filename)
		{
			string[] s = filename.Split(new char[] { Path.DirectorySeparatorChar });
			Filename = s[s.Length - 1];
			Code = File.ReadAllText(filename);
		}

		/// <summary>
		/// Parses the code from the Code property and files syntax errors if any.
		/// </summary>
		protected void Parse()
		{
			// The parser expects each instruction to end with '\n'.
			if (!Code.EndsWith("\n")) Code += "\n";

			Analyzer a = new Analyzer(this);
			Parser p = new PicoParser(new StringReader(Code), a);
			try
			{
				Node n = p.Parse();
			}
			catch (ParserLogException ex)
			{
				for (int i = 0; i < ex.Count; i++)
				{
					ParseException e = ex[i];
					Error err = new Error();
					err.ID = 0;
					err.Description = string.Format(Messages.E0000, e.ErrorMessage);
					err.Line = e.Line;
					err.Column = e.Column;
					Errors.Add(err);
				}
			}
		}

		/// <summary>
		/// Translates all instructions and files errors if any.
		/// </summary>
		protected void Translate()
		{
			foreach (Instruction i in Instructions)
			{
				if (!i.Translate())
				{
					Errors.Add(i.Error);
				}
			}
		}

		/// <summary>
		/// Assembles the Code and populates Instructions, Symbols and Errors.
		/// </summary>
		/// <returns>A value indicating if the assembly has been successful</returns>
		public bool Process()
		{
			Instructions.Clear();
			Symbols.Clear();
			Errors.Clear();
			DebugInformation.Clear();
			
			Parse();
			if (Errors.Count > 0)
				return false;

			Translate();
			if (Errors.Count > 0)
				return false;

			return true;
		}

		#region Export
		/// <summary>
		/// Exports the machine code as HEX strings.
		/// </summary>
		/// <remarks>
		/// The first string is the address of the first instruction (origin).
		/// Each word of the machine code is a separate string.
		/// 
		/// The format is compatible with the original simulator, pCAS.
		/// </remarks>
		/// <returns>Strings representing the machine code in HEX format</returns>
		public string[] ExportToHex()
		{
			if (Instructions.Count == 0) return new string[0];
			
			List<string> result = new List<string>();
			result.Add(string.Format("{0:X4}", Instructions[0].Address));
			foreach (Instruction i in Instructions)
			{
				foreach (ushort c in i.Code)
				{
					result.Add(string.Format("{0:X4}", c));
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Exports the machine code.
		/// </summary>
		/// <remarks>
		/// The first word is the address of the first instruction (origin).
		/// </remarks>
		/// <returns>Machine code with the first word being the starting address</returns>
		public ushort[] ExportToBinary()
		{
			if (Instructions.Count == 0) return new ushort[0];

			List<ushort> result = new List<ushort>();
			result.Add(Instructions[0].Address);
			foreach (Instruction i in Instructions)
			{
				result.AddRange(i.Code);
			}
			return result.ToArray();
		}

		/// <summary>
		/// Exports the text representation of the translated code.
		/// </summary>
		/// <returns>Text representation of the code</returns>
		public string[] ExportToText()
		{
			string[] result = new string[Symbols.Count + Instructions.Count];
			int c = 0;

			// Symbol table
			foreach (string sym in Symbols.Keys)
			{
				result[c++] = string.Format("                  {0} = {1}", sym, Symbols[sym]);
			}
			// Instructions
			foreach (Instruction i in Instructions)
			{
				string ins = i.ToString();
				if (!string.IsNullOrEmpty(i.Label))
				{
					ins = i.Label + ": " + ins;
				}
				// Example : "   99: 0108 01F2  label: MOV a, #adr"
				ins = string.Format("{0,5}: {1,-10} {2}", i.Address, i.CodeToString(), ins);
				result[c++] = ins;
			}
			return result;
		}

		/// <summary>
		/// Saves the machine code to file.
		/// </summary>
		/// <param name="path">Full path to the file to save.</param>
		public void SaveAsBinary(string path)
		{
			FileStream fs = File.Create(path);
			BinaryWriter b = new BinaryWriter(fs);
			foreach (ushort u in ExportToBinary())
			{
				b.Write(u);
			}
			b.Close();
			fs.Close();
		}

		/// <summary>
		/// Saves the machine code as HEX file.
		/// </summary>
		/// <param name="path">Full path to the file to save.</param>
		public void SaveAsHex(string path)
		{
			File.WriteAllLines(path, ExportToHex());
		}

		/// <summary>
		/// Saves the text representation of the translated code.
		/// </summary>
		/// <param name="path">Full path to the file to save.</param>
		public void SaveAsText(string path)
		{
			File.WriteAllLines(path, ExportToText());
		}
		#endregion

		/// <summary>
		/// Adds the specified symbol to the symbol table and files an error (E0001)
		/// if the symbol is already defined.
		/// </summary>
		/// <param name="name">The symbol name</param>
		/// <param name="value">The value</param>
		/// <param name="line">The Line in the source code, used for filing errors</param>
		/// <param name="column">The Column in the source code, used for filing errors</param>
		/// <param name="isLabel">Indicates whether the symbol is an Instruction label</param>
		protected void AddSymbol(string name, int value, int line, int column, bool isLabel)
		{
			try
			{
				Symbols.Add(name, value);
				DebugInformation.AddSymbol(Filename, line, name, value, isLabel);
			}
			catch (ArgumentException)
			{
				Error e = new Error();
				e.ID = 1;
				e.Line = line;
				e.Column = column;
				e.Description = string.Format(Messages.E0001, name);
				Errors.Add(e);
			}
		}

		/// <summary>
		/// Implements a Grammatica Analyzer. It retrieves values from the parse tree
		/// and stores then in the appropriate Assembler fields.
		/// </summary>
		class Analyzer : PicoAnalyzer
		{
			/// <summary>
			/// A reference to the Assembler object used to store parsed data.
			/// </summary>
			protected Assembler Assembler;

			public Analyzer(Assembler assembler)
			{
				Assembler = assembler;
			}

			#region Primitives
			/// <summary>
			/// Stores the identifier to the Node Values for later use.
			/// </summary>
			public override Node ExitIdentifier(Token node)
			{
				node.Values.Add(node.Image.ToLower());
				return node;
			}

			/// <summary>
			/// Files an out of range error (E0002).
			/// </summary>
			/// <param name="value">The value that's out of range</param>
			/// <param name="line">The Line in the source code, used for filing errors</param>
			/// <param name="column">The Column in the source code, used for filing errors</param>
			protected void AddOutOfRangeError(string value, int line, int column)
			{
				Error e = new Error();
				e.ID = 2;
				e.Line = line;
				e.Column = column;
				e.Description = string.Format(Messages.E0002, value);
				Assembler.Errors.Add(e);
			}

			/// <summary>
			/// Parses an unsigned number and stores it as a node value for later use.
			/// If the number is out of range for ushort, it files an error.
			/// </summary>
			public override Node ExitNumber(Token node)
			{
				bool error = false;
				int value = 0;

				try { value = int.Parse(node.Image); }
				catch (OverflowException) { error = true; }

				if (value > ushort.MaxValue)
				{
					value = 0;
					error = true;
				}

				if (error)
				{
					AddOutOfRangeError(node.Image, node.StartLine, node.StartColumn);
				}
				node.Values.Add(value);
				return node;
			}

			/// <summary>
			/// Stores the number sign as a node value for later use.
			/// </summary>
			public override Node ExitSign(Token node)
			{
				node.Values.Add(node.Image);
				return node;
			}

			/// <summary>
			/// Uses values stored in Number and Sign child nodes to parse an integer
			/// The parsed integer is stored as a node value for later use.
			/// If the integer is lower than short.MinValue, an Out of Range error is
			/// filed.
			/// </summary>
			public override Node ExitInteger(Production node)
			{
				ArrayList values = GetChildValues(node);
				int value = values.Count == 1 ?
					(int)values[0] // No Sign
					: int.Parse((string)values[0] + values[1]);
				if (value < short.MinValue)
				{
					AddOutOfRangeError(value.ToString(), node.StartLine, node.StartColumn);
					value = short.MinValue;
				}
				node.Values.Add(value);
				return node;
			}
			#endregion

			#region Symbols
			/// <summary>
			/// Uses the values from the child nodes to add a new symbol entry.
			/// </summary>
			public override Node ExitSymbol(Production node)
			{
				ArrayList values = GetChildValues(node);
				Assembler.AddSymbol((string)values[0], (int)values[1], node.StartLine, node.StartColumn, false);
				return node;
			}

			/// <summary>
			/// Adds labels to the Symbol Table and increments the NextInstructionAddress
			/// by the Size of the current instruction.
			/// </summary>
			public override Node ExitLine(Production node)
			{
				Instruction lastIns = Assembler.Instructions[Assembler.Instructions.Count - 1];
				
				// Add label if any
				if (node[0].Id == (int)PicoConstants.IDENTIFIER)
				{
					ArrayList values = GetChildValues(node);
					string label = (string)values[0];
					Assembler.AddSymbol(label, Assembler.NextInstructionAddress, node.StartLine, node.StartColumn, true);
					lastIns.Label = label;
				}
				else
				{
					// Debug Information about instructions without labels
					Assembler.DebugInformation.AddSymbol(Assembler.Filename, node.StartLine, string.Empty, Assembler.NextInstructionAddress, true);
				}

				// Increment NextInstructionAddress
				ushort oldAdd = Assembler.NextInstructionAddress;
				Assembler.NextInstructionAddress += lastIns.Size;
				if (Assembler.NextInstructionAddress < oldAdd) // Overflow check
				{
					Error e = new Error();
					e.ID = 6;
					e.Description = Messages.E0006;
					e.Line = node.StartLine;
					e.Column = node.StartColumn;
					Assembler.Errors.Add(e);
				}
				return node;
			}
			#endregion

			#region Origin
			/// <summary>
			/// Sets the ORG address as the address of the first instruction.
			/// </summary>
			public override Node ExitOrigin(Production node)
			{
				ArrayList values = GetChildValues(node);
				int i = (int)values[0];
				Assembler.NextInstructionAddress = (ushort)i;
				return node;
			}
			#endregion

			#region Instructions

			#region Arguments
			/// <summary>
			/// Creates and populates an Argument object.
			/// </summary>
			/// <param name="node">Argument parser node</param>
			/// <param name="type">Argument type</param>
			/// <returns>Created Argument</returns>
			protected Argument ArgumentFromNode(Production node, ArgumentType type)
			{
				ArrayList values = GetChildValues(node);
				Argument a = new Argument();
				a.Column = node.StartColumn;
				a.Text = values[0].ToString();
				a.Type = type;
				return a;
			}

			/// <summary>
			/// Stores an Argument object to the Node Values.
			/// </summary>
			public override Node ExitArg1(Production node)
			{
				node.Values.Add(ArgumentFromNode(node, ArgumentType.Constant));
				return node;
			}

			/// <summary>
			/// Stores an Argument object to the Node Values.
			/// </summary>
			public override Node ExitArg2(Production node)
			{
				node.Values.Add(ArgumentFromNode(node, ArgumentType.SymbolConstant));
				return node;
			}

			/// <summary>
			/// Stores an Argument object to the Node Values.
			/// </summary>
			public override Node ExitArg3(Production node)
			{
				node.Values.Add(ArgumentFromNode(node, ArgumentType.Direct));
				return node;
			}

			/// <summary>
			/// Stores an Argument object to the Node Values.
			/// </summary>
			public override Node ExitArg4(Production node)
			{
				node.Values.Add(ArgumentFromNode(node, ArgumentType.Indirect));
				return node;
			}

			/// <summary>
			/// Propagates Child Node Values.
			/// </summary>
			public override Node ExitArg12(Production node)
			{
				node.Values.AddRange(GetChildValues(node));
				return node;
			}

			/// <summary>
			/// Propagates Child Node Values.
			/// </summary>
			public override Node ExitArg123(Production node)
			{
				node.Values.AddRange(GetChildValues(node));
				return node;
			}

			/// <summary>
			/// Propagates Child Node Values.
			/// </summary>
			public override Node ExitArg1234(Production node)
			{
				node.Values.AddRange(GetChildValues(node));
				return node;
			}

			/// <summary>
			/// Propagates Child Node Values.
			/// </summary>
			public override Node ExitArg34(Production node)
			{
				node.Values.AddRange(GetChildValues(node));
				return node;
			}

			/// <summary>
			/// Propagates Child Node Values.
			/// </summary>
			public override Node ExitMoveArgs(Production node)
			{
				node.Values.AddRange(GetChildValues(node));
				return node;
			}

			/// <summary>
			/// Propagates Child Node Values.
			/// </summary>
			public override Node ExitArithmeticArgs(Production node)
			{
				node.Values.AddRange(GetChildValues(node));
				return node;
			}

			/// <summary>
			/// Propagates Child Node Values.
			/// </summary>
			public override Node ExitBranchArgs(Production node)
			{
				node.Values.AddRange(GetChildValues(node));
				return node;
			}

			/// <summary>
			/// Propagates Child Node Values.
			/// </summary>
			public override Node ExitIoargs(Production node)
			{
				node.Values.AddRange(GetChildValues(node));
				return node;
			}

			/// <summary>
			/// Propagates Child Node Values.
			/// </summary>
			public override Node ExitEndArgs(Production node)
			{
				node.Values.AddRange(GetChildValues(node));
				return node;
			}

			#endregion

			/// <summary>
			/// Creates a new MoveInstruction.
			/// </summary>
			public override Node ExitMove(Production node)
			{
				ArrayList values = GetChildValues(node);
				MoveInstruction i = new MoveInstruction();
				i.Address = Assembler.NextInstructionAddress;
				i.Argument1 = (Argument)values[0];
				i.Argument2 = (Argument)values[1];
				if (values.Count == 3) i.Argument3 = (Argument)values[2];
				i.Line = node.StartLine;
				i.Column = node.StartColumn;
				i.Assembler = Assembler;
				Assembler.Instructions.Add(i);
				return node;
			}

			/// <summary>
			/// Creates a new ArithmeticInstruction.
			/// </summary>
			public override Node ExitArithmetic(Production node)
			{
				ArrayList values = GetChildValues(node);
				ArithmeticInstruction i = new ArithmeticInstruction();
				switch (node[0].Id)
				{
					case (int)PicoConstants.ADD: i.Type = ArithmeticTypes.Add; break;
					case (int)PicoConstants.SUB: i.Type = ArithmeticTypes.Sub; break;
					case (int)PicoConstants.MUL: i.Type = ArithmeticTypes.Mul; break;
					case (int)PicoConstants.DIV: i.Type = ArithmeticTypes.Div; break;
				}
				i.Address = Assembler.NextInstructionAddress;
				i.Argument1 = (Argument)values[0];
				i.Argument2 = (Argument)values[1];
				i.Argument3 = (Argument)values[2];
				i.Line = node.StartLine;
				i.Column = node.StartColumn;
				i.Assembler = Assembler;
				Assembler.Instructions.Add(i);
				return node;
			}

			/// <summary>
			/// Creates a new BranchInstruction.
			/// </summary>
			public override Node ExitBranch(Production node)
			{
				ArrayList values = GetChildValues(node);
				BranchInstruction i = new BranchInstruction();
				switch (node[0].Id)
				{
					case (int)PicoConstants.BEQ: i.Type = BranchTypes.Beq; break;
					case (int)PicoConstants.BGT: i.Type = BranchTypes.Bgt; break;
				}
				i.Address = Assembler.NextInstructionAddress;
				i.Argument1 = (Argument)values[0];
				i.Argument2 = (Argument)values[1];
				i.Argument3 = (Argument)values[2];
				i.Line = node.StartLine;
				i.Column = node.StartColumn;
				i.Assembler = Assembler;
				Assembler.Instructions.Add(i);
				return node;
			}

			/// <summary>
			/// Creates a new IOInstruction.
			/// </summary>
			public override Node ExitIo(Production node)
			{
				ArrayList values = GetChildValues(node);
				IOInstruction i = new IOInstruction();
				switch (node[0].Id)
				{
					case (int)PicoConstants.IN: i.Type = IOTypes.In; break;
					case (int)PicoConstants.OUT: i.Type = IOTypes.Out; break;
				}
				i.Address = Assembler.NextInstructionAddress;
				i.Argument1 = (Argument)values[0];
				if (values.Count == 2) i.Argument2 = (Argument)values[1];
				i.Line = node.StartLine;
				i.Column = node.StartColumn;
				i.Assembler = Assembler;
				Assembler.Instructions.Add(i);
				return node;
			}

			/// <summary>
			/// Creates a new CallInstruction.
			/// </summary>
			public override Node ExitCall(Production node)
			{
				ArrayList values = GetChildValues(node);
				CallInstruction i = new CallInstruction();
				i.Address = Assembler.NextInstructionAddress;
				i.Argument1 = (Argument)values[0];
				i.Line = node.StartLine;
				i.Column = node.StartColumn;
				i.Assembler = Assembler;
				Assembler.Instructions.Add(i);
				return node;
			}

			/// <summary>
			/// Creates a new ReturnInstruction.
			/// </summary>
			public override Node ExitReturn(Production node)
			{
				ReturnInstruction i = new ReturnInstruction();
				i.Address = Assembler.NextInstructionAddress;
				i.Line = node.StartLine;
				i.Column = node.StartColumn;
				i.Assembler = Assembler;
				Assembler.Instructions.Add(i);
				return node;
			}

			/// <summary>
			/// Creates a new StopInstruction.
			/// </summary>
			/// <remarks>
			/// The grammar production is named "End" to avoid a conflict
			/// with token "STOP".
			/// </remarks>
			public override Node ExitEnd(Production node)
			{
				ArrayList values = GetChildValues(node);
				StopInstruction i = new StopInstruction();
				i.Address = Assembler.NextInstructionAddress;
				if (values.Count > 0) i.Argument1 = (Argument)values[0];
				if (values.Count > 1) i.Argument2 = (Argument)values[1];
				if (values.Count > 2) i.Argument3 = (Argument)values[2];
				i.Line = node.StartLine;
				i.Column = node.StartColumn;
				i.Assembler = Assembler;
				Assembler.Instructions.Add(i);
				return node;
			}
			#endregion

		}
	}
}
