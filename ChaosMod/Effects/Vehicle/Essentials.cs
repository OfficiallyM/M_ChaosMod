using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectEssentials : Effect
	{
		public override string Name => "Just the essentials";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				carscript carscript = mainscript.M.player.Car;
				GameObject car = carscript.gameObject;

				List<string> whitelist = new List<string>()
				{
					"felni",
					"gumi",
					"coolanttank",
					"engine",
					"tank"
				};

				partslotscript[] slots = car.GetComponentsInChildren<partslotscript>();
				foreach (partslotscript slot in slots)
				{
					bool drop = true;
					foreach (var tipus in slot.tipus)
					{
						if (whitelist.Contains(tipus))
							drop = false;
					}

					if (slot.part != null && drop)
						slot.part.FallOFf();
				}
			}
		}
	}
}
