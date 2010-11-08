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

namespace MessyLab.Debugger
{
	/// <summary>
	/// A collection of breakpoints with the ability to determine the hit breakpoints.
	/// </summary>
	public class BreakpointManager : IEnumerable<Breakpoint>, ICollection<Breakpoint>, IList<Breakpoint>
	{
		/// <summary>
		/// Creates the object using the specified Debugger
		/// </summary>
		/// <param name="debugger">A Debugger reference used to access the Breakpoint Checkers of the Target.</param>
		public BreakpointManager(Debugger debugger)
		{
			Debugger = debugger;
			Breakpoints = new List<Breakpoint>();
			HitBreakpoints = new List<Breakpoint>();
		}

		/// <summary>
		/// A Debugger reference used to access the Breakpoint Checkers of the Target.
		/// </summary>
		public Debugger Debugger { get; set; }

		/// <summary>
		/// A list containing the breakpoints.
		/// </summary>
		protected List<Breakpoint> Breakpoints { get; set; }

		/// <summary>
		/// A list of currently hit breakpoints.
		/// </summary>
		/// <remarks>
		/// The list is updated by the <c>Process</c> method.
		/// </remarks>
		public List<Breakpoint> HitBreakpoints { get; protected set; }

		/// <summary>
		/// Checks all breakpoints and stores the hit ones in <c>HitBreakpoints</c>
		/// and removes the hit one-shot breakpoints.
		/// </summary>
		public void Process()
		{
			lock (this)
			{
				HitBreakpoints.Clear();
				for (int i = Breakpoints.Count - 1; i >= 0; i--)
				{
					Breakpoint bp = Breakpoints[i];
					if (bp.Check())
					{
						if (bp.IsOneShot)
							Breakpoints.RemoveAt(i);
						HitBreakpoints.Add(bp);
					}
				}
			}
		}

		/// <summary>
		/// Removes all Step breakpoints.
		/// </summary>
		/// <remarks>
		/// This is typically used when the execution is stopped.
		/// </remarks>
		public void RemoveStepBreakpoints()
		{
			lock (this)
			{
				for (int i = Breakpoints.Count - 1; i >= 0; i--)
				{
					if (Breakpoints[i] is StepBreakpoint)
					{
						Breakpoints.RemoveAt(i);
					}
				}
			}
		}

		#region ICollection<Breakpoint> Members

		/// <summary>
		/// Adds a breakpoint to the collection and sets its Checker.
		/// </summary>
		/// <param name="breakpoint">The breakpoint to add. Cannot be null.</param>
		public void Add(Breakpoint breakpoint)
		{
			lock (this)
			{
				if (breakpoint == null) throw new ArgumentNullException();
				Debugger.Target.SetBreakpointChecker(breakpoint);
				Breakpoints.Add(breakpoint);
			}
		}

		public bool Remove(Breakpoint breakpoint) { lock(this) return Breakpoints.Remove(breakpoint); }
		public void Clear() { lock (this) Breakpoints.Clear(); }
		public bool Contains(Breakpoint item) { lock (this) return Breakpoints.Contains(item); }
		public void CopyTo(Breakpoint[] array, int arrayIndex) { lock (this) Breakpoints.CopyTo(array, arrayIndex); }
		public int Count { get { lock (this) return Breakpoints.Count; } }
		public bool IsReadOnly { get { return false; } }

		#endregion

		#region IEnumerable<Breakpoint> Members

		public IEnumerator<Breakpoint> GetEnumerator()
		{
			lock (this)
			{
				for (int i = 0; i < Breakpoints.Count; i++)
				{
					yield return Breakpoints[i];
				}
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			lock (this)
			{
				for (int i = 0; i < Breakpoints.Count; i++)
				{
					yield return Breakpoints[i];
				}
			}
		}

		#endregion

		#region IList<Breakpoint> Members

		public int IndexOf(Breakpoint item) { return Breakpoints.IndexOf(item); }

		/// <summary>
		/// Insers a new breakpoint at a specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the breakpoint should be inserted.</param>
		/// <param name="item">The breakpoint to insert. Cannot be null.</param>
		public void Insert(int index, Breakpoint item)
		{
			lock (this)
			{
				if (item == null) throw new ArgumentNullException();
				Debugger.Target.SetBreakpointChecker(item);
				Breakpoints.Insert(index, item);
			}
		}

		public void RemoveAt(int index) { lock (this) Breakpoints.RemoveAt(index); }

		/// <summary>
		/// Gets of sets the breakpoint at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the breakpoint to get or set.</param>
		/// <returns>The breakpoint at the specified index.</returns>
		public Breakpoint this[int index]
		{
			get { lock (this) return Breakpoints[index]; }
			set
			{
				lock (this)
				{
					if (value == null) throw new ArgumentNullException();
					Debugger.Target.SetBreakpointChecker(value);
					Breakpoints[index] = value;
				}
			}
		}

		#endregion
	}
}
