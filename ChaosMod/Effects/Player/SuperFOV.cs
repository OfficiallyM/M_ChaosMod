using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects.Player
{
	internal class EffectSuperFOV : Effect
	{
		public override string Name => "Super FOV";
		public override string Type => "timed";
		public override float Length => 30;

		private float fovFoot = 0;
		private float fovCar = 0;

		public override void Trigger()
		{
			fovFoot = settingsscript.s.S.FFieldOfViewFoot;
			fovCar = settingsscript.s.S.FFieldOfViewCar;
			settingsscript.s.S.FFieldOfViewFoot *= 1.75f;
			settingsscript.s.S.FFieldOfViewCar *= 1.75f;
		}

		public override void End()
		{
			settingsscript.s.S.FFieldOfViewFoot = fovFoot;
			settingsscript.s.S.FFieldOfViewCar = fovCar;
		}
	}
}
