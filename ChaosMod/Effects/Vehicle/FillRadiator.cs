using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectFillRadiator : Effect
	{
		public override string Name => "Fill radiator";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				coolantTankscript coolant = mainscript.M.player.Car.coolant;

				// Return early if car has no radiator.
				if (coolant == null)
					return;

				tankscript tank = coolant.coolant;

				tank.F.fluids.Clear();
				tank.F.ChangeOne(tank.F.maxC, mainscript.fluidenum.water);
			}
		}
	}
}
