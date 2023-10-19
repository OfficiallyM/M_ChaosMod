using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectEmptyFuel : Effect
	{
		public override string Name => "You didn't need fuel, did you?";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				tankscript tank = mainscript.M.player.Car.Tank;

				tank?.F.fluids.Clear();
			}
		}
	}
}
