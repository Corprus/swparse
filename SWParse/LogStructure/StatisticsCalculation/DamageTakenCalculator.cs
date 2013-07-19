using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure.StatisticsCalculation
{
    internal class DamageTakenCalculator : Calculator
    {
        public DamageTakenCalculator(LogBattle battle)
            :base(battle)
        {
            
        }

        private IEnumerable<LogRecord> DamageTakenRecords
        {
            get { return Battle.Where(rec => rec.Target.Name == Battle.LogOwner && rec.Effect.Name == LogEffect.DamageString); }
        }
        private IEnumerable<LogRecord> CritDamageTakenRecords
        {
            get { return DamageTakenRecords.Where(rec => rec.Quantity.IsCrit); }
        }

        public int HitsTaken
        {
            get { return DamageTakenRecords.Count(); }
        }

        public long DamageTaken
        {
            get { return DamageTakenRecords.Sum(rec => rec.Quantity.Value); }
        }

        public double DPSTaken
        {
            get { return DamageTaken / Battle.Statistics.Duration.TotalSeconds; }
        }

        public long CritDamageTaken
        {
            get { return CritDamageTakenRecords.Sum(rec => rec.Quantity.Value); }
        }

        public double CritHitsTakenPercent
        {
            get { return (double)HitsTaken > 0 ? (double)CritTakenHits / (double)HitsTaken : 0; }
        }

        public long NormalDamageTaken
        {
            get { return DamageTaken - CritDamageTaken; }
        }

        public int CritTakenHits
        {
            get { return CritDamageTakenRecords.Count(); }
        }

        public override void Calculate()
        {
#warning should be implemented to prevent overcalculating;
        }

        public override string GetLog()
        {
            var text = string.Join(Environment.NewLine, new[]
                {
                    string.Format("Hits: {0}", HitsTaken),
                    string.Format("Damage: {0}", DamageTaken),
                    string.Format("DPS: {0:0.##}", DPSTaken),
                    string.Format("Crit Damage: {0}", CritDamageTaken),
                    string.Format("Crit hits: {0}", CritTakenHits),
                    string.Format("Crit Damage %: {0: 0.##}", CritHitsTakenPercent*100)
                });

            return text;
        }

        protected override void InitProperties()
        {
        }
    }
}
