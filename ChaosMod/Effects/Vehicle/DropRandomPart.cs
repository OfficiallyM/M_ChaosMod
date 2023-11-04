using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDLoader;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectDropRandomPart : Effect
	{
		public override string Name => "Drop a random vehicle part";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				carscript carscript = mainscript.M.player.Car;
				GameObject car = carscript.gameObject;

				partslotscript[] slots = car.GetComponentsInChildren<partslotscript>();
				partslotscript slot = slots[UnityEngine.Random.Range(0, slots.Length)];
				slot.part?.FallOFf();
			}
		}
	}
}
