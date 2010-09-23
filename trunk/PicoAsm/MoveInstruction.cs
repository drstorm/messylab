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
	/// Implements the move instructions of the pC, MOV
	/// </summary>
	public class MoveInstruction : Instruction
	{
		/// <summary>
		/// Always returns "MOV".
		/// </summary>
		public override string Mnemonic
		{
			get { return "MOV"; }
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
		/// </remarks>
		/// <returns>A value indicating if Argument2 is valid</returns>
		protected bool CheckArgument2()
		{
			int value;

			if (!Argument2.LookUpValue(Assembler, out value))
				return false;

			if (Argument2.Type == ArgumentType.Direct || Argument2.Type == ArgumentType.Indirect)
			{
				if (value < 0 || value > 7)
				{
					Error = new Error();
					Error.ID = 3;
					Error.Description = string.Format(Messages.E0003, Argument2.Text, 0, 7);
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
		/// The third argument can be either a constant or a direct 3-bit addressing.
		/// 
		/// If the argument is a constant, it's value can be between [0..65535]
		/// 
		/// If the argument is a 3-bit address, the value must be between [1..7]
		/// The zero address cannot be used because 0000b is used to indicate
		/// that there is no third argument.
		/// </remarks>
		/// <returns>A value indicating if Argument3 is valid</returns>
		protected bool CheckArgument3()
		{
			int value;

			if (!Argument3.LookUpValue(Assembler, out value))
				return false;

			if (Argument3.Type == ArgumentType.Direct)
			{
				if (value < 1 || value > 7)
				{
					Error = new Error();
					Error.ID = 3;
					Error.Description = string.Format(Messages.E0003, Argument3.Text, 1, 7);
					Error.Line = Line;
					Error.Column = Argument3.Column;
					return false;
				}
			}
			else
			{
				if (value < ushort.MinValue || value > ushort.MaxValue)
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
		/// Translates the second argument to the machine code.
		/// The resulting value contains the argument encoding in the right
		/// position within the word, and zeroes in locations intended for the
		/// OPCode (mnemonic) or other arguments.
		/// </summary>
		/// <param name="constantSet">used to indicate that the argument is a consant, otherwise unchanged</param>
		/// <param name="constant">used to store the constant, if any, otherwise unchanged</param>
		/// <returns>A value containing the translation of the argument</returns>
		protected ushort TranslateArgument2(ref bool constantSet, ref ushort constant)
		{
			int value;
			Argument2.LookUpValue(Assembler, out value);

			if (Argument2.Type == ArgumentType.Constant || Argument2.Type == ArgumentType.SymbolConstant)
			{
				constant = (ushort)value;
				constantSet = true;
				// The argument field content is not defined if it is a constant.
				return 0;
			}
			else
			{
				// Write address to the lowest 3 bits
				ushort result = (ushort)value;
				if (Argument2.Type == ArgumentType.Indirect)
				{
					// Indirect addressing is indicated by setting the 4th lowest bit.
					result |= 0x8;
				}
				// Move the argument to the right position : 00000000aaaa0000
				result <<= 4;
				return result;
			}
		}

		/// <summary>
		/// Translates the third argument to the machine code. If there is no
		/// third argument, its argument field is used as an extension to the
		/// OPCode to indicate if the second argument is a constant.
		/// 
		/// The resulting value contains the argument encoding in the right
		/// position within the word, and zeroes in locations intended for the
		/// OPCode (mnemonic) or other arguments.
		/// </summary>
		/// <param name="constantSet">used to indicate that the argument is a consant, otherwise unchanged</param>
		/// <param name="constant">used to store the constant, if any, otherwise unchanged</param>
		/// <returns>A value containing the translation of the argument</returns>
		protected ushort TranslateArgument3(ref bool constantSet, ref ushort constant)
		{
			if (Argument3.Type == ArgumentType.None)
			{
				// If there is no third argument, its field is used to indicate
				// the type of the second one (1000b if it is a constant, or 0000b
				// if it is a 3-bit address).
				if (constantSet) return 8;
				else return 0;
			}
			else
			{
				int value;
				Argument3.LookUpValue(Assembler, out value);

				if (Argument3.Type == ArgumentType.Direct)
				{
					// The address is already in the lowest 3 bits.
					return (ushort)value;
				}
				else // A constant
				{
					constantSet = true;
					constant = (ushort)value;
					// A constant as the third argument is indicated by 1111b value
					// in its argument field.
					return 0xF;
				}
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
			if (!CheckArgument1()) return false;
			if (!CheckArgument2()) return false;
			if (!CheckArgument3()) return false;

			ushort word1 = 0;
			// Bitwise OR operation is used to pack partial translations to a single word.
			word1 |= TranslateArgument1();
			bool constantSet = false;
			ushort constant = 0;
			word1 |= TranslateArgument2(ref constantSet, ref constant);
			word1 |= TranslateArgument3(ref constantSet, ref constant);

			// The instruction OPCode for MOV is 0000b, so it does not need explicit setting.

			// Depanding on the existance of a constant, the instruction is either one or two words long
			Code = new ushort[constantSet ? 2 : 1];
			Code[0] = word1;
			if (constantSet) Code[1] = constant;

			return true;
		}

	}
}
