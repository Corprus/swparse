using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure.StatisticsCalculation
{
    internal class HealCalculator : Calculator
    {
        public HealCalculator(LogBattle battle)
            :base(battle)
        {            
        }

        private IEnumerable<LogRecord> HealsGivenRecords
        {
            get { return Battle.Where(rec => rec.Source.Name == Battle.LogOwner && rec.Effect.Name == LogEffect.HealString); }
        }

        private IEnumerable<LogRecord> CritHealsGivenRecords
        {
            get { return HealsGivenRecords.Where(rec => rec.Quantity.IsCrit); }
        }

        public int HealsGivenCount
        {
            get { return HealsGivenRecords.Count(); }
        }

        public int CritHealsGivenCount
        {
            get { return CritHealsGivenRecords.Count(); }
        }

        public double CritHealsPercent
        {
            get { return HealsGivenCount > 0 ? (double)CritHealsGivenCount / (double)HealsGivenCount : 0; }
        }

        public long HealsTaken
        {
            get
            {
                return
                    Battle.Where(rec => rec.Target.Name == Battle.LogOwner && rec.Effect.Name == LogEffect.HealString)
                        .Sum(rec => rec.Quantity.Value);
            }
        }

        public double HPS
        {
            get { return HealsGiven / Battle.Statistics.Duration.TotalSeconds; }
        }

        public long Overheal
        {
            get
            {
                return
                    (long)Battle.Where(rec => rec.Source.Name == Battle.LogOwner && rec.Effect.Name == LogEffect.HealString)
                               .Sum(
                                   rec =>
                                   rec.Quantity.Value -
                                   (rec.Threat * 2 / (rec.HealThreatMultiplier * (rec.Guarded ? 0.75 : 1))));
            }
        }

        public long EffectiveHeal
        {
            get { return HealsGiven - Overheal; }
        }

        public double EHPS
        {
            get { return (HealsGiven - Overheal) / Battle.Statistics.Duration.TotalSeconds; }
        }

        public double EffectiveHealsPercent
        {
            get { return HealsGiven != 0 ? (double)EffectiveHeal / (double)HealsGiven : 0; }
        }

        public long HealsGiven
        {
            get { return HealsGivenRecords.Sum(rec => rec.Quantity.Value); }
        }

        public long NormalHeals
        {
            get
            {
                return HealsGivenRecords.Where(rec => !rec.Quantity.IsCrit).Sum(rec => rec.Quantity.Value);
            }
        }

        public long CritHeals
        {
            get
            {
                return CritHealsGivenRecords.Sum(rec => rec.Quantity.Value);
            }
        }

        public override string GetLog()
        {
            var text = string.Join(Environment.NewLine, new[]
                {
                    string.Format("Heals: {0}", HealsGivenCount),
                    string.Format("Heals Given: {0}", HealsGiven),
                    string.Format("Overheal: {0}", Overheal),
                    string.Format("Normal Heal: {0}", NormalHeals),
                    string.Format("Crit Heal Count: {0}", CritHealsGivenCount),
                    string.Format("Crit Heals: {0}", CritHeals),
                    string.Format("Crit Heals %: {0:0.##}%", CritHealsPercent*100),
                    string.Format("HPS: {0:0.##}", HPS),
                    string.Format("EHPS: {0:0.##}", EHPS),
                    string.Format("Effective healing%: {0:0.##}%", EffectiveHealsPercent*100)
                });

            return text;
        }

        public override void Calculate()
        {
#warning should be implemented to prevent overcalculating;
        }
    }
}
