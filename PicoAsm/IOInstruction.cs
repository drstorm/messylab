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
	/// Implements the I/O instructions of the pC, IN and OUT
	/// </summary>
	public class IOInstruction : Instruction
	{
		/// <summary>
		/// Holds tye instruction type (operation) as defined in the
		/// IOTypes enumeration.
		/// </summary>
		public IOTypes Type { get; set; }

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
					case IOTypes.In: return "IN";
					case IOTypes.Out: return "OUT";
					default: return string.Empty;
				}
			}
		}

		/// <summary>
		/// The size is always 1 word.
		/// </summary>
		public override ushort Size
		{
			get { return 1; }
		}

		#region Argument Checking
		/// <summary>
		/// Checks if the first argument is valid.
		/// </summary>
		/// <remarks>
		/// The first argument of an I/O instruction must be a symbol
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
		/// If the argument is a constant, its value must be between [0..127] because there are
		/// 7 bits reserved for storing the constant.
		/// These instructions have a maximum of two arguments, so the second argument is stored in eight
		/// instead of the usual four bits.
		/// 
		/// If the argument is a 3-bit address, the value must be between [0..7]
		/// </remarks>
		/// <returns>A value indicating if Argument2 is valid</returns>
		protected bool CheckArgument2()
		{
			// If there is no second argumnt, then there is no argument here. See what I did there? :P
			if (Argument2.Type == ArgumentType.None) return true;

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
				if (value < 0 || value > 127)
				{
					Error = new Error();
					Error.ID = 5;
					Error.Description = Messages.E0005;
					Error.Line = Line;
					Error.Column = Argument2.Column;
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
		/// OPCode (mnemonic) and the other argument.
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
		/// OPCode (mnemonic) and the other argument.
		/// </summary>
		/// <remarks>
		/// Because there is no third argument for I/O instructions, the second
		/// argument uses additional four bits.
		/// 
		/// If the argument is a constant, it is stored in the lowest 7 bits,
		/// while the 8th has the value 0 to indicate a constant.
		/// 
		/// If 3-bit addressing is used, the 5th to 8th bits have value 1000b,
		/// while the address is stored in the 3 lowest bits. The 4th lowest
		/// bit indicates if direct (0) or indirect (1) addressing is used.
		/// </remarks>
		/// <returns>A value containing the translation of the argument</returns>
		protected ushort TranslateArgument2()
		{
			if (Argument2.Type == ArgumentType.None) return 1; // This argument is optional.

			int value;
			Argument2.LookUpValue(Assembler, out value);

			if (Argument2.Type == ArgumentType.Constant || Argument2.Type == ArgumentType.SymbolConstant)
			{
				ushort constant = (ushort)value;
				// No shifting is required, and the constant range [0..127] garanties that
				// only the first 7 low bits can be set.
				return constant;
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
				// The 3-bit address and the (in)direct addresing indicator are already in place.
				// It is only necessary to write the 1000b marker to the second argument position.
				result |= 0x8 << 4;
				return result;
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
				case IOTypes.In: return 0x7 << 12;
				case IOTypes.Out: return 0x8 << 12;
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
			if (!CheckArgument1()) return false;
			if (!CheckArgument2()) return false;

			ushort word = 0;
			// Bitwise OR operation is used to pack partial translations to a single word.
			word |= TranslateArgument1();
			word |= TranslateArgument2();
			word |= TranslateMnemonic();

			Code = new ushort[1];
			Code[0] = word;

			return true;
		}
	}

	/// <summary>
	/// A list of I/O mnemonics
	/// </summary>
	public enum IOTypes
	{
		In,
		Out
	}
}
