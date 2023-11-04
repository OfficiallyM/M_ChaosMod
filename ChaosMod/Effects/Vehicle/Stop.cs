using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectStop : Effect
	{
		public override string Name => "Emergency stop";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.Car != null)
			{
				carscript car = mainscript.M.player.Car;
				car.lastPos = car.transform.position;
				car.lspeed = car.speed = 0.0f;
				car.RB.velocity = car.RB.angularVelocity = Vector3.zero;
			}
		}
	}
}
