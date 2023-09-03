using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectLights : Effect
	{
		public override string Name => "Vehicle lights";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				mainscript.M.player.Car.InteractSwitchLights();
			}
		}
	}
}
