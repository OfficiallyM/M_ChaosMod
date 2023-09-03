using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectDropParts : Effect
	{
		public override string Name => "Drop all car parts";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				carscript carscript = mainscript.M.player.Car;
				GameObject car = carscript.gameObject;

				partslotscript[] slots = car.GetComponentsInChildren<partslotscript>();
				foreach (partslotscript slot in slots)
				{
					if (slot.part != null)
						slot.part.FallOFf();
				}
			}
		}
	}
}
