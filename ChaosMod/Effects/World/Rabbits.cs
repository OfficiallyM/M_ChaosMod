using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectRabbits : Effect
	{
		public override string Name => "Rabbits!";
		public override string Type => "instant";

		public override void Trigger()
		{
			Vector3 playerPosition = mainscript.M.player.transform.position;

			int spawnCount = UnityEngine.Random.Range(5, 10);
			for (int i = 0; i <= spawnCount; i++)
			{
				Vector3 offset = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0.75f, UnityEngine.Random.Range(-10f, 10f));
				Vector3 spawnPosition = playerPosition + offset;

				GameObject munkas = UnityEngine.Object.Instantiate(itemdatabase.d.cnyul, spawnPosition, mainscript.M.player.transform.rotation);
				munkas.transform.LookAt(mainscript.M.player.transform);
			}
		}
	}
}
