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
	/// Implements the branch instructions of the pC, BEQ and BGT
	/// </summary>
	public class BranchInstruction : Instruction
	{
		/// <summary>
		/// Holds tye instruction type (operation) as defined in the
		/// BranchTypes enumeration.
		/// </summary>
		public BranchTypes Type { get; set; }

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
					case BranchTypes.Beq: return "BEQ";
					case BranchTypes.Bgt: return "BGT";
					default: return string.Empty;
				}
			}
		}

		/// <summary>
		/// The third argument type is used to determine the size.
		/// </summary>
		public override ushort Size
		{
			get
			{
				if (Argument3.Type == ArgumentType.Direct)
				{
					return 2;
				}
				else // Indirect
				{
					return 1;
				}
			}
		}

		#region Argument Checking
		/// <summary>
		/// Checks if the first or second argument is valid.
		/// </summary>
		/// <remarks>
		/// The argument can be either a constant, or a direct or indirect 3-bit addressing.
		/// 
		/// If the argument is a constant, it's value must be zero (encoded as 0000b in the
		/// address field, without an additional word).
		/// 
		/// If the argument is a 3-bit address, the value can be between [0..7] for the
		/// indirect addressing or between [1..7] for direct addressing.
		/// This is due to fact that the constant argument would be indistinguishable from
		/// direct addressing the zero address (0000b).
		/// </remarks>
		/// <param name="argument">an argument to check (either the first of the second)</param>
		/// <returns>A value indicating if the argument is valid</returns>
		protected bool CheckArgument12(Argument argument)
		{
			int value;

			if (!argument.LookUpValue(Assembler, out value))
				return false;

			if (argument.Type == ArgumentType.Constant)
			{
				if (value != 0)
				{
					Error = new Error();
					Error.ID = 4;
					Error.Description = Messages.E0004;
					Error.Line = Line;
					Error.Column = argument.Column;
					return false;
				}
			}
			else
			{
				// Zero address is allowed for indirect addressing, because
				// it does not conflict with the zero constant.
				int min = (argument.Type == ArgumentType.Indirect) ? 0 : 1;
				if (value < min || value > 7)
				{
					Error = new Error();
					Error.ID = 3;
					Error.Description = string.Format(Messages.E0003, argument.Text, min, 7);
					Error.Line = Line;
					Error.Column = argument.Column;
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Checks if the third argument is valid.
		/// </summary>
		/// <remarks>
		/// The third argument represents the branch destination address, and uses direct
		/// or indirect addressing. However, it is not translated in the same manner that
		/// applies to the 3-bit addressing in other instructions.
		/// 
		/// If the direct addressing is used, the value of the symbol is considered the
		/// branch destination and is stored in a separate word as a constant. The value
		/// must be a valid 16-bit address [0..65535].
		/// 
		/// If the indirect addressing is used, the value must be a 3-bit address [0..7]
		/// </remarks>
		/// <returns>A value indicating if Argument3 is valid</returns>
		protected bool CheckArgument3()
		{
			int value;

			if (!Argument3.LookUpValue(Assembler, out value))
				return false;

			if (Argument3.Type == ArgumentType.Direct)
			{
				if (value < ushort.MinValue || value > ushort.MaxValue)
				{
					Error = new Error();
					Error.ID = 3;
					Error.Description = string.Format(Messages.E0003, Argument3.Text, ushort.MinValue, ushort.MaxValue);
					Error.Line = Line;
					Error.Column = Argument3.Column;
					return false;
				}
			}
			else
			{
				if (value < 0 || value > 7)
				{
					Error = new Error();
					Error.ID = 3;
					Error.Description = string.Format(Messages.E0003, Argument3.Text, 0, 7);
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
		/// Translates the first or second argument to the machine code.
		/// The resulting value contains the argument encoding in the right
		/// position within the word, and zeroes in locations intended for the
		/// OPCode (mnemonic) or other arguments.
		/// </summary>
		/// <param name="argument">the argument for translation</param>
		/// <param name="argumentNumber">the argument number (either 1 or 2)</param>
		/// <returns>A value containing the translation of the argument</returns>
		protected ushort TranslateArgument12(Argument argument, int argumentNumber)
		{
			int value;
			argument.LookUpValue(Assembler, out value);

			// This is valid for 3-bit adressing and for the constants, because
			// zero is the only valid constant.
			// The value fits in the lowest 3 bits.
			ushort result = (ushort)value;
			
			if (argument.Type == ArgumentType.Indirect)
			{
				// Indirect addressing is indicated by setting the 4th lowest bit.
				result |= 0x8;
			}
			// Move the argument to the right position
			// Argument1: 0000aaaa00000000
			// Argument2: 00000000aaaa0000
			result <<= (3 - argumentNumber) * 4;
			return result;
		}

		/// <summary>
		/// Translates the third argument to the machine code.
		/// The resulting value contains the argument encoding in the right
		/// position within the word, and zeroes in locations intended for the
		/// OPCode (mnemonic) or other arguments.
		/// </summary>
		/// <param name="constantSet">used to indicate that the argument is a consant, otherwise unchanged</param>
		/// <param name="constant">used to store the constant, if any, otherwise unchanged</param>
		/// <returns>A value containing the translation of the Argument3</returns>
		protected ushort TranslateArgument3(ref bool constantSet, ref ushort constant)
		{
			int ivalue;
			Argument3.LookUpValue(Assembler, out ivalue);

			ushort value = (ushort)ivalue;
			if (Argument3.Type == ArgumentType.Direct)
			{
				// If the direct addressing is used, the argument translates as 1000b
				// and the actual value is stored in a second word.
				constant = value;
				constantSet = true;
				// No shifting is required because the third argument uses the 4 lowest bits
				return 0x8;
			}
			else // Indirect
			{
				// Unlike for other instructions, indirect addressing is not marked by setting the
				// 4th lowest bit.
				return value;
			}
		}

		/// <summary>
		/// Translates the mnemonic to the machine Operation code.
		/// The resulting value contains the OPCode in the right
		/// position within the word, and zeroes in locations intended
		/// for the arguments.
		/// </summary>
		/// <returns>A value containing the translation of the mnemonic</returns>
		protected ushort TranslateMnemonic()
		{
			switch (Type)
			{
				case BranchTypes.Beq: return 0x5 << 12;
				case BranchTypes.Bgt: return 0x6 << 12;
				default: return 0;
			}
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
			if (!CheckArgument12(Argument1)) return false;
			if (!CheckArgument12(Argument2)) return false;
			if (!CheckArgument3()) return false;

			ushort word1 = 0;
			// Bitwise OR operation is used to pack partial translations to a single word.
			word1 |= TranslateArgument12(Argument1, 1);
			word1 |= TranslateArgument12(Argument2, 2);
			bool constantSet = false;
			ushort constant = 0;
			word1 |= TranslateArgument3(ref constantSet, ref constant);
			word1 |= TranslateMnemonic();

			// Depanding on the existance of a constant, the instruction is either one or two words long
			Code = new ushort[constantSet ? 2 : 1];
			Code[0] = word1;
			if (constantSet) Code[1] = constant;

			return true;
		}

	}

	/// <summary>
	/// A list of Branch mnemonics
	/// </summary>
	public enum BranchTypes
	{
		Beq,
		Bgt
	}
}
