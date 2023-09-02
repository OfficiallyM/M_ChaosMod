using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects
{
	internal class EffectExitVehicle : Effect
	{
		public override string Name => "Exit vehicle";
		public override string Type => "instant";

		public override void Trigger()
		{
			mainscript.M.player.GetOut(mainscript.M.player.transform.position + mainscript.M.player.transform.up * 2f, true);
		}
	}
}
