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
	/// Implements the arithmetic instructions of the pC, ADD, SUB, MUL and DIV
	/// </summary>
	public class ArithmeticInstruction : Instruction
	{
		/// <summary>
		/// Holds the instruction type (operation) as defined in the
		/// ArithmeticTypes enumeration.
		/// </summary>
		public ArithmeticTypes Type { get; set; }

		/// <summary>
		/// Uses the instruction type to return a string mnemonic (symbolic name
		/// of the instruction)
		/// </summary>
		public override string Mnemonic
		{
			get
			{
				switch (Type)
				{
					case ArithmeticTypes.Add: return "ADD";
					case ArithmeticTypes.Sub: return "SUB";
					case ArithmeticTypes.Mul: return "MUL";
					case ArithmeticTypes.Div: return "DIV";
					default: return string.Empty;
				}
			}
		}

		/// <summary>
		/// The argument types are used to determine the size.
		/// </summary>
		public override ushort Size
		{
			get
			{
				if (Argument2.Type == ArgumentType.Constant || Argument2.Type == ArgumentType.SymbolConstant
					|| Argument3.Type == ArgumentType.Constant || Argument3.Type == ArgumentType.SymbolConstant)
				{
					return 2;
				}
				else
				{
					return 1;
				}
			}
		}

		#region Argument Checking
		/// <summary>
		/// Checks if the first argument is valid.
		/// </summary>
		/// <remarks>
		/// The first argument of an arithmetic instruction must be a symbol
		/// present in the symbol table and its value must be between [0..7].
		/// </remarks>
		/// <returns>A value indicating if Argument1 is valid</returns>
		protected bool CheckArgument1()
		{
			int value;

			if (!Argument1.LookUpValue(Assembler, out value))
				return false;

			if (value < 0 || value > 7)
			{
				Error = new Error();
				Error.ID = 3;
				Error.Description = string.Format(Messages.E0003, Argument1.Text, 0, 7);
				Error.Line = Line;
				Error.Column = Argument1.Column;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Checks if the second argument is valid.
		/// </summary>
		/// <remarks>
		/// The second argument can be either a constant, or a direct or indirect 3-bit addressing.
		/// 
		/// If the argument is a constant, it's value can be between [-32768..32767]
		/// 
		/// If the argument is a 3-bit address, the value can be between [0..7] only if
		/// the third argument is NOT a constant. Otherwise, it is between [1..7].
		/// This is due to fact that the constant argument is encoded as 0 and it
		/// would be imposible to differentiate it from the 0 address.
		/// (The constant is stored in the second word of the instruction and only one
		/// argument can be a constant.)
		/// </remarks>
		/// <returns>A value indicating if Argument2 is valid</returns>
		protected bool CheckArgument2()
		{
			int value;

			if (!Argument2.LookUpValue(Assembler, out value))
				return false;

			if (Argument2.Type == ArgumentType.Direct || Argument2.Type == ArgumentType.Indirect)
			{
				// Techically, an indirect addressing of the zero address would not be a problem, because the encoded
				// argument would be 1000b, not 0000b like a constant, but pCAS (the original assembler) does not allow this.
				// For compatibility, neither does Messy Lab.
				int min = Argument3.Type == ArgumentType.Constant || Argument3.Type == ArgumentType.SymbolConstant ? 1 : 0;
				if (value < min || value > 7)
				{
					Error = new Error();
					Error.ID = 3;
					Error.Description = string.Format(Messages.E0003, Argument2.Text, min, 7);
					Error.Line = Line;
					Error.Column = Argument2.Column;
					return false;
				}
			}
			else
			{
				if (value < short.MinValue || value > short.MaxValue)
				{
					Error = new Error();
					Error.ID = 2;
					Error.Description = string.Format(Messages.E0002, Argument2.Text);
					Error.Line = Line;
					Error.Column = Argument2.Column;
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Checks if the third argument is valid.
		/// </summary>
		/// <remarks>
		/// The third argument can be either a constant, or a direct or indirect 3-bit addressing.
		/// 
		/// If the argument is a constant, it's value can be between [-32768..32767]
		/// 
		/// If the argument is a 3-bit address, the value can be between [0..7] only if
		/// the second argument is NOT a constant. Otherwise, it is between [1..7].
		/// This is due to fact that the constant argument is encoded as 0 and it
		/// would be imposible to differentiate it from the 0 address.
		/// (The constant is stored in the second word of the instruction and only one
		/// argument can be a constant.)
		/// </remarks>
		/// <returns>A value indicating if Argument3 is valid</returns>
		protected bool CheckArgument3()
		{
			int value;

			if (!Argument3.LookUpValue(Assembler, out value))
				return false;

			if (Argument3.Type == ArgumentType.Direct || Argument3.Type == ArgumentType.Indirect)
			{
				// Techically, an indirect addressing of the zero address would not be a problem, because the encoded
				// argument would be 1000b, not 0000b like a constant, but pCAS (the original assembler) does not allow this.
				// For compatibility, neither does Messy Lab.
				int min = Argument2.Type == ArgumentType.Constant || Argument2.Type == ArgumentType.SymbolConstant ? 1 : 0;
				if (value < min || value > 7)
				{
					Error = new Error();
					Error.ID = 3;
					Error.Description = string.Format(Messages.E0003, Argument3.Text, min, 7);
					Error.Line = Line;
					Error.Column = Argument3.Column;
					return false;
				}
			}
			else
			{
				if (value < short.MinValue || value > short.MaxValue)
				{
					Error = new Error();
					Error.ID = 2;
					Error.Description = string.Format(Messages.E0002, Argument3.Text);
					Error.Line = Line;
					Error.Column = Argument3.Column;
					return false;
				}
			}
			return true;
		}
		#endregion

		#region Partial Translation
		/// <summary>
		/// Translates the first argument to the machine code.
		/// The resulting value contains the argument encoding in the right
		/// position within the word, and zeroes in locations intended for the
		/// OPCode (mnemonic) or other arguments.
		/// </summary>
		/// <returns>A value containing the translation of the argument</returns>
		protected ushort TranslateArgument1()
		{
			int value;
			Argument1.LookUpValue(Assembler, out value);

			// Write address to the lowest 3 bits
			ushort result = (ushort)value;
			if (Argument1.Type == ArgumentType.Indirect)
			{
				// Indirect addressing is indicated by setting the 4th lowest bit.
				result |= 0x8;
			}
			// Move the argument to the right position : 0000aaaa00000000
			result <<= 8;
			return result;
		}

		/// <summary>
		/// Translates the second or third argument to the machine code.
		/// The resulting value contains the argument encoding in the right
		/// position within the word, and zeroes in locations intended for the
		/// OPCode (mnemonic) or other arguments.
		/// </summary>
		/// <param name="argument">the argument for translation</param>
		/// <param name="argumentNumber">the argument number (either 2 or 3)</param>
		/// <param name="constantSet">used to indicate that the argument is a consant, otherwise unchanged</param>
		/// <param name="constant">used to store the constant, if any, otherwise unchanged</param>
		/// <returns>A value containing the translation of the argument</returns>
		protected ushort TranslateArgument23(Argument argument, int argumentNumber, ref bool constantSet, ref ushort constant)
		{
			int value;
			argument.LookUpValue(Assembler, out value);

			if (argument.Type == ArgumentType.Constant || argument.Type == ArgumentType.SymbolConstant)
			{
				constant = (ushort)value;
				constantSet = true;
				// A constant is encoded as 0000b in the Argument field.
				return 0;
			}
			else
			{
				// Write address to the lowest 3 bits
				ushort result = (ushort)value;
				if (argument.Type == ArgumentType.Indirect)
				{
					// Indirect addressing is indicated by setting the 4th lowest bit.
					result |= 0x8;
				}
				// Move the argument to the right position
				// Argument2: 00000000aaaa0000
				// Argument3: 000000000000aaaa
				result <<= (3 - argumentNumber) * 4;
				return result;
			}
		}

		/// <summary>
		/// Translates the mnemonic to the machine Operation code.
		/// The resulting value contains the OPCode in the right
		/// position within the word, and zeroes in locations intended
		/// for the arguments.
		/// </summary>
		/// <param name="containsConstant">A value indicating if a constant argument is present</param>
		/// <returns>A value containing the translation of the mnemonic</returns>
		protected ushort TranslateMnemonic(bool containsConstant)
		{
			ushort word = 0;
			// Store the operation code in the lower 3 bits.
			switch (Type)
			{
				case ArithmeticTypes.Add: word = 1; break;
				case ArithmeticTypes.Sub: word = 2; break;
				case ArithmeticTypes.Mul: word = 3; break;
				case ArithmeticTypes.Div: word = 4; break;
			}
			// If an argument is a constant, the 4th lowest bit needs to be set.
			// In other words each symbolic arithmetic operation uses two different OPCodes.
			if (containsConstant) word |= 8;
			// The OPCode is shifted to the right location: oooo000000000000
			word <<= 12;
			return word;
		}
		#endregion

		/// <summary>
		/// Translates the instruction and sets the Code property.
		/// If an error is detected, Error property is set instead.
		/// </summary>
		/// <returns>A value indicating if the translation was successful</returns>
		public override bool Translate()
		{
			Error = null;
			if (!CheckArgument1()) return false;
			if (!CheckArgument2()) return false;
			if (!CheckArgument3()) return false;

			ushort word1 = 0;
			// Bitwise OR operation is used to pack partial translations to a single word.
			word1 |= TranslateArgument1();
			bool constantSet = false;
			ushort constant = 0;
			word1 |= TranslateArgument23(Argument2, 2, ref constantSet, ref constant);
			word1 |= TranslateArgument23(Argument3, 3, ref constantSet, ref constant);
			word1 |= TranslateMnemonic(constantSet);

			// Depanding on the existance of a constant, the instruction is either one or two words long
			Code = new ushort[constantSet ? 2 : 1];
			Code[0] = word1;
			if (constantSet) Code[1] = constant;

			return true;
		}

	}

	/// <summary>
	/// A list of Arithmetic mnemonics
	/// </summary>
	public enum ArithmeticTypes
	{
		Add,
		Sub,
		Mul,
		Div
	}
}
