using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectSpammyHorn : Effect
	{
		public override string Name => "Spammy horn";
		public override string Type => "repeated";
		public override float Length => 30f;
		public override float Frequency => 1f;

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				mainscript.M.player.Car.HornHit(0.9f, 1.2f);
			}
		}
	}
}
