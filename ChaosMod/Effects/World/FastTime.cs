using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.World
{
	internal class EffectFastTime : Effect
	{
		public override string Name => "Look how the time flies";
		public override string Type => "fixedRepeated";
		public override float Length => 90;
		public override float Frequency => 0;

		public override void Trigger()
		{
			napszakvaltakozas time = mainscript.M.napszak;

			// Credit to Runden.
			time.tekerve = (time.tekeresSpeed * Time.fixedDeltaTime * time.tekeresHelp) * 0.25f;
			time.tekeres += mainscript.M.napszak.tekerve;
		}

		public override void End()
		{
			mainscript.M.napszak.tekerve = 0;
		}
	}
}
