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
using MessyLab.Debugger.CommandLine;

namespace MessyLab.Debugger
{
	class Program
	{
		static void Greet()
		{
			Console.WriteLine("Messy Lab Debugger, Version 1.0");
			Console.WriteLine("Copyright (c) 2010 by Milos Andjelkovic");
			Console.WriteLine();
		}

		static void Main(string[] args)
		{
			Greet();

			// TODO: When support of another architecture is added, implement architecture
			// selection here.

			Debugger debugger = new Debugger(new MessyLab.Debugger.Target.Pico.PicoTarget());
			Console.WriteLine("Target architecture: picoComputer");

			Cli cli = new Cli(debugger);
			cli.Run();
		}
	}
}
