using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectRandomiseCondition : Effect
	{
		public override string Name => "Randomise vehicle condition";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.lastCar != null)
			{
				carscript carscript = mainscript.M.player.lastCar;
				GameObject car = carscript.gameObject;
				partconditionscript[] partconditionscripts = car.GetComponentsInChildren<partconditionscript>();
				Modules.Utilities.Game.RandomiseCondition(partconditionscripts);
			}
		}
	}
}
