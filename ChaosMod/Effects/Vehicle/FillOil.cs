using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectFillOil : Effect
	{
		public override string Name => "Fill engine oil";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				enginescript engine = mainscript.M.player.Car.Engine;

				// Return early if car has no engine.
				if (engine == null)
					return;

				tankscript tank = engine.T;

				tank.F.fluids.Clear();
				tank.F.ChangeOne(tank.F.maxC, mainscript.fluidenum.oil);
			}
		}
	}
}
