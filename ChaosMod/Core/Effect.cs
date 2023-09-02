using ChaosMod.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Core
{
	public abstract class Effect
	{
		public virtual string Name { get; }

		/// <summary>
		/// Can be instant, timed or repeated.
		/// </summary>
		public virtual string Type { get; }

		/// <summary>
		/// Only required if Type is timed.
		/// </summary>
		public virtual float Length { get { return 0; } }

		/// <summary>
		/// Only required if Type is repeated.
		/// </summary>
		public virtual float Frequency { get { return 0; } }

		/// <summary>
		/// Called once if Type is instant or timed or at the Frequency interval for repeated.
		/// </summary>
		public virtual void Trigger() { }

		/// <summary>
		/// Only called if Type is timed or repeated. Called after the length elapses.
		/// </summary>
		public virtual void End() { }
	}
}
