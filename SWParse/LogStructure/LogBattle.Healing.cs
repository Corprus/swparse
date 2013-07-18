using System.Collections.Generic;
using System.Linq;

namespace SWParse.LogStructure
{
    internal partial class LogBattle
    {
        private IEnumerable<LogRecord> HealsGivenRecords
        {
            get { return this.Where(rec => rec.Source.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString); }
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
                    this.Where(rec => rec.Target.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString)
                        .Sum(rec => rec.Quantity.Quantity);
            }
        }

        public double HPS
        {
            get { return HealsGiven / Duration.TotalSeconds; }
        }

        public long Overheal
        {
            get
            {
                return
                    (long)this.Where(rec => rec.Source.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString)
                               .Sum(
                                   rec =>
                                   rec.Quantity.Quantity -
                                   (rec.Threat * 2 / (rec.HealThreatMultiplier * (rec.Guarded ? 0.75 : 1))));
            }
        }

        public long EffectiveHeal
        {
            get { return HealsGiven - Overheal; }
        }

        public double EHPS
        {
            get { return (HealsGiven - Overheal) / Duration.TotalSeconds; }
        }

        public double EffectiveHealsPercent
        {
            get { return HealsGiven != 0 ? (double)EffectiveHeal / (double)HealsGiven : 0; }
        }

        public long HealsGiven
        {
            get { return HealsGivenRecords.Sum(rec => rec.Quantity.Quantity); }
        }

        public long NormalHeals
        {
            get
            {
                return HealsGivenRecords.Where(rec => !rec.Quantity.IsCrit).Sum(rec => rec.Quantity.Quantity);
            }
        }

        public long CritHeals
        {
            get
            {
                return CritHealsGivenRecords.Sum(rec => rec.Quantity.Quantity);
            }
        }
    }
}