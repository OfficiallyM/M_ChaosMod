using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Vehicle
{
	internal class EffectReplaceWheels : Effect
	{
		public override string Name => "Replace wheels";
		public override string Type => "instant";

		private List<string> smallWheels = new List<string>()
		{
			"Bike01Rim",
			"Bike03Rim",
			"Bike01Wheel",
			"Bike03Wheel"
		};
		private List<string> mediumWheels = new List<string>()
		{
			"TireFelni",
			"TireFelni Variant"
		};
		private List<string> largeWheels = new List<string>()
		{
			"Bus01TireFelni",
			"Bus01WheelFull",
		};

		private string smallReplace = "Bike01Wheel";
		private string mediumReplace = "TireFelni Variant";
		private string largeReplace = "Bus01WheelFull";

		public override void Trigger()
		{
			if (mainscript.M.player.lastCar != null)
			{
				carscript carscript = mainscript.M.player.lastCar;
				GameObject car = carscript.gameObject;

				//List<partslotscript> slots = Modules.Utilities.Game.FindAllParts(carscript);
				List<partslotscript> slots = car.GetComponentsInChildren<partslotscript>().ToList();

				List<partslotscript> wheels = slots.Where(s => s.tipus.Contains("felni")).ToList();

				// Randomise for medium wheels now to ensure all wheels randomise the same.
				int rand = UnityEngine.Random.Range(0, 2);

				foreach (partslotscript wheel in wheels)
				{
					string wheelPartName = wheel.part.gameObject.name;

					// Tidy up part name.
					wheelPartName = wheelPartName.Replace("(Clone)", string.Empty);

					Transform transform = wheel.part.gameObject.transform;

					GameObject newWheelObject = null;

					if (smallWheels.Contains(wheelPartName) || largeWheels.Contains(wheelPartName))
					{
						// Replace with medium wheels.
						newWheelObject = itemdatabase.d.items.Where(i => i.name == mediumReplace).FirstOrDefault();
					}
					else if (mediumWheels.Contains(wheelPartName))
					{
						// Currently has medium wheels, randomise to either small or large.
						if (rand == 0)
							newWheelObject = itemdatabase.d.items.Where(i => i.name == smallReplace).FirstOrDefault();
						else
							newWheelObject = itemdatabase.d.items.Where(i => i.name == largeReplace).FirstOrDefault();
					}

					if (newWheelObject != null)
					{
						GameObject oldWheel = wheel.part.gameObject;
						GameObject newWheel = UnityEngine.Object.Instantiate(newWheelObject, transform.position, transform.rotation);

						wheel.UnCraft(true);
						Modules.Utilities.Game.Destroy(oldWheel);

						partscript wheelPart = newWheel.GetComponent<partscript>();
						wheel.Craft(wheelPart);
					}
				}
			}
		}
	}
}
