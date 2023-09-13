using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects.Player
{
	internal class EffectBlind : Effect
	{
		public override string Name => "Blind";
		public override string Type => "timed";
		public override float Length => 10;
		public override bool UseGUI => true;

		private GUIStyle style = null;

		private int resolutionX = 0;
		private int resolutionY = 0;

		public EffectBlind(int _resolutionX, int _resolutionY)
		{
			resolutionX = _resolutionX;
			resolutionY = _resolutionY;
		}

		public override void OnGUI()
		{
			if (style == null)
			{
				style = new GUIStyle(GUI.skin.box);
				style.normal.background = Modules.Utilities.GUI.ColorTexture(2, 2, new Color(0, 0, 0));
			}

			GUI.Box(new Rect(0, 0, resolutionX, resolutionY), string.Empty, style);
		}
	}
}
