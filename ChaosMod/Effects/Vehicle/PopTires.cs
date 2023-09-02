using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectPopTires : Effect
	{
		public override string Name => "Pop current vehicle tires";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.lastCar != null)
			{
				carscript carscript = mainscript.M.player.lastCar;
				GameObject car = carscript.gameObject;
				List<partslotscript> slots = new List<partslotscript>();
				foreach (partslotscript slot in car.GetComponentsInChildren<partslotscript>())
				{
					slots.Add(slot);
					FindAllParts(slot, ref slots);
				}

				foreach (partslotscript slot in slots)
				{
					if (slot.tipus.Contains("gumi"))
					{
						Vector3 position = slot.part.gameObject.transform.position;
						slot.part.FallOFf();
						carscript.RB.AddForceAtPosition(new Vector3(0f, 1500f, 0f), position, ForceMode.Impulse);
					}
				}
			}
		}

		/// <summary>
		/// Recursively find all populated part slots
		/// </summary>
		/// <param name="slot">The slot to search through</param>
		/// <param name="allChildren">The existing parts list</param>
		private void FindAllParts(partslotscript slot, ref List<partslotscript> allChildren)
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
