using ChaosMod.Core;
using ChaosMod.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDLoader;

namespace ChaosMod.Effects.Player
{
	internal class EffectHeal : Effect
	{
		public override string Name => "Full heal";
		public override string Type => "instant";

		public override void Trigger()
		{
			Logger.Log(mainscript.M.player.survival.maxHp.ToString(), Logger.LogLevel.Debug);
			mainscript.M.HpCh(100);
		}
	}
}
