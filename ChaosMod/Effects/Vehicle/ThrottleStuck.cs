using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectThrottleStuck : Effect
	{
		public override string Name => "HELP! The throttle is stuck";
		public override string Type => "timed";
		public override float Length => 30;

		public override void ModEventCallback(object sender, string eventType)
		{
			if (eventType == "postCarGetPlayerInput")
			{
				carscript carscript = sender as carscript;
				carscript.ithrottle = 100f;
			}
		}
	}
}
