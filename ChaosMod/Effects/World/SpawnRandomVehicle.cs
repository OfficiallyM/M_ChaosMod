using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectSpawnRandomVehicle : Effect
	{
		public override string Name => "Spawn random vehicle";
		public override string Type => "instant";

		public override void Trigger()
		{
			List<GameObject> vehicles = new List<GameObject>();
			foreach (GameObject gameObject in itemdatabase.d.items)
			{
				if (gameObject.name.ToLower().Contains("full") && gameObject.GetComponentsInChildren<carscript>().Length > 0)
					vehicles.Add(gameObject);
			}

			Color color = new Color();
			color.r = UnityEngine.Random.Range(0f, 255f) / 255f;
			color.g = UnityEngine.Random.Range(0f, 255f) / 255f;
			color.b = UnityEngine.Random.Range(0f, 255f) / 255f;

			int index = UnityEngine.Random.Range(0, vehicles.Count);
			Modules.Utilities.Game.Spawn(vehicles[index], color);
		}
	}
}
