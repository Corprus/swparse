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
            get { return Enter.When; }
        }

        public DateTime End
        {
            get { return Exit.When; }
        }

        public TimeSpan Duration
        {
            get { return End - Start; }
        }

        public LogRecord Enter
        {
            get { return Battle[0]; }
        }

        public LogRecord Exit
        {
            get { return Battle[Battle.Count - 1]; }
        }


        public override void Calculate()
        {
#warning should be implemented to prevent overcalculating;
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
    }
}
