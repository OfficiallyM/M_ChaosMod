using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects
{
	internal class Test : Effect
	{
		public override string Name => "Test effect";
		public override string Type => "instant";

		public override void Trigger()
		{
			mainscript.M.player.AJump();
		}
	}
}
