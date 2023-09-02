using ChaosMod.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Core
{
	public class ActiveEffect
	{
		public Effect Effect { get; set; }
		public float Remaining { get; set; }
		public float TriggerRemaining { get; set; }
	}
}
