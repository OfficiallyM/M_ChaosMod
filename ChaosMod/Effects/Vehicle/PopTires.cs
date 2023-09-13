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
			if (mainscript.M.player.Car != null)
			{
				carscript carscript = mainscript.M.player.lastCar;
				GameObject car = carscript.gameObject;
				List<partslotscript> slots = new List<partslotscript>();
				foreach (partslotscript slot in car.GetComponentsInChildren<partslotscript>())
				{
					if (slot.part != null)
					{
						slots.Add(slot);
						Modules.Utilities.Game.FindAllParts(slot, ref slots);
					}
				}

				foreach (partslotscript slot in slots)
				{
					if (slot.tipus[0] == "gumi" && slot.part != null)
					{
						if (slot.part.gameObject != null)
						{
							GameObject part = slot.part.gameObject;
							Vector3 position = part.transform.position;
							slot.part.FallOFf();
							tosaveitemscript save = part.GetComponent<tosaveitemscript>();
							if (save != null)
							{
								save.removeFromMemory = true;
							}
							UnityEngine.Object.Destroy(part.transform.root.gameObject);
							carscript.RB.AddForceAtPosition(new Vector3(0f, 1500f, 0f), position, ForceMode.Impulse);
						}
					}
				}
			}
		}
	}
}
