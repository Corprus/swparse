using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure.StatisticsCalculation
{
    internal class DamageCalculator : Calculator
    {
        public DamageCalculator(LogBattle battle)
            : base(battle)
        {
        }

        private IEnumerable<LogRecord> DamageDealtRecords
        {
            get { return Battle.Where(rec => rec.Source.Name == Battle.LogOwner && rec.Target.Name != Battle.LogOwner && rec.Effect.Name == LogEffect.DamageString); }
        }

        private IEnumerable<LogRecord> CritDamageDealtRecords
        {
            get { return DamageDealtRecords.Where(rec => rec.Quantity.IsCrit); }
        }

        public int HitsDealt
        {
            get { return DamageDealtRecords.Count(); }
        }

        public long Damage
        {
            get { return DamageDealtRecords.Sum(rec => rec.Quantity.Value); }
        }

        public long CritDamage
        {
            get
            {
                return CritDamageDealtRecords.Sum(rec => rec.Quantity.Value);
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
                return DamageDealtRecords.Where(rec => !rec.Quantity.IsCrit).Sum(rec => rec.Quantity.Value);
            }
        }

        public double CritHitsPercent
        {
            get { return (double)HitsDealt > 0 ? (double)CritHitsDealt / (double)HitsDealt : 0; }
        }

        public double DPS
        {
            get { return Damage / Battle.Statistics.Duration.TotalSeconds; }
        }

        public override void Calculate()
        {
#warning should be implemented to prevent overcalculating;
        }

        public override string GetLog()
        {
            var text = string.Join(Environment.NewLine, new[]
                {
                    string.Format("Hits: {0}", HitsDealt),
                    string.Format("Damage: {0}", Damage),
                    string.Format("DPS: {0:0.##}", DPS),
                    string.Format("Crit Damage: {0}", CritDamage),
                    string.Format("Crit Hits: {0}", CritHitsDealt),
                    string.Format("Crit Hits %: {0: 0.##}", CritHitsPercent*100)
                });

            return text;
        }

        protected override void InitProperties()
        {
        }
    }
}
