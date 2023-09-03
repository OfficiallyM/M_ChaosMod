using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectToggleIgnition : Effect
	{
		public override string Name => "Ignition";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				mainscript.M.player.Car.ignition = !mainscript.M.player.Car.ignition;
			}
		}
	}
}
