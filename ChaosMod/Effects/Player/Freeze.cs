using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Player
{
	internal class EffectFreeze : Effect
	{
		public override string Name => "Freeze!";
		public override string Type => "timed";
		public override float Length => 10;

		public override void Trigger()
		{
			// Freeze player.
			mainscript.M.player.mind = false;
		}

		public override void End()
		{
			// Unfreeze player.
			mainscript.M.player.mind = true;
		}
	}
}
