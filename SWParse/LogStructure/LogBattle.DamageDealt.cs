using System.Collections.Generic;
using System.Linq;

namespace SWParse.LogStructure
{
    internal  partial class LogBattle
    {
        private IEnumerable<LogRecord> DamageDealtRecords
        {
            get { return this.Where(rec => rec.Source.Name == LogOwner && rec.Target.Name != LogOwner && rec.Effect.Effect == LogEffect.DamageString); }
        }

        private IEnumerable<LogRecord>  CritDamageDealtRecords
        {
            get { return DamageDealtRecords.Where(rec => rec.Quantity.IsCrit); }
        }

        public int HitsDealt
        {
            get { return DamageDealtRecords.Count(); }
        }

        public long Damage
        {
            get { return DamageDealtRecords.Sum(rec => rec.Quantity.Quantity); }
        }

        public long CritDamage
        {
            get
            {
                return CritDamageTakenRecords.Sum(rec => rec.Quantity.Quantity);
            }
        }

        public int CritHitsDealt
        {
            get { return CritDamageDealtRecords.Count(); }
        }

        public long NormalDamage
        {
            get
            {
                return DamageDealtRecords.Where(rec => !rec.Quantity.IsCrit).Sum(rec => rec.Quantity.Quantity);
            }
        }

        public double CritHitsPercent
        {
            get { return (double)HitsDealt > 0 ? (double)CritHitsDealt / (double)HitsDealt : 0; }
        }

        public double DPS
        {
            get { return Damage / Duration.TotalSeconds; }
        }
    }
}
