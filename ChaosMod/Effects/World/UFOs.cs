using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectUFOs : Effect
	{
		public override string Name => "UFOs!";
		public override string Type => "repeated";
		public override float Length => 30f;
		public override float Frequency => 2.5f;

		public override void Trigger()
		{
			temporaryTurnOffGeneration temp = mainscript.M.menu.GetComponentInChildren<temporaryTurnOffGeneration>();
			if (temp == null)
				return;

			if (temp.FEDOSPAWN == null)
				return;

			temp.FEDOSPAWN.DoSpawn();
		}
	}
}
