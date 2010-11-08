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
	/// Implements the call instruction of the pC, JSR
	/// </summary>
	public class CallInstruction : Instruction
	{
		/// <summary>
		/// Always returns "JSR".
		/// </summary>
		public override string Mnemonic
		{
			get { return "JSR"; }
		}

		/// <summary>
		/// The size is always 2 words.
		/// </summary>
		public override ushort Size
		{
			get { return 2; }
		}

		/// <summary>
		/// Checks if the argument is valid.
		/// </summary>
		/// <remarks>
		/// The argument must be a valid 16-bit address [0..65535]
		/// </remarks>
		/// <returns>A value indicating if Argument1 is valid</returns>
		protected bool CheckArgument1()
		{
			int value;
			
			if (!Argument1.LookUpValue(Assembler, out value))
				return false;

			if (value < ushort.MinValue || value > ushort.MaxValue)
			{
				Error = new Error();
				Error.ID = 3;
				Error.Description = string.Format(Messages.E0003, Argument1.Text, ushort.MinValue, ushort.MaxValue);
				Error.Line = Line;
				Error.Column = Argument1.Column;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Translates the instruction and sets the Code property.
		/// If an error is detected, Error property is set instead.
		/// </summary>
		/// <returns>A value indicating if the translation was successful</returns>
		public override bool Translate()
		{
			Error = null;
			if (!CheckArgument1()) return false;
			Code = new ushort[2];
			Code[0] = 0xD << 12; // The first word is always the same (only an OPCode).
			int value;
			Argument1.LookUpValue(Assembler, out value);
			Code[1] = (ushort)value; // The second word is the address of the subroutine.
			return true;
		}
	}
}
