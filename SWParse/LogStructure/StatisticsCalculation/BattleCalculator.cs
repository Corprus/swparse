using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure.StatisticsCalculation
{
    internal class BattleCalculator: Calculator
    {
        public BattleCalculator(LogBattle battle)
            :base(battle)
        {
            Heals = new HealCalculator(Battle);
            Damage = new DamageCalculator(Battle);
            DamageTaken = new DamageTakenCalculator(Battle);
            Threat = new ThreatCalculator(Battle);
        }

        #region Calculators
        public HealCalculator Heals { get; private set; }
        public DamageCalculator Damage { get; private set; }
        public DamageTakenCalculator DamageTaken { get; private set; }
        public ThreatCalculator Threat { get; private set; }
        #endregion

        public DateTime Start
        {
            get { return (_calculableProperties["Start"] as ICalculable<DateTime>).Value; }
        }

        public DateTime End
        {
            get { return (_calculableProperties["End"] as ICalculable<DateTime>).Value; }
        }

        public TimeSpan Duration
        {
            get { return (_calculableProperties["Duration"] as ICalculable<TimeSpan>).Value; }
        }

        public LogRecord Enter
        {
            get { return (_calculableProperties["Enter"] as ICalculable<LogRecord>).Value; }
        }

        public LogRecord Exit
        {
            get { return (_calculableProperties["Exit"] as ICalculable<LogRecord>).Value; }
        }

        public override void Calculate()
        {
            base.Calculate();
//            Heals.Calculate();
//            Damage.Calculate();
//            Threat.Calculate();
//            DamageTaken.Calculate();
        }

        public override string GetLog()
        {
            var text = string.Join(Environment.NewLine, new[]
                {
                    string.Format("Combat {0:T} - {1:T}", Start, End),
                    string.Format("Duration: {0}", Duration.ToCombatLengthFormat()),
                    string.Format("Enter: {0}", Enter.Effect.Name),
                    string.Format("Exit: {0}", Exit.Effect.Name),
                    string.Format("Damage: {0}", Damage.Damage),
                    string.Format("Heals Given: {0}", Heals.HealsGiven),
                    string.Format("DPS: {0:0.##}", Damage.DPS),
                    string.Format("HPS: {0:0.##}", Heals.HPS)
                });
            return text;
        }

        protected override void InitProperties()
        {
            _calculableProperties.Add("Enter", new CalculableProperty<LogRecord>(() => Battle[0]));
            _calculableProperties.Add("Exit", new CalculableProperty<LogRecord>(() => Battle[Battle.Count - 1]));
            _calculableProperties.Add("Start", new CalculableProperty<DateTime>(() => Enter.When));
            _calculableProperties.Add("End", new CalculableProperty<DateTime>(() => Exit.When));
            _calculableProperties.Add("Duration", new CalculableProperty<TimeSpan>(() => End - Start));
        }
    }
}
