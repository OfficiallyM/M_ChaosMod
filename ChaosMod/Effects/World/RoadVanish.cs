using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectRoadVanish : Effect
	{
		public override string Name => "Uh oh, where'd the road go?";
		public override string Type => "timed";
		public override float Length => 60f;

		private GameObject roadParent = null;

		public EffectRoadVanish(GameObject _roadParent)
		{
			roadParent = _roadParent;
		}

		public override void Trigger()
		{
			roadParent.SetActive(false);
		}

		public override void End()
		{
			roadParent.SetActive(true);
		}
	}
}
