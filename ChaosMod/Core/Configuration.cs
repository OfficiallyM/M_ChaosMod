using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static ChaosMod.Modules.Keybinds;

namespace ChaosMod.Core
{
	[DataContract]
	internal class Configuration
	{
		[DataMember] public List<Key> keybinds { get; set; }
		[DataMember] public List<string> enabledEffects { get; set; }
		[DataMember] public List<string> allEffects { get; set; }
		[DataMember] public float? timerLength { get; set; }
	}

	internal class Settings
	{
		public static float s_timerLength = 30f;
		public float timerLength
		{
			get { return s_timerLength; }
			set { s_timerLength = value; }
		}

		public static float s_currentEffectDelay = s_timerLength;
		public float currentEffectDelay
		{
			get { return s_currentEffectDelay; }
			set { s_currentEffectDelay = value; }
		}

		public static float s_effectDelay = s_currentEffectDelay;
		public float effectDelay
		{
			get { return s_effectDelay; }
			set { s_effectDelay = value; }
		}

		public static List<Effect> s_enabledEffects = new List<Effect>();
		public List<Effect> enabledEffects
		{
			get { return s_enabledEffects; }
			set { s_enabledEffects = value; }
		}
	}
}
