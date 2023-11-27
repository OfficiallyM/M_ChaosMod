using ChaosMod.Core;
using ChaosMod.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDLoader;
using UnityEngine;

namespace ChaosMod.Effects.Player
{
	internal class EffectSmallTeleport : Effect
	{
		public override string Name => "Teleport";
		public override string Type => "instant";

		public override void Trigger()
		{
			Transform target = mainscript.M.player.transform;
			if (mainscript.M.player.Car != null)
				target = mainscript.M.player.Car.transform;

			Vector3[] directions = new Vector3[]
			{
				Vector3.up,
				Vector3.forward,
				Vector3.right,
				Vector3.back,
				Vector3.left
			};

			int random = UnityEngine.Random.Range(0, directions.Length);
			Vector3 direction = directions[random];

			target.position += direction * UnityEngine.Random.Range(2f, 10f);
		}
	}
}
