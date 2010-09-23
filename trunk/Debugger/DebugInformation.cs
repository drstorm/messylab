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
using System.Xml.Serialization;
using System.IO;

namespace MessyLab.Debugger
{
	/// <summary>
	/// Holds the Debug Information and provides file saving and loading
	/// functionalities.
	/// </summary>
	[Serializable]
	public class DebugInformation
	{
		private Dictionary<DebugSymbolLocation, DebugSymbol> _byLocation = new Dictionary<DebugSymbolLocation, DebugSymbol>();

		/// <summary>
		/// Allows access to the information by location.
		/// </summary>
		[XmlIgnore]
		public Dictionary<DebugSymbolLocation, DebugSymbol> ByLocation { get { return _byLocation; } }

		private Dictionary<string, DebugSymbol> _byName = new Dictionary<string, DebugSymbol>();

		/// <summary>
		/// Allows access to the information by Name.
		/// </summary>
		[XmlIgnore]
		public Dictionary<string, DebugSymbol> ByName { get { return _byName; } }

		private Dictionary<long, DebugSymbol> _byValue = new Dictionary<long, DebugSymbol>();

		/// <summary>
		/// Allows access to the information by value.
		/// </summary>
		[XmlIgnore]
		public Dictionary<long, DebugSymbol> ByValue { get { return _byValue; } }

		private List<DebugSymbol> _symbols = new List<DebugSymbol>();

		/// <summary>
		/// List holding the Debug Symbols.
		/// </summary>
		public List<DebugSymbol> Symbols { get { return _symbols; } }

		/// <summary>
		/// Adds the specified symbol to the collections.
		/// </summary>
		/// <param name="symbol">The symbol to add.</param>
		public void AddSymbol(DebugSymbol symbol)
		{
			Symbols.Add(symbol);
			ByLocation[symbol.Location] = symbol;
			ByName[symbol.Name] = symbol;
			ByValue[symbol.Value] = symbol;
		}

		/// <summary>
		/// Creates a new DebugSymbol object by using the specified data and
		/// adds it to the collections.
		/// </summary>
		/// <param name="file">File where the symbol is defined.</param>
		/// <param name="line">Line within the file where the symbol is defined.</param>
		/// <param name="name">Name of the Symbol.</param>
		/// <param name="value">Value of the Symbol.</param>
		/// <param name="isExecutable">A value indicating whether the symbol represents an executable line (i.e. a Label)</param>
		public void AddSymbol(string file, int line, string name, long value, bool isExecutable)
		{
			DebugSymbol s = new DebugSymbol();
			s.Location.File = file;
			s.Location.Line = line;
			s.Name = name;
			s.Value = value;
			s.IsExecutable = isExecutable;
			AddSymbol(s);
		}

		/// <summary>
		/// Clears the symbol collections.
		/// </summary>
		public void Clear()
		{
			ByLocation.Clear();
			ByName.Clear();
			ByValue.Clear();
			Symbols.Clear();
		}

		/// <summary>
		/// Saves the Debug information to file.
		/// </summary>
		/// <param name="path">Full path to the file to save.</param>
		public void SaveToFile(string path)
		{
			XmlSerializer s = new XmlSerializer(GetType());
			TextWriter w = new StreamWriter(path);
			s.Serialize(w, this);
			w.Close();
		}

		/// <summary>
		/// Loads the Debug information from file.
		/// </summary>
		/// <param name="path">Full path to the file to load.</param>
		public void LoadFromFile(string path)
		{
			XmlSerializer s = new XmlSerializer(GetType());
			TextReader r = new StreamReader(path);
			DebugInformation inf =  s.Deserialize(r) as DebugInformation;
			r.Close();
			Clear();
			foreach (DebugSymbol symbol in inf.Symbols)
			{
				AddSymbol(symbol);
			}
		}
	}

	/// <summary>
	/// A structure holding the symbol location.
	/// </summary>
	[Serializable]
	public struct DebugSymbolLocation
	{
		/// <summary>
		/// Name of the file where the symbol was defined.
		/// </summary>
		public string File;
		
		/// <summary>
		/// Line within the file where the symbol was defined.
		/// </summary>
		public int Line;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(File);
			sb.Append(':');
			sb.Append(Line);
			return sb.ToString();
		}
	}

	/// <summary>
	/// Represents a Debug Symbol
	/// </summary>
	[Serializable]
	public class DebugSymbol
	{
		/// <summary>
		/// Definition Location of the symbol.
		/// </summary>
		public DebugSymbolLocation Location;

		public string Name;
		public long Value;

		/// <summary>
		/// A value indicating whether the symbol represents an executable line (i.e. is a label).
		/// </summary>
		public bool IsExecutable;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Location.ToString());
			sb.Append(' ');
			sb.Append(Name);
			sb.Append('=');
			sb.Append(Value);
			return sb.ToString();
		}
	}

	/// <summary>
	/// An Exception to be thrown when the symbol look-up fails.
	/// </summary>
	public class DebugSymbolNotFoundException : Exception
	{
		public override string Message
		{
			get { return "Could not find Debug Symbol that matches the specified parameters."; }
		}
	}
}
