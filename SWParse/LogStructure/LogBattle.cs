using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure
{
    class LogBattle: List<LogRecord>
    {
        public string LogOwner { get; set; }

        public DateTime Start
        {
            get { return this.Enter.When; }
        }
        public DateTime End
        {
            get { return this.Exit.When; }
        }
        public TimeSpan Duration
        {
            get { return this.End - this.Start; }
        }
        public LogRecord Enter
        {
            get { return this[0]; }
        }
        public LogRecord Exit
        {
            get { return this[this.Count - 1]; }
        }

        public long Damage
        {
            get { return this.Where(rec => rec.Source.Name == LogOwner && rec.Effect.Effect == LogEffect.DamageString).Sum(rec => rec.Quantity.Quantity); }
        }

        public long CritDamage
        {
            get
            {
                return this.Where(rec => rec.Source.Name == LogOwner
                                         && rec.Effect.Effect == LogEffect.DamageString
                                         && rec.Quantity.IsCrit)
                            .Sum(rec => rec.Quantity.Quantity);
            }
        }

        public long NormalDamage
        {
            get
            {
                return this.Where(rec => rec.Source.Name == LogOwner
                                         && rec.Effect.Effect == LogEffect.DamageString
                                         && !rec.Quantity.IsCrit)
                            .Sum(rec => rec.Quantity.Quantity);
            }
        }

        public double CritDamagePercent
        {
            get { return (double)Damage != 0 ? (double)CritDamage / (double)Damage : 0; }
        }

        public double DPS
        {
            get
            {
                return Damage / Duration.TotalSeconds;
            }
        }

        public long HealsGiven
        {
            get { return this.Where(rec => rec.Source.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString).Sum(rec => rec.Quantity.Quantity); }
        }

        public long NormalHeals
        {
            get { return this.Where(rec => rec.Source.Name == LogOwner 
                                           && rec.Effect.Effect == LogEffect.HealString 
                                           && !rec.Quantity.IsCrit)
                              .Sum(rec => rec.Quantity.Quantity); }
        }

        public long CritHeals
        {
            get
            {
                return this.Where(rec => rec.Source.Name == LogOwner
                                         && rec.Effect.Effect == LogEffect.HealString
                                         && rec.Quantity.IsCrit)
                            .Sum(rec => rec.Quantity.Quantity);
            }
        }

        public double CritHealsPercent
        {
            get { return (double)(HealsGiven != 0 ? (double)CritHeals / (double)HealsGiven : 0); }
        }

        public long HealsTaken
        {
            get { return this.Where(rec => rec.Target.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString).Sum(rec => rec.Quantity.Quantity); }
        }

        public double HPS
        {
            get { return HealsGiven / Duration.TotalSeconds; }
        }

        public long Overheal
        {
            get
            {
                return (long)this.Where(rec => rec.Source.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString)
                    .Sum(rec => rec.Quantity.Quantity - (rec.Threat * 2 / (rec.HealThreadMultiplier * (rec.Guarded ? 0.75 : 1))));
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
            get { return (double)(HealsGiven != 0 ? (double)EffectiveHeal / (double)HealsGiven : 0); }
        }


    }
}
