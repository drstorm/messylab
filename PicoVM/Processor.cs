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
using System.Text;

namespace MessyLab.PicoComputer
{
	/// <summary>
	/// A picoComputer processor implementation.
	/// </summary>
	public class Processor
	{
		/// <summary>
		/// Data that represents the state of the machine
		/// (both the memory and the processor registers).
		/// </summary>
		public Data Data { get; set; }
		/// <summary>
		/// Input/Output device for IN and OUT instructiuons.
		/// </summary>
		public IIODevice IODevice { get; set; }

		/// <summary>
		/// Creates a new Processor object.
		/// </summary>
		/// <param name="data">A Data object that represents the machine state.</param>
		/// <param name="ioDevice">An IIODevice object that represents the IODevice.</param>
		public Processor(Data data, IIODevice ioDevice)
		{
			Data = data;
			IODevice = ioDevice;
		}

		private ushort _ir;
		/// <summary>
		/// IR (instruction register) stores the value read from memory
		/// by the Fatch() method.
		/// </summary>
		protected ushort IR { get { return _ir; } set { _ir = value; } }

		/// <summary>
		/// Reads the value at address from PC, stores it in IR and increments the PC.
		/// </summary>
		protected void Fatch()
		{
			IR = Data[Data.PC++];
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
		/// Loads the instruction from IR and stores it in the Instruction property.
		/// </summary>
		protected void LoadInstruction()
		{
			byte[] ins = new byte[4];
			ins[0] = (byte)(IR >> 12);
			ins[1] = (byte)((IR >> 8) & 0xF);
			ins[2] = (byte)((IR >> 4) & 0xF);
			ins[3] = (byte)(IR & 0xF);
			Instruction = ins;
		}

		/// <summary>
		/// Executes a single instruction.
		/// </summary>
		/// <remarks>
		/// Fatches the instruction, decodes its OP Code and calls
		/// the appropriate method to execute it.
		/// </remarks>
		public void Step()
		{
			Fatch();
			LoadInstruction();
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
		}

		/// <summary>
		/// Gets address from an instruction argument and the addressing mode indicator.
		/// </summary>
		/// <param name="argument">The argument to decode</param>
		/// <param name="indirect">A value indicating if the addressing is indirect.</param>
		/// <returns>The address of the memory location pointed to by the argument</returns>
		protected ushort GetAddress(byte argument, bool indirect)
		{
			if (indirect)
			{
				return Data[argument];
			}
			else
				return argument;
		}

		/// <summary>
		/// Gets address from an instruction argument.
		/// </summary>
		/// <remarks>
		/// If the highest bit is 1, addressing mode is indirect.
		/// The lower three bits are passed to the overloaded method
		/// as the argument value along with the addressing mode
		/// indicator.
		/// </remarks>
		/// <param name="argument">The argument to decode</param>
		/// <returns>The address of the memory location pointed to by the argument</returns>
		protected ushort GetAddress(byte argument)
		{
			return GetAddress((byte)(argument & 0x7), (argument & 0x8) == 0 ? false : true);
		}

		/// <summary>
		/// Implements the MOV instruction.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// </remarks>
		protected void Mov()
		{
			ushort d = 0;
			ushort s = 0;
			ushort n = 0;
			// The third operand is used as an extension of the OP Code.
			switch (Instruction[3])
			{
				case 0x0: // MOV x, y
					Data[GetAddress(Instruction[1])] = Data[GetAddress(Instruction[2])];
					return;
				case 0x8: // MOV x, 1
					Fatch();
					Data[GetAddress(Instruction[1])] = IR;
					return;
				case 0xF: // MOV x, y, 5
					d = GetAddress(Instruction[1]);
					s = GetAddress(Instruction[2]);
					Fatch(); // Fatch the constant.
					n = IR;
					break;
				default: // MOV x, y, n
					if ((Instruction[3] & 0x8) != 0)
					{
						throw new InvalidInstructionRuntimeException(new ushort[] { IR }, (ushort)(Data.PC - 1));
					}
					d = GetAddress(Instruction[1]);
					s = GetAddress(Instruction[2]);
					n = Data[GetAddress(Instruction[3], false)];
					break;
			}
			for (int i = 0; i < n; i++)
			{
				Data[d++] = Data[s++];
			}
		}

		/// <summary>
		/// Gets the operands of an arithmetic instruction (ADD, SUB, MUL, DIV).
		/// </summary>
		/// <param name="constant">A value indicating if the instruction contains a constant</param>
		/// <param name="destinationAddress">The address of the memory location to store the result.</param>
		/// <param name="value1">The value of the first operand.</param>
		/// <param name="value2">The value of the second operand.</param>
		protected void GetArithmeticOperands(bool constant, out ushort destinationAddress, out short value1, out short value2)
		{
			destinationAddress = GetAddress(Instruction[1]);

			ushort adr1 = GetAddress(Instruction[2]);
			ushort adr2 = GetAddress(Instruction[3]);
			
			if (constant)
			{
				ushort instruction = IR;

				Fatch();
				short c = (short)IR;

				if ((Instruction[2] == 0 && Instruction[3] == 0) || (Instruction[2] != 0 && Instruction[3] != 0))
				{
					throw new InvalidInstructionRuntimeException(new ushort[] { instruction, IR }, (ushort)(Data.PC - 2));
				}

				value1 = Instruction[2] != 0 ? (short)Data[adr1] : c;
				value2 = Instruction[3] != 0 ? (short)Data[adr2] : c;
			}
			else
			{
				value1 = (short)Data[adr1];
				value2 = (short)Data[adr2];
			}
		}

		/// <summary>
		/// Implements the ADD instruction.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// </remarks>
		/// <param name="constant"></param>
		protected void Add(bool constant)
		{
			ushort d;
			short v1;
			short v2;
			GetArithmeticOperands(constant, out d, out v1, out v2);
			Data[d] = (ushort)(v1 + v2);
		}

		/// <summary>
		/// Implements the SUB instruction.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// </remarks>
		protected void Sub(bool constant)
		{
			ushort d;
			short v1;
			short v2;
			GetArithmeticOperands(constant, out d, out v1, out v2);
			Data[d] = (ushort)(v1 - v2);
		}

		/// <summary>
		/// Implements the MUL instruction.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// </remarks>
		protected void Mul(bool constant)
		{
			ushort d;
			short v1;
			short v2;
			GetArithmeticOperands(constant, out d, out v1, out v2);
			Data[d] = (ushort)(v1 * v2);
		}

		/// <summary>
		/// Implements the DIV instruction.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// Throws an exception when deviding by zero.
		/// </remarks>
		protected void Div(bool constant)
		{
			ushort d;
			short v1;
			short v2;
			GetArithmeticOperands(constant, out d, out v1, out v2);
			try
			{
				Data[d] = (ushort)(v1 / v2);
			}
			catch (DivideByZeroException)
			{
				ushort[] instruction;
				ushort address;
				if (constant)
				{
					instruction = new ushort[] { Data[(ushort)(Data.PC - 2)], IR };
					address = (ushort)(Data.PC - 2);
				}
				else
				{
					instruction = new ushort[] { IR };
					address = (ushort)(Data.PC - 1);
				}
				throw new DivisionByZeroRuntimeException(instruction, address);
			}
		}

		/// <summary>
		/// Implements the BEQ and BGT instructions.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// </remarks>
		/// <param name="isBeq">If true executes BEQ, otherwise BGT.</param>
		protected void Branch(bool isBeq)
		{
			short v1 = (short)(Instruction[1] == 0 ? 0 : Data[GetAddress(Instruction[1])]);
			short v2 = (short)(Instruction[2] == 0 ? 0 : Data[GetAddress(Instruction[2])]);
			ushort branchAddress;
			if (Instruction[3] > 7)
			{
				if (Instruction[3] != 0x8)
				{
					throw new InvalidInstructionRuntimeException(new ushort[] { IR }, (ushort)(Data.PC - 1));
				}
				Fatch();
				branchAddress = IR;
			}
			else
			{
				branchAddress = GetAddress(Instruction[3], true);
			}
			if (isBeq)
			{
				if (v1 == v2) Data.PC = branchAddress;
			}
			else // Bgt
			{
				if (v1 > v2) Data.PC = branchAddress;
			}
		}

		/// <summary>
		/// Implements the IN and OUT instructions.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// </remarks>
		/// <param name="isIn">If true executes IN, otherwise OUT.</param>
		protected void InOut(bool isIn)
		{
			ushort address = GetAddress(Instruction[1]);
			ushort count = 1;
			if ((Instruction[2] & 0x8) == 0)
			{
				count = (ushort)((Instruction[2] << 4) | Instruction[3]);
			}
			else
			{
				if (Instruction[2] != 0x8)
				{
					throw new InvalidInstructionRuntimeException(new ushort[] { IR }, (ushort)(Data.PC - 1));
				}
				count = Data[GetAddress(Instruction[3])];
			}
			if (isIn)
			{
				try
				{
					IODevice.Read(address, count, Data);
				}
				catch (Exception)
				{
					throw new InvalidInputRuntimeException(new ushort[] { IR }, (ushort)(Data.PC - 1));
				}
			}
			else // Out
			{
				IODevice.Write(address, count, Data);
			}
		}

		/// <summary>
		/// Implements the JSR instruction.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// </remarks>
		protected void Jsr()
		{
			Fatch();
			ushort address = IR;
			Data[Data.SP--] = Data.PC;
			Data.PC = address;
		}

		/// <summary>
		/// Implements the RTS instruction.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// Throws an exception if the stack is empty.
		/// </remarks>
		protected void Rts()
		{
			Data.SP++;
#pragma warning disable 0652
			if (Data.SP == Data.Size || Data.SP == 0)
#pragma warning restore 0652
			{
				throw new EmptyStackRuntimeException(new ushort[] { IR }, (ushort)(Data.PC - 1));
			}
			ushort address = Data[Data.SP];
			Data.PC = address;
		}

		/// <summary>
		/// Implements the STOP instruction.
		/// </summary>
		/// <remarks>
		/// For further details refer to the picoComputer documentation.
		/// Throws a NormalTerminationRuntimeException to terminate the program.
		/// </remarks>
		protected void Stop()
		{
			for (int i = 1; i <= 3; i++)
			{
				if (Instruction[i] != 0)
				{
					IODevice.Write(GetAddress(Instruction[i]), 1, Data);
				}
			}
			throw new NormalTerminationRuntimeException(new ushort[] { IR }, (ushort)(Data.PC - 1));
		}
	}
}
