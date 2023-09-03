using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectRandomiseTime : Effect
	{
		public override string Name => "Randomise time";
		public override string Type => "instant";

		public override void Trigger()
		{
			mainscript.M.napszak.tekeres += UnityEngine.Random.Range(100, 500);
		}
	}
}
