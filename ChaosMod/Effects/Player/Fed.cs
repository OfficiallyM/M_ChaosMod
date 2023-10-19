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
	internal class EffectFed : Effect
	{
		public override string Name => "Fed";
		public override string Type => "instant";

		public override void Trigger()
		{
			mainscript.M.FoodCh(100);
		}
	}
}
