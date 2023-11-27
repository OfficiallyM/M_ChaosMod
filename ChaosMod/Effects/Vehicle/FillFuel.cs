using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectFillFuel : Effect
	{
		public override string Name => "Free fuel!";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				tankscript tank = mainscript.M.player.Car.Tank;
				enginescript engine = mainscript.M.player.Car.Engine;

				// Return early if we can't find the fuel tank.
				if (tank == null)
					return;

				tank.F.fluids.Clear();

				float fuelMax = tank.F.maxC;

				// Find the correct fluid(s) from the engine.
				List<mainscript.fluidenum> fluids = new List<mainscript.fluidenum>();
				foreach (mainscript.fluid fluid in engine.FuelConsumption.fluids)
				{
					fluids.Add(fluid.type);
				}

				if (fluids.Count > 0)
				{
					// Two stoke.
					if (fluids.Contains(mainscript.fluidenum.oil) && fluids.Contains(mainscript.fluidenum.gas))
					{
						tank.F.ChangeOne(fuelMax / 100 * 97, mainscript.fluidenum.gas);
						tank.F.ChangeOne(fuelMax / 100 * 3, mainscript.fluidenum.oil);
					}
					else
					{
						// Just use the first fluid found by default.
						// Only mixed fuel currently is two-stroke which we're
						// accounting for already.
						tank.F.ChangeOne(fuelMax, fluids[0]);
					}
				}
			}
		}
	}
}
