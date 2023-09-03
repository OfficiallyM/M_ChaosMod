using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectExitVehicle : Effect
	{
		public override string Name => "Exit vehicle";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				mainscript.M.player.SwitchToCockpitView(true);
				mainscript.M.player.GetOut(mainscript.M.player.transform.position + mainscript.M.player.transform.up * 2f, true);
			}
		}
	}
}
