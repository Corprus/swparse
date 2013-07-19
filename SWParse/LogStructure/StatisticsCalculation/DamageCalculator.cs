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
            get { return (_calculableProperties["DamageDealtRecords"] as ICalculable<IEnumerable<LogRecord>>).Value; }
        }

        private IEnumerable<LogRecord> CritDamageDealtRecords
        {
            get { return (_calculableProperties["CritDamageDealtRecords"] as ICalculable<IEnumerable<LogRecord>>).Value; }
        }

        public int HitsDealt
        {
            get { return (_calculableProperties["HitsDealt"] as ICalculable<int>).Value; }
        }

        public int Damage
        {
            get { return (_calculableProperties["Damage"] as ICalculable<int>).Value; }
        }

        public int CritDamage
        {
            get { return (_calculableProperties["CritDamage"] as ICalculable<int>).Value; }
        }

        public int CritHitsDealt
        {
            get { return (_calculableProperties["CritHitsDealt"] as ICalculable<int>).Value; }
        }

        public int NormalDamage
        {
            get { return (_calculableProperties["NormalDamage"] as ICalculable<int>).Value; }
        }

        public double CritHitsPercent
        {
            get { return (_calculableProperties["CritHitsPercent"] as ICalculable<double>).Value; }
        }

        public double DPS
        {
            get { return (_calculableProperties["DPS"] as ICalculable<double>).Value; }
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
            _calculableProperties.Add("DamageDealtRecords",
                new CalculableProperty<IEnumerable<LogRecord>>(() =>
                    Battle.Where(rec =>
                             rec.Source.Name == Battle.LogOwner
                             && rec.Target.Name != Battle.LogOwner
                             && rec.Effect.Name == LogEffect.DamageString)
                    .ToList()));
            _calculableProperties.Add("CritDamageDealtRecords", 
                new CalculableProperty<IEnumerable<LogRecord>>(() => DamageDealtRecords.Where(rec => rec.Quantity.IsCrit).ToList()));
            _calculableProperties.Add("HitsDealt", new CalculableProperty<int>(() => DamageDealtRecords.Count()));
            _calculableProperties.Add("Damage",
                new CalculableProperty<int>(() => DamageDealtRecords.Sum(rec => rec.Quantity.Value)));
            _calculableProperties.Add("CritDamage", 
                new CalculableProperty<int>(() => CritDamageDealtRecords.Sum(rec => rec.Quantity.Value)));
            _calculableProperties.Add("CritHitsDealt",
                new CalculableProperty<int>(() => CritDamageDealtRecords.Count()));
            _calculableProperties.Add("NormalDamage",
                new CalculableProperty<int>(() => DamageDealtRecords.Where(rec => !rec.Quantity.IsCrit).Sum(rec => rec.Quantity.Value)));
            _calculableProperties.Add("CritHitsPercent",
                new CalculableProperty<double>(() => (double)HitsDealt > 0 ? (double)CritHitsDealt / (double)HitsDealt : 0));
            _calculableProperties.Add("DPS",
                new CalculableProperty<double>(() => Damage / Battle.Statistics.Duration.TotalSeconds));
        }
    }
}
