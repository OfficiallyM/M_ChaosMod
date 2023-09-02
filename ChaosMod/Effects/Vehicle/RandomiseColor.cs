using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectRandomiseColor : Effect
	{
		public override string Name => "Randomise vehicle color";
		public override string Type => "instant";

		public override void Trigger()
		{
			if (mainscript.M.player.lastCar != null)
			{
				carscript carscript = mainscript.M.player.lastCar;
				GameObject car = carscript.gameObject;
				partconditionscript partconditionscript = car.GetComponent<partconditionscript>();
				Color color = new Color();
				color.r = UnityEngine.Random.Range(0f, 255f) / 255f;
				color.g = UnityEngine.Random.Range(0f, 255f) / 255f;
				color.b = UnityEngine.Random.Range(0f, 255f) / 255f;
				Paint(color, partconditionscript);
			}
		}

		/// <summary>
		/// Paint all child parts of a vehicle.
		/// </summary>
		/// <param name="c">The colour to paint</param>
		/// <param name="partconditionscript">The root vehicle partconditionscript</param>
		private void Paint(Color c, partconditionscript partconditionscript)
		{
			partconditionscript.Paint(c);
			foreach (partconditionscript child in partconditionscript.childs)
			{
				if (!child.isChild && !child.loaded)
					Paint(c, child);
			}
		}
	}
}
