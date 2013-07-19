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
            get { return (_calculableProperties["DamageTakenRecords"] as ICalculable<IEnumerable<LogRecord>>).Value; }
        }
        
        private IEnumerable<LogRecord> CritDamageTakenRecords
        {
            get { return (_calculableProperties["CritDamageTakenRecords"] as ICalculable<IEnumerable<LogRecord>>).Value; }
        }

        public int HitsTaken
        {
            get { return (_calculableProperties["HitsTaken"] as ICalculable<int>).Value; }
        }

        public int DamageTaken
        {
            get { return (_calculableProperties["DamageTaken"] as ICalculable<int>).Value; }
        }

        public double DPSTaken
        {
            get { return (_calculableProperties["DPSTaken"] as ICalculable<double>).Value; }
        }

        public int CritDamageTaken
        {
            get { return (_calculableProperties["CritDamageTaken"] as ICalculable<int>).Value; }
        }

        public double CritHitsTakenPercent
        {
            get { return (_calculableProperties["CritHitsTakenPercent"] as ICalculable<double>).Value; }
        }

        public int NormalDamageTaken
        {
            get { return (_calculableProperties["NormalDamageTaken"] as ICalculable<int>).Value; }
        }

        public int CritTakenHits
        {
            get { return (_calculableProperties["CritTakenHits"] as ICalculable<int>).Value; }
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
            _calculableProperties.Add("DamageTakenRecords", 
                new CalculableProperty<IEnumerable<LogRecord>>(() => 
                    Battle.Where(rec => rec.Target.Name == Battle.LogOwner && rec.Effect.Name == LogEffect.DamageString).ToList()));
            _calculableProperties.Add("CritDamageTakenRecords",
                new CalculableProperty<IEnumerable<LogRecord>>(() => DamageTakenRecords.Where(rec => rec.Quantity.IsCrit)));
            _calculableProperties.Add("HitsTaken", new CalculableProperty<int>(() => DamageTakenRecords.Count()));
            _calculableProperties.Add("DamageTaken", new CalculableProperty<int>(() => DamageTakenRecords.Sum(rec => rec.Quantity.Value)));
            _calculableProperties.Add("DPSTaken", new CalculableProperty<double>(() => DamageTaken / Battle.Statistics.Duration.TotalSeconds));
            _calculableProperties.Add("CritDamageTaken", new CalculableProperty<int>(() => CritDamageTakenRecords.Sum(rec => rec.Quantity.Value)));
            _calculableProperties.Add("CritHitsTakenPercent",
                new CalculableProperty<double>(() => (double)HitsTaken > 0 ? (double)CritTakenHits / (double)HitsTaken : 0));
            _calculableProperties.Add("NormalDamageTaken", new CalculableProperty<int>(() => DamageTaken - CritDamageTaken));
            _calculableProperties.Add("CritTakenHits", new CalculableProperty<int>(() => CritDamageTakenRecords.Count()));
        }
    }
}
