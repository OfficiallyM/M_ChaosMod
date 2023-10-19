using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectHalveRPM : Effect
	{
		public override string Name => "Halve engine RPM";
		public override string Type => "timed";
		public override float Length => 30;

		private float maxRpm = 0;

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				carscript carscript = mainscript.M.player.Car;
				enginescript engine = carscript.Engine;
				maxRpm = engine.maxRpm;
				engine.maxRpm = maxRpm / 2;
			}
		}

		public override void End()
		{
			if (mainscript.M.player.lastCar != null)
			{
				carscript carscript = mainscript.M.player.lastCar;
				enginescript engine = carscript.Engine;
				engine.maxRpm = maxRpm;
			}
		}
	}
}
