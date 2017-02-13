using System;
namespace MagicFighters.ParticleEngine.Helpers
{
	class EmitterHelper
	{
		Random random = new Random();

		public double RandomizedDouble(RandomMinMax randomMinMax)
		{
			double min = randomMinMax.Min;
			double max = randomMinMax.Max;

			if (min == max)
				return max;
			else
				return min + (random.NextDouble() * (max - min));
		}
	}
}
