using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure
{
    internal partial class LogBattle
    {
        private IEnumerable<LogRecord> DamageTakenRecords
        {
            get { return this.Where(rec => rec.Target.Name == LogOwner  && rec.Effect.Name == LogEffect.DamageString); }
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
            get { return DamageTaken / Duration.TotalSeconds; }
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


    }
}
