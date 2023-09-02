using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectLowGravity : Effect
	{
		public override string Name => "Low gravity";
		public override string Type => "timed";
		public override float Length => 30;


		private Vector3 gravity = Physics.gravity;

		public override void Trigger()
		{
			Physics.gravity = new Vector3(0, -2, 0);
		}

		public override void End()
		{
			Physics.gravity = gravity;
		}
	}
}
