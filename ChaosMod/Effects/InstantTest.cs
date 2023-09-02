using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects
{
	internal class EffectInstantTest : Effect
	{
		public override string Name => "Instant test";
		public override string Type => "instant";

		public override void Trigger()
		{
			mainscript.M.player.AJump();
		}
	}
}
