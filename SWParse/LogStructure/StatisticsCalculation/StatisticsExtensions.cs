using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure.StatisticsCalculation
{
    public static class StatisticsExtensions
    {
        public static int CalculateOverheal(this LogRecord rec)
        {
            if (rec.Effect.Name != LogEffect.HealString)
            {
                return 0;
            }
            return (int) (rec.Quantity.Value - (rec.Threat*2/(rec.HealThreatMultiplier*(rec.Guarded ? 0.75 : 1))));
        }
    }
}
