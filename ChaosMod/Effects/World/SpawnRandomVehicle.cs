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
			Spawn(vehicles[index], color, true, 0, -1);
		}

		/// <summary>
		/// Based off mainscript Spawn method
		/// </summary>
		private void Spawn(GameObject gameObject, Color color, bool fullRandom, int condition, int variant)
		{
			GameObject spawned = UnityEngine.Object.Instantiate(gameObject, mainscript.M.player.transform.position + (mainscript.M.player.transform.forward * 4f) + (Vector3.up * 0.75f), Quaternion.FromToRotation(Vector3.forward, -mainscript.M.player.transform.right));
			partconditionscript component1 = spawned.GetComponent<partconditionscript>();
			if (component1 == null && spawned.GetComponent<childunparent>() != null)
				component1 = spawned.GetComponent<childunparent>().g.GetComponent<partconditionscript>();
			if (component1 != null)
			{
				if (variant != -1)
				{
					randomTypeSelector component2 = component1.GetComponent<randomTypeSelector>();
					if (component2 != null)
					{
						component2.forceStart = false;
						component2.rtipus = variant;
						component2.Refresh();
					}
				}

				if (fullRandom)
				{
					RandomiseCondition(component1);
				}
				else
				{
					component1.StartPaint(condition, color);
				}

				Paint(color, component1);
			}
			mainscript.M.PostSpawn(spawned);
		}

		/// <summary>
		/// Paint all child parts of a vehicle.
		/// </summary>
		/// <param name="c">The colour to paint</param>
		/// <param name="partconditionscript">The root vehicle partconditionscript</param>
		private void Paint(Color c, partconditionscript partconditionscript)
		{
			partconditionscript.Paint(c);
			foreach (partconditionscript child in partconditionscript.childs)
			{
				if (!child.isChild && !child.loaded)
					Paint(c, child);
			}
		}

		/// <summary>
		/// Randomise condition of all parts
		/// </summary>
		/// <param name="partconditionscript">Base vehicle partconditionscript</param>
		private void RandomiseCondition(partconditionscript partconditionscript)
		{
			List<partconditionscript> children = new List<partconditionscript>();
			FindPartChildren(partconditionscript, ref children);

			foreach (partconditionscript child in children)
			{
				child.RandomState(0, 4);
				child.Refresh();
			}
		}

		/// <summary>
		/// Recursively find all child parts
		/// </summary>
		/// <param name="root">Parent part</param>
		/// <param name="allChildren">Current list of child parts</param>
		private void FindPartChildren(partconditionscript root, ref List<partconditionscript> allChildren)
		{
			foreach (partconditionscript child in root.childs)
			{
				allChildren.Add(child);
				FindPartChildren(child, ref allChildren);
			}
		}
	}
}
