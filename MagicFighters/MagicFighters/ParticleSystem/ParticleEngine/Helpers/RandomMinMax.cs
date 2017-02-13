using System;

namespace MagicFighters.ParticleEngine.Helpers
{
	public class RandomMinMax
	{
		public double Min { get; set; }
		public double Max { get; set; }

		public RandomMinMax(double value)
			: this(value, value)
		{
		}

		public RandomMinMax(double min, double max)
		{
			Min = min;
			Max = max;
		}
	}
}
