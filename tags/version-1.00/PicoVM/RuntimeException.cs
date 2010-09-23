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
	/// Represents an abstruct runtime error thrown by the Processor.
	/// </summary>
	/// <remarks>
	/// On a physical machine condition resulting in throwing a RuntimeException
	/// would halt the processor.
	/// </remarks>
	public abstract class RuntimeException : Exception
	{
		/// <summary>
		/// Machine code of the current instruction
		/// </summary>
		public ushort[] Instruction { get; set; }

		/// <summary>
		/// The Address of the current instruction.
		/// </summary>
		public ushort Address { get; set; }

		public RuntimeException(ushort[] instruction, ushort address)
		{
			Instruction = instruction;
			Address = address;
		}

		/// <summary>
		/// Error ID
		/// </summary>
		public abstract int ID { get; }

		/// <summary>
		/// Formats the exception as string.
		/// </summary>
		/// <returns>A string representing the exception</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("Address: {0}, Instruction: ", Address));
			if (Instruction.Length >= 1)
				sb.Append(string.Format("{0:X4}", Instruction[0]));
			if (Instruction.Length == 2)
				sb.Append(string.Format(" {0:X4}", Instruction[1]));
			sb.Append(", ");
			sb.Append(Message);
			sb.Append(string.Format(" (E{0:0000})", ID));
			return sb.ToString();
		}
	}

	/// <summary>
	/// This exception is thrown when executing STOP instruction and represents
	/// the normal program termination.
	/// </summary>
	public class NormalTerminationRuntimeException : RuntimeException
	{
		public NormalTerminationRuntimeException(ushort[] instruction, ushort address) : base(instruction, address) { }
		
		public override int ID
		{
			get { return 0; }
		}

		public override string Message
		{
			get { return "Program exited normally."; }
		}
	}

	/// <summary>
	/// This exception is thrown when divading by zero while executing DIV instruction.
	/// </summary>
	public class DivisionByZeroRuntimeException : RuntimeException
	{
		public DivisionByZeroRuntimeException(ushort[] instruction, ushort address) : base(instruction, address) { }

		public override int ID
		{
			get { return 1; }
		}

		public override string Message
		{
			get { return "Division by zero."; }
		}
	}

	/// <summary>
	/// This exception is thrown when trying to execute a malformed instruction.
	/// </summary>
	/// <remarks>
	/// In most cases, the cause for this error is executing data (instead of code) either
	/// by an invalid branch or by failing to end the program with STOP.
	/// </remarks>
	public class InvalidInstructionRuntimeException : RuntimeException
	{
		public InvalidInstructionRuntimeException(ushort[] instruction, ushort address) : base(instruction, address) { }

		public override int ID
		{
			get { return 2; }
		}

		public override string Message
		{
			get { return "Invalid instruction."; }
		}
	}

	/// <summary>
	/// This exception is thrown when entered value is not a valid integer (or out of range).
	/// </summary>
	public class InvalidInputRuntimeException : RuntimeException
	{
		public InvalidInputRuntimeException(ushort[] instruction, ushort address) : base(instruction, address) { }

		public override int ID
		{
			get { return 3; }
		}

		public override string Message
		{
			get { return "Input is not valid."; }
		}
	}

	/// <summary>
	/// This exception is thrown when trying to pop a value from an empty stack. This occures when RTS is called before JSR.
	/// </summary>
	public class EmptyStackRuntimeException : RuntimeException
	{
		public EmptyStackRuntimeException(ushort[] instruction, ushort address) : base(instruction, address) { }

		public override int ID
		{
			get { return 4; }
		}

		public override string Message
		{
			get { return "Pop from empty stack."; }
		}
	}

	/// <summary>
	/// This exception can be thrown by a debugger component when it is unable to performe a Step Back operation due
	/// to empty History of the Data object.
	/// </summary>
	public class HistoryEmptyRuntimeException : RuntimeException
	{
		public HistoryEmptyRuntimeException(ushort[] instruction, ushort address) : base(instruction, address) { }

		public override int ID
		{
			get { return 5; }
		}

		public override string Message
		{
			get { return "Execution History empty. Cannot Step Back further."; }
		}
	}
}
