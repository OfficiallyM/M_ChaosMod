using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectSandstorms : Effect
	{
		public override string Name => "I think that's a sandstorm!";
		public override string Type => "repeated";
		public override float Length => 30f;
		public override float Frequency => 10f;

		public override void Trigger()
		{
			temporaryTurnOffGeneration temp = mainscript.M.menu.GetComponentInChildren<temporaryTurnOffGeneration>();
			if (temp == null)
				return;

			temp.sandstormspawn.SpawnAtPlayer();
		}
	}
}
