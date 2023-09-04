using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Player
{
	internal class EffectWobblyHead : Effect
	{
		public override string Name => "Wobbly head";
		public override string Type => "timed";
		public override float Length => 30;

		private float defaultBob = 0;

		public override void Trigger()
		{
			defaultBob = mainscript.M.player.maxWalkBob;
			mainscript.M.player.maxWalkBob = 0.75f;
		}

		public override void End()
		{
			mainscript.M.player.maxWalkBob = defaultBob;
		}
	}
}
