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
		/// Can be instant, timed, repeated or fixedRepeated.
		/// </summary>
		public virtual string Type { get; }

		/// <summary>
		/// Only required if Type is timed.
		/// </summary>
		public virtual float Length { get { return 0; } }

		/// <summary>
		/// Only required if Type is repeated.
		/// </summary>
		public virtual float Frequency { get { return -1; } }

		/// <summary>
		/// Whether OnGUI should be called.
		/// </summary>
		public virtual bool UseGUI { get { return false; } }

		/// <summary>
		/// Whether the effect needs to be manually enabled.
		/// </summary>
		public virtual bool DisabledByDefault { get { return false; } }

		/// <summary>
		/// Called once if Type is instant or timed or at the Frequency interval for repeated.
		/// </summary>
		public virtual void Trigger() { }

		/// <summary>
		/// Only called if Type is timed or repeated. Called after the length elapses.
		/// </summary>
		public virtual void End() { }

		/// <summary>
		/// Unity OnGUI method. Only called if UseGUI is true.
		/// </summary>
		public virtual void OnGUI() { }

		/// <summary>
		/// Subscribe to ModEvent callbacks.
		/// </summary>
		/// <param name="sender">ModEvent sender</param>
		/// <param name="eventType">ModEvent eventType</param>
		public virtual void ModEventCallback(object sender, string eventType) { }
	}

	public class EffectHistory
	{
		public Effect Effect { get; set; }
		public ActiveEffect ActiveEffect { get; set; }
	}
}
