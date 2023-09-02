﻿using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects
{
	internal class EffectBhop : Effect
	{
		public override string Name => "Bhop scripting";
		public override string Type => "repeated";
		public override float Length => 10;
		public override float Frequency => 0.75f;

		public override void Trigger()
		{
			mainscript.M.player.AJump();
		}
	}
}