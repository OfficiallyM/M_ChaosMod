using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Modules.Utilities
{
	/// <summary>
	/// Game interaction utilities.
	/// </summary>
	public static class Game
	{
		/// <summary>
		/// Based off mainscript Spawn method.
		/// </summary>
		public static void Spawn(GameObject gameObject, Color color)
		{
			GameObject spawned = UnityEngine.Object.Instantiate(gameObject, mainscript.M.player.transform.position + (mainscript.M.player.transform.forward * 4f) + (Vector3.up * 0.75f), Quaternion.FromToRotation(Vector3.forward, -mainscript.M.player.transform.right));
			partconditionscript component1 = spawned.GetComponent<partconditionscript>();
			if (component1 == null && spawned.GetComponent<childunparent>() != null)
				component1 = spawned.GetComponent<childunparent>().g.GetComponent<partconditionscript>();
			if (component1 != null)
			{
				RandomiseCondition(component1);
				Paint(color, component1);
			}
			mainscript.M.PostSpawn(spawned);
		}

		/// <summary>
		/// Paint all child parts of a vehicle.
		/// </summary>
		/// <param name="c">The colour to paint</param>
		/// <param name="partconditionscript">The root vehicle partconditionscript</param>
		public static void Paint(Color c, partconditionscript partconditionscript)
		{
			partconditionscript.Paint(c);
			foreach (partconditionscript child in partconditionscript.childs)
			{
				if (!child.isChild && !child.loaded)
					Paint(c, child);
			}
		}

		/// <summary>
		/// Randomise condition of all parts.
		/// </summary>
		/// <param name="partconditionscript">Base vehicle partconditionscript</param>
		public static void RandomiseCondition(partconditionscript partconditionscript)
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
		/// Randomise condition of all parts.
		/// </summary>
		/// <param name="partconditionscripts">Array of partconditionscripts</param>
		public static void RandomiseCondition(partconditionscript[] partconditionscripts)
		{
			foreach (partconditionscript child in partconditionscripts)
			{
				if (child.gameObject != null)
				{
					child.RandomState(0, 4);
					child.Refresh();
				}
			}
		}

		/// <summary>
		/// Recursively find all child parts.
		/// </summary>
		/// <param name="root">Parent part</param>
		/// <param name="allChildren">Current list of child parts</param>
		public static void FindPartChildren(partconditionscript root, ref List<partconditionscript> allChildren)
		{
			foreach (partconditionscript child in root.childs)
			{
				allChildren.Add(child);
				FindPartChildren(child, ref allChildren);
			}
		}

		/// <summary>
		/// Recursively find all populated part slots
		/// </summary>
		/// <param name="slot">The slot to search through</param>
		/// <param name="allChildren">The existing parts list</param>
		public static void FindAllParts(partslotscript slot, ref List<partslotscript> allChildren)
		{
			if (slot.part != null)
			{
				allChildren.Add(slot);

				foreach (var subslot in slot.part.tosaveitem.partslotscripts)
				{
					FindAllParts(subslot, ref allChildren);
				}
			}
		}
	}
}
