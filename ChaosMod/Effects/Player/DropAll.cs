using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static itemdatabase;

namespace ChaosMod.Effects.Player
{
	internal class EffectDropAll : Effect
	{
		public override string Name => "Drop inventory";
		public override string Type => "instant";


		public override void Trigger()
		{
			fpscontroller player = mainscript.M.player;
			for (int i = 0; i < player.inventory.Count; i++)
			{
				player.InvSwitchTo(i);
				player.RightHandRelease();
				player.Drop();
			}
		}
	}
}
