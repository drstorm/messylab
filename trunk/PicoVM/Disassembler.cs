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
	/// Disassembler for picoComputer used to
	/// disassemble the current instruction.
	/// </summary>
	public class Disassembler
	{
		/// <summary>
		/// Data that represents the state of the machine
		/// (both the memory and the processor registers).
		/// </summary>
		public Data Data { get; set; }

		/// <summary>
		/// Creates a new Processor object.
		/// </summary>
		/// <param name="data">A Data object that represents the machine state.</param>
		public Disassembler(Data data)
		{
			Data = data;
		}

		/// <summary>
		/// The decoded instruction
		/// </summary>
		/// <remarks>
		/// i[0] - OP Code
		/// i[1..3] - Arguments
		/// </remarks>
		protected byte[] Instruction { get; set; }
		
		/// <summary>
		/// The First Word of the loaded instruction.
		/// </summary>
		protected ushort FirstWord { get; set; }
		/// <summary>
		/// The Second Word of the loaded instruction.
		/// </summary>
		protected ushort SecondWord { get; set; }

		/// <summary>
		/// Loads the instruction from Data object and by setting Instruction,
		/// FirstWord and SecondWord properties.
		/// </summary>
		protected void LoadInstruction()
		{
			FirstWord = Data.DirectMemoryRead(Data.DirectPC);
			SecondWord = Data.DirectMemoryRead((ushort)(Data.DirectPC + 1));

			byte[] ins = new byte[4];
			ins[0] = (byte)(FirstWord >> 12);
			ins[1] = (byte)((FirstWord >> 8) & 0xF);
			ins[2] = (byte)((FirstWord >> 4) & 0xF);
			ins[3] = (byte)(FirstWord & 0xF);
			Instruction = ins;
		}

		/// <summary>
		/// Represents the Word Count of the current instruction.
		/// </summary>
		protected int WordCount { get; set; }

		/// <summary>
		/// Represents the Symbolic representation of the current instruction.
		/// </summary>
		protected string DisassembledInstruction { get; set; }

		/// <summary>
		/// Creates the binary representation of the current instruction.
		/// </summary>
		/// <remarks>
		/// If the instruction has one word the result is formated as
		/// following: "xxxx xxxx xxxx xxxx";
		/// otherwise: "xxxx xxxx xxxx xxxx  xxxxxxxxxxxxxxxx".
		/// </remarks>
		/// <returns>Binary representation of the current instruction</returns>
		protected string ToBinary()
		{
			string result = null;
			
			for (int i = 0; i < 4; i++)
			{
				string s = Convert.ToString(Instruction[i], 2);
				while (s.Length < 4) s = "0" + s;
				
				if (string.IsNullOrEmpty(result))
				{ result = s; }
				else
				{ result += " " + s; }
			}

			if (WordCount == 2)
			{
				string s = Convert.ToString(SecondWord, 2);
				while (s.Length < 16) s = "0" + s;
				result += "  " + s;
			}
			
			return result;
		}

		/// <summary>
		/// Disassembles the current instruction.
		/// </summary>
		/// <param name="disassembledInstruction">Symbolic representation of the disassembled instruction.</param>
		/// <param name="binaryRepresentation">Binary representation of the disassembled instruction.</param>
		public void Disassemble(out string disassembledInstruction, out string binaryRepresentation)
		{
			LoadInstruction();
			WordCount = 1;

			byte opcode = Instruction[0];
			switch (opcode)
			{
				case 0: Mov(); break;
				case 1: Add(false); break;
				case 2: Sub(false); break;
				case 3: Mul(false); break;
				case 4: Div(false); break;
				case 5: Branch(true); break; // Beq
				case 6: Branch(false); break; // Bgt
				case 7: InOut(true); break; // In
				case 8: InOut(false); break; //Out
				case 9: Add(true); break;
				case 10: Sub(true); break;
				case 11: Mul(true); break;
				case 12: Div(true); break;
				case 13: Jsr(); break;
				case 14: Rts(); break;
				case 15: Stop(); break;
				default: break;
			}

			disassembledInstruction = DisassembledInstruction;
			binaryRepresentation = ToBinary();
		}

		/// <summary>
		/// Formats the address using the instruction argument and the addressing mode indicator.
		/// </summary>
		/// <param name="argument">The argument to format.</param>
		/// <param name="indirect">A value indicating if the addressing is indirect.</param>
		/// <returns>The formated address of the memory location pointed to by the argument.</returns>
		protected string FormatAddress(byte argument, bool indirect)
		{
			if (indirect)
			{
				return "(" + argument + ")";
			}
			else
				return argument.ToString();
		}

		/// <summary>
		/// Formats the address.
		/// </summary>
		/// <remarks>
		/// If the highest bit is 1, addressing mode is indirect.
		/// The lower three bits are passed to the overloaded method
		/// as the argument value along with the addressing mode
		/// indicator.
		/// </remarks>
		/// <param name="argument">The argument to format.</param>
		/// <returns>The formated address of the memory location pointed to by the argument.</returns>
		protected string FormatAddress(byte argument)
		{
			return FormatAddress((byte)(argument & 0x7), (argument & 0x8) == 0 ? false : true);
		}

		/// <summary>
		/// Describes MOV instruction.
		/// </summary>
		protected void Mov()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("MOV ");

			// The third operand is used as an extension of the OP Code.
			switch (Instruction[3])
			{
				case 0x0: // MOV x, y
					WordCount = 1;
					sb.Append(FormatAddress(Instruction[1]));
					sb.Append(", ");
					sb.Append(FormatAddress(Instruction[2]));
					break;
				case 0x8: // MOV x, 1
					WordCount = 2;
					sb.Append(FormatAddress(Instruction[1]));
					sb.Append(", #");
					sb.Append(SecondWord);
					break;
				case 0xF: // MOV x, y, 5
					WordCount = 2;
					sb.Append(FormatAddress(Instruction[1]));
					sb.Append(", ");
					sb.Append(FormatAddress(Instruction[2]));
					sb.Append(", #");
					sb.Append(SecondWord);
					break;
				default: // MOV x, y, n
					if ((Instruction[3] & 0x8) != 0)
					{
						sb.Append("?");
					}
					else
					{
						WordCount = 1;
						sb.Append(FormatAddress(Instruction[1]));
						sb.Append(", ");
						sb.Append(FormatAddress(Instruction[2]));
						sb.Append(", ");
						sb.Append(FormatAddress(Instruction[3]));
					}
					break;
			}
			DisassembledInstruction = sb.ToString();
		}

		/// <summary>
		/// Formats operands of an arithmetic instruction.
		/// </summary>
		/// <param name="constant">A value indicating if the instruction contains a constant</param>
		/// <returns>The string representation of the operands.</returns>
		protected string FormatArithmeticOperands(bool constant)
		{
			WordCount = constant ? 2 : 1;

			StringBuilder sb = new StringBuilder();
			sb.Append(FormatAddress(Instruction[1]));
			sb.Append(", ");

			string adr1 = FormatAddress(Instruction[2]);
			string adr2 = FormatAddress(Instruction[3]);

			if (constant)
			{
				if ((Instruction[2] == 0 && Instruction[3] == 0) || (Instruction[2] != 0 && Instruction[3] != 0))
				{
					adr1 = "?";
					adr2 = "?";
				}
				else
				{
					adr1 = Instruction[2] != 0 ? adr1 : "#" + SecondWord;
					adr2 = Instruction[3] != 0 ? adr2 : "#" + SecondWord;
				}
			}
			sb.Append(adr1);
			sb.Append(", ");
			sb.Append(adr2);

			return sb.ToString();
		}

		/// <summary>
		/// Describes MOV instruction.
		/// </summary>
		protected void Add(bool constant)
		{
			DisassembledInstruction = "ADD " + FormatArithmeticOperands(constant);
		}

		/// <summary>
		/// Describes SUB instruction.
		/// </summary>
		protected void Sub(bool constant)
		{
			DisassembledInstruction = "SUB " + FormatArithmeticOperands(constant);
		}

		/// <summary>
		/// Describes MUL instruction.
		/// </summary>
		protected void Mul(bool constant)
		{
			DisassembledInstruction = "MUL " + FormatArithmeticOperands(constant);
		}

		/// <summary>
		/// Describes DIV instruction.
		/// </summary>
		protected void Div(bool constant)
		{
			DisassembledInstruction = "DIV " + FormatArithmeticOperands(constant);
		}

		/// <summary>
		/// Describes branck instructions (BEQ and BGT).
		/// </summary>
		/// <param name="isBeq">If true describes BEQ, otherwise BGT.</param>
		protected void Branch(bool isBeq)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(isBeq ? "BEQ " : "BGT ");

			string v1 = Instruction[1] == 0 ? "#0" : FormatAddress(Instruction[1]);
			sb.Append(v1);
			sb.Append(", ");
			string v2 = Instruction[2] == 0 ? "#0" : FormatAddress(Instruction[2]);
			sb.Append(v2);
			sb.Append(", ");

			string branchAddress;

			WordCount = 1;
			if (Instruction[3] > 7)
			{
				if (Instruction[3] != 0x8)
				{
					branchAddress = "?";
				}
				else
				{
					WordCount = 2;
					branchAddress = SecondWord.ToString();
				}
			}
			else
			{
				branchAddress = FormatAddress(Instruction[3], true);
			}

			sb.Append(branchAddress);

			DisassembledInstruction = sb.ToString();
		}

		/// <summary>
		/// Describes IN and OUT instructions.
		/// </summary>
		/// <param name="isIn">If true describes IN, otherwise OUT.</param>
		protected void InOut(bool isIn)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(isIn ? "IN " : "OUT ");

			sb.Append(FormatAddress(Instruction[1]));
			sb.Append(", ");

			string count;
			if ((Instruction[2] & 0x8) == 0)
			{
				count = "#" + (ushort)((Instruction[2] << 4) | Instruction[3]);
			}
			else
			{
				if (Instruction[2] != 0x8)
				{
					count = "?";
				}
				count = FormatAddress(Instruction[3]);
			}
			sb.Append(count);
			WordCount = 1;
			DisassembledInstruction = sb.ToString();
		}

		/// <summary>
		/// Describes JSR instruction.
		/// </summary>
		protected void Jsr()
		{
			WordCount = 2;
			DisassembledInstruction = "JSR " + SecondWord;
		}

		/// <summary>
		/// Describes RTS instruction.
		/// </summary>
		protected void Rts()
		{
			WordCount = 1;
			DisassembledInstruction = "RTS";
		}

		/// <summary>
		/// Describes STOP instruction.
		/// </summary>
		protected void Stop()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("STOP");

			List<string> args = new List<string>();
			for (int i = 1; i <= 3; i++)
			{
				if (Instruction[i] != 0)
				{
					args.Add(FormatAddress(Instruction[i]));
				}
			}

			for (int i = 0; i < args.Count; i++)
			{
				if (i == 0) sb.Append(" ");
				else sb.Append(", ");
				sb.Append(args[i]);
			}

			WordCount = 1;
			DisassembledInstruction = sb.ToString();
		}
	}
}
