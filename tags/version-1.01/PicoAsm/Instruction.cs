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
	/// Includes members common to all instructions.
	/// </summary>
	public abstract class Instruction
	{
		public Instruction()
		{
			Label = string.Empty;
			_argument1.ParentInstruction = this;
			_argument2.ParentInstruction = this;
			_argument3.ParentInstruction = this;
		}

		/// <summary>
		/// Instruction symbolic name, mnemonic
		/// </summary>
		public abstract string Mnemonic { get; }
		
		/// <summary>
		/// The First Argument
		/// </summary>
		public Argument Argument1 { get { return _argument1; } set { _argument1 = value; _argument1.ParentInstruction = this; } }
		private Argument _argument1;

		/// <summary>
		/// The Second Argument
		/// </summary>
		public Argument Argument2 { get { return _argument2; } set { _argument2 = value; _argument2.ParentInstruction = this; } }
		private Argument _argument2;
	
		/// <summary>
		/// The Third Argument
		/// </summary>
		public Argument Argument3 { get { return _argument3; } set { _argument3 = value; _argument3.ParentInstruction = this; } }
		private Argument _argument3;

		/// <summary>
		/// Memory address of the instruction
		/// </summary>
		public ushort Address { get; set; }

		/// <summary>
		/// Label attached to the instruction
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// The size of the machine language representation of the instruction (either 1 or 2 words).
		/// </summary>
		public abstract ushort Size { get; }

		/// <summary>
		/// Translates the instruction to machine code or sets the Error
		/// property.
		/// </summary>
		/// <returns>A value indicating if the translation was successful</returns>
		public abstract bool Translate();

		/// <summary>
		/// The Translation Error, set by the Translate method.
		/// </summary>
		public Error Error { get; set; }

		/// <summary>
		/// The generated machine code, set by the Translate method.
		/// </summary>
		public ushort[] Code { get { return _code; } protected set { _code = value; } }
		private ushort[] _code;

		/// <summary>
		/// Line containing the instruction in the source code
		/// </summary>
		public int Line { get; set; }

		/// <summary>
		/// Column of the instruction start in the source code
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// A reference to the Assembler object used to store parsed data.
		/// </summary>
		public Assembler Assembler { get; set; }

		/// <summary>
		/// Formats the instruction as string.
		/// <example>
		/// ADD (x), x, 2
		/// </example>
		/// </summary>
		/// <returns>A string representing the instruction</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Mnemonic);
			sb.Append(' ');
			sb.Append(Argument1.ToString());
			if (Argument2.Type != ArgumentType.None)
			{
				sb.Append(", ");
				sb.Append(Argument2.ToString());
			}
			if (Argument3.Type != ArgumentType.None)
			{
				sb.Append(", ");
				sb.Append(Argument3.ToString());
			}
			return sb.ToString();
		}

		/// <summary>
		/// Formats the instruction machine code as a hex string.
		/// </summary>
		/// <returns>A string representing the machine code of the instruction</returns>
		public string CodeToString()
		{
			if (Code == null) return string.Empty;
			StringBuilder sb = new StringBuilder();
			if (Code.Length >= 1)
				sb.Append(string.Format("{0:X4}", Code[0]));
			if (Code.Length == 2)
				sb.Append(string.Format(" {0:X4}", Code[1]));
			return sb.ToString();
		}
	}

	/// <summary>
	/// A structure holding information about instruction argument.
	/// </summary>
	public struct Argument
	{
		/// <summary>
		/// The symbol or a constant, without '#' or parens
		/// </summary>
		public string Text;
		/// <summary>
		/// Argument can be either a Constant, SymbolicConstant, Direct or Indirect
		/// depending on the addressing mode
		/// </summary>
		public ArgumentType Type;

		/// <summary>
		/// A reference to the parent instruction
		/// </summary>
		public Instruction ParentInstruction;

		/// <summary>
		/// Column of the Argument in the source code
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// Formats the argument as a string.
		/// <example>
		/// (x)
		/// </example>
		/// </summary>
		/// <returns>A string representing the argument</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			if (Type == ArgumentType.SymbolConstant) sb.Append('#');
			if (Type == ArgumentType.Indirect) sb.Append('(');
			sb.Append(Text);
			if (Type == ArgumentType.Indirect) sb.Append(')');
			return sb.ToString();
		}

		/// <summary>
		/// Gets the numeric value of the Argument either by parsing it as an integer
		/// (in case of a Constant), or by looking up the value in the Symbol table.
		/// 
		/// If the symbol cannot be found, an Error is filed.
		/// </summary>
		/// <param name="assembler">A reference to the Assembler object holding Symbol table</param>
		/// <param name="value">A integer representing the argument's numeric value</param>
		/// <returns>A value indicating if the look up has been successful</returns>
		public bool LookUpValue(Assembler assembler, out int value)
		{
			switch (Type)
			{
				case ArgumentType.Constant:
					value = int.Parse(Text);
					break;
				case ArgumentType.SymbolConstant:
				case ArgumentType.Direct:
				case ArgumentType.Indirect:
					try
					{
						value = assembler.Symbols[Text];
					}
					catch (Exception)
					{
						Error e = new Error();
						e.ID = 7;
						e.Description = string.Format(Messages.E0007, Text);
						e.Line = ParentInstruction.Line;
						e.Column = Column;
						ParentInstruction.Error = e;

						value = 0;
						return false;
					}
					break;
				default:
					value = 0;
					break;
			}
			return true;
		}
	}

	public enum ArgumentType
	{
		/// <summary>
		/// No Argument
		/// </summary>
		None,
		/// <summary>
		/// Example: 12
		/// </summary>
		Constant,
		/// <summary>
		/// Example: #x
		/// </summary>
		SymbolConstant,
		/// <summary>
		/// Example: x
		/// </summary>
		Direct,
		/// <summary>
		/// Example: (x)
		/// </summary>
		Indirect
	}
}
