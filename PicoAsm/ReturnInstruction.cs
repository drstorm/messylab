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
	/// Implements the return instructions of the pC, RTS
	/// </summary>
	public class ReturnInstruction : Instruction
	{
		/// <summary>
		/// Always returns "RTS".
		/// </summary>
		public override string Mnemonic
		{
			get { return "RTS"; }
		}

		/// <summary>
		/// The size is always 1 word.
		/// </summary>
		public override ushort Size
		{
			get { return 1; }
		}

		/// <summary>
		/// Translates the instruction and sets the Code property.
		/// </summary>
		/// <returns>A value indicating if the translation was successful (always true for RTS)</returns>
		public override bool Translate()
		{
			Code = new ushort[1];
			// The instruction has no arguments so only its OPCode (1110b) is encoded.
			Code[0] = 0xE << 12;
			return true;
		}
	}
}
