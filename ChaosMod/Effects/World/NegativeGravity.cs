using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectNegativeGravity : Effect
	{
		public override string Name => "Negative gravity";
		public override string Type => "timed";
		public override float Length => 10;


		private Vector3 gravity = Physics.gravity;

		public override void Trigger()
		{
			Physics.gravity = new Vector3(0, 0.75f, 0);
		}

		public override void End()
		{
			Physics.gravity = gravity;
		}
	}
}
