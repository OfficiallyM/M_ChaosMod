using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectSpammyDoors : Effect
	{
		public override string Name => "Spammy doors";
		public override string Type => "repeated";
		public override float Frequency => 0.2f;
		public override float Length => 30;

		private bool toggle = false;

		public override void Trigger()
		{
			if (mainscript.M.player.lastCar != null)
			{
				carscript carscript = mainscript.M.player.lastCar;
				GameObject car = carscript.gameObject;
				usablescript[] useables = car.GetComponentsInChildren<usablescript>();

				foreach (usablescript useable in useables)
				{
					float val = 100f;
					if (!toggle)
						val = -100f;
					useable.Rot(val, val);
				}
				toggle = !toggle;
			}
		}
	}
}
