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
	internal class EffectHydrated : Effect
	{
		public override string Name => "Hydrated";
		public override string Type => "instant";

		public override void Trigger()
		{
			mainscript.M.WaterCh(100);
		}
	}
}
