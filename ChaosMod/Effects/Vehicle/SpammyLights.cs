using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectSpammyLights : Effect
	{
		public override string Name => "Spammy vehicle lights";
		public override string Type => "repeated";
		public override float Frequency => 0.2f;
		public override float Length => 30;

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				mainscript.M.player.Car.InteractSwitchLights();
			}
		}
	}
}
