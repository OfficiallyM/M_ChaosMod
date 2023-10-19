using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectPristine : Effect
	{
		public override string Name => "Pristine vehicle";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.lastCar != null)
			{
				carscript carscript = mainscript.M.player.lastCar;
				GameObject car = carscript.gameObject;
				partconditionscript condition = car.GetComponent<partconditionscript>();
				condition.state = 0;
				condition.Refresh();

				List<partconditionscript> parts = new List<partconditionscript>();
				Modules.Utilities.Game.FindPartChildren(condition, ref parts);
				foreach (partconditionscript part in parts)
				{
					part.state = 0;
					part.Refresh();
				}
			}
		}
	}
}
