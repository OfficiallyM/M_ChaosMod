using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectEmptyFuel : Effect
	{
		public override string Name => "You didn't need fuel, did you?";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.lastCar != null)
			{
				carscript car = mainscript.M.player.lastCar;
				tankscript tank = car.gameObject.GetComponentInChildren<tankscript>();
				if (tank != null)
				{
					tank.F.fluids.Clear();
				}
			}
		}
	}
}
