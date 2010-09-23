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
	/// Implements the stop instructions of the pC, STOP
	/// </summary>
	public class StopInstruction : Instruction
	{
		/// <summary>
		/// Always returns "STOP".
		/// </summary>
		public override string Mnemonic
		{
			get { return "STOP"; }
		}

		/// <summary>
		/// The size is always 1 word.
		/// </summary>
		public override ushort Size
		{
			get { return 1; }
		}

		/// <summary>
		/// Checks if the argument is valid.
		/// </summary>
		/// <remarks>
		/// Each argument of the stop instruction is optional and may be a symbol
		/// present in the symbol table and its value must be between [0..7] for the
		/// indirect addressing or between [1..7] for direct addressing.
		/// This is because absence of an argument is encoded as 0000b.
		/// </remarks>
		/// <param name="argument">an argument to check</param>
		/// <returns>A value indicating if Argument1 is valid</returns>
		protected bool CheckArgument(Argument argument)
		{
			if (argument.Type == ArgumentType.None) return true;

			int value;

			if (!argument.LookUpValue(Assembler, out value))
				return false;

			// Zero address is allowed for indirect addressing, because
			// it does not conflict with the argument absence.
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
			return true;
		}

		/// <summary>
		/// Translates the argument to the machine code.
		/// The resulting value contains the argument encoding in the right
		/// position within the word, and zeroes in locations intended for the
		/// OPCode (mnemonic) or other arguments.
		/// </summary>
		/// <param name="argument">the argument for translation</param>
		/// <param name="argumentNumber">the argument number</param>
		/// <returns>A value containing the translation of the argument</returns>
		protected ushort TranslateArgument(Argument argument, int argumentNumber)
		{
			if (argument.Type == ArgumentType.None) return 0;

			int value;
			argument.LookUpValue(Assembler, out value);

			// Write address to the lowest 3 bits
			ushort result = (ushort)value;
			if (argument.Type == ArgumentType.Indirect)
			{
				// Indirect addressing is indicated by setting the 4th lowest bit.
				result |= 0x8;
			}
			// Move the argument to the right position
			// Argument1 : 0000aaaa00000000
			// Argument2 : 00000000aaaa0000
			// Argument3 : 000000000000aaaa
			result <<= (3 - argumentNumber) * 4;
			return result;
		}

		/// <summary>
		/// Translates the instruction and sets the Code property.
		/// If an error is detected, Error property is set instead.
		/// </summary>
		/// <returns>A value indicating if the translation was successful</returns>
		public override bool Translate()
		{
			if (!CheckArgument(Argument1)) return false;
			if (!CheckArgument(Argument2)) return false;
			if (!CheckArgument(Argument3)) return false;

			ushort word = 0xF << 12; // The instruction OPCode
			// Bitwise OR operation is used to pack partial translations to a single word.
			word |= TranslateArgument(Argument1, 1);
			word |= TranslateArgument(Argument2, 2);
			word |= TranslateArgument(Argument3, 3);

			Code = new ushort[1];
			Code[0] = word;
			
			return true;
		}
	}
}
