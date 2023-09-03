using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectMunkasInvasion : Effect
	{
		public override string Name => "Munkas invasion";
		public override string Type => "instant";

		public override void Trigger()
		{
			Vector3 playerPosition = mainscript.M.player.transform.position;

			int spawnCount = UnityEngine.Random.Range(5, 10);
			for (int i = 0; i <= spawnCount; i++)
			{
				Vector3 offset = new Vector3(UnityEngine.Random.Range(-15f, 15f), 0.75f, UnityEngine.Random.Range(-15f, 15f));
				Vector3 spawnPosition = playerPosition + offset;

				GameObject munkas = UnityEngine.Object.Instantiate(itemdatabase.d.gmunkas01, spawnPosition, mainscript.M.player.transform.rotation);
				munkas.transform.LookAt(mainscript.M.player.transform);
			}
		}
	}
}
