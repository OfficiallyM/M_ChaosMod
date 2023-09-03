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
				RandomiseCondition(partconditionscripts);
			}
		}

		/// <summary>
		/// Randomise condition of all parts.
		/// </summary>
		/// <param name="partconditionscripts">Array of partconditionscripts</param>
		private void RandomiseCondition(partconditionscript[] partconditionscripts)
		{
			foreach (partconditionscript child in partconditionscripts)
			{
				if (child.gameObject != null)
				{
					child.RandomState(0, 4);
					child.Refresh();
				}
			}
		}

		/// <summary>
		/// Recursively find all child parts.
		/// </summary>
		/// <param name="root">Parent part</param>
		/// <param name="allChildren">Current list of child parts</param>
		private void FindPartChildren(partconditionscript root, ref List<partconditionscript> allChildren)
		{
			foreach (partconditionscript child in root.childs)
			{
				allChildren.Add(child);
				FindPartChildren(child, ref allChildren);
			}
		}
	}
}
