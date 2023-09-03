using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectRainingShit : Effect
	{
		public override string Name => "It's raining shit";
		public override string Type => "repeated";
		public override float Frequency => 0.5f;
		public override float Length => 30f;

		public override void Trigger()
		{
			Vector3 basePosition = mainscript.M.player.transform.position + mainscript.M.player.transform.up * 12f;

			int shitSpawnCount = UnityEngine.Random.Range(3, 15);
			for (int i = 0; i <= shitSpawnCount; i++)
			{
				Vector3 offset = new Vector3(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(0, 4f), UnityEngine.Random.Range(-4f, 4f));
				Vector3 spawnPosition = basePosition + offset;

				UnityEngine.Object.Instantiate(itemdatabase.d.gturd, spawnPosition, mainscript.M.player.transform.rotation);
			}
		}
	}
}
