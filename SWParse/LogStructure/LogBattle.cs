using System;
using System.Collections.Generic;
using System.Linq;
using SWParse.LogBattleVisitors;

namespace SWParse.LogStructure
{
    internal class LogBattle : List<LogRecord>
    {
        public LogBattle(string logOwner)
        {
            LogOwner = logOwner;
        }

        private string LogOwner { get; set; }

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
            get { return this[0]; }
        }

        public LogRecord Exit
        {
            get { return this[Count - 1]; }
        }

        public long Damage
        {
            get
            {
                return
                    this.Where(rec => rec.Source.Name == LogOwner && rec.Effect.Effect == LogEffect.DamageString)
                        .Sum(rec => rec.Quantity.Value);
            }
        }

        public long CritDamage
        {
            get
            {
                return this.Where(rec => rec.Source.Name == LogOwner
                                         && rec.Effect.Effect == LogEffect.DamageString
                                         && rec.Quantity.IsCrit)
                           .Sum(rec => rec.Quantity.Value);
            }
        }

        public long NormalDamage
        {
            get
            {
                return this.Where(rec => rec.Source.Name == LogOwner
                                         && rec.Effect.Effect == LogEffect.DamageString
                                         && !rec.Quantity.IsCrit)
                           .Sum(rec => rec.Quantity.Value);
            }
        }

        public double CritDamagePercent
        {
            get { return (double) Damage <= 0 ? (double) CritDamage/(double) Damage : 0; }
        }

        public double DPS
        {
            get { return Damage/Duration.TotalSeconds; }
        }

        public long HealsGiven
        {
            get
            {
                return
                    this.Where(rec => rec.Source.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString)
                        .Sum(rec => rec.Quantity.Value);
            }
        }

        public long NormalHeals
        {
            get
            {
                return this.Where(rec => rec.Source.Name == LogOwner
                                         && rec.Effect.Effect == LogEffect.HealString
                                         && !rec.Quantity.IsCrit)
                           .Sum(rec => rec.Quantity.Value);
            }
        }

        public long CritHeals
        {
            get
            {
                return this.Where(rec => rec.Source.Name == LogOwner
                                         && rec.Effect.Effect == LogEffect.HealString
                                         && rec.Quantity.IsCrit)
                           .Sum(rec => rec.Quantity.Value);
            }
        }

        public double CritHealsPercent
        {
            get { return HealsGiven != 0 ? (double) CritHeals/HealsGiven : 0; }
        }

        public long HealsTaken
        {
            get
            {
                return
                    this.Where(rec => rec.Target.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString)
                        .Sum(rec => rec.Quantity.Value);
            }
        }

        public double HPS
        {
            get { return HealsGiven/Duration.TotalSeconds; }
        }

        public long Overheal
        {
            get
            {
                return
                    (long) this.Where(rec => rec.Source.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString)
                               .Sum(rec => rec.Quantity.Value - (rec.Threat*2/(rec.HealThreadMultiplier*(rec.Guarded ? 0.75 : 1))));
            }
        }

        public long EffectiveHeal
        {
            get { return HealsGiven - Overheal; }
        }

        public double EHPS
        {
            get { return (HealsGiven - Overheal)/Duration.TotalSeconds; }
        }

        public double EffectiveHealsPercent
        {
            get { return HealsGiven != 0 ? (double) EffectiveHeal/HealsGiven : 0; }
        }

        public override string ToString()
        {
            return string.Format("{0:T} - {1:T} ({2:g})", Start, End, Duration);
        }

        public void Visit(params IBattleLogVisitor[] visitors)
        {
            foreach (var battleLogVisitor in visitors)
            {
                battleLogVisitor.Apply(this);
            }
            
        }
    }
}
