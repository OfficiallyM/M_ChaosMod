using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDLoader;

namespace ChaosMod.Effects.Player
{
	internal class EffectGodmode : Effect
	{
		public override string Name => "Godmode";
		public override string Type => "timed";
		public override float Length => 60;

		public override void Trigger()
		{
			mainscript.M.ChGodMode(true);
		}

		public override void End()
		{
			mainscript.M.ChGodMode(false);
		}
	}
}
