using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Player
{
	internal class EffectDropHand : Effect
	{
		public override string Name => "Butter fingers";
		public override string Type => "instant";


		public override void Trigger()
		{
			fpscontroller player = mainscript.M.player;
			player.RightHandRelease();
			player.Drop();
		}
	}
}
