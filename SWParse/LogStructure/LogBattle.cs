using System;
using System.Collections.Generic;
using System.Linq;

namespace SWParse.LogStructure
{
    internal class LogBattle : List<LogRecord>
    {
        public string LogOwner { get; set; }

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
                        .Sum(rec => rec.Quantity.Quantity);
            }
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
                        .Sum(rec => rec.Quantity.Quantity);
            }
        }

        public long NormalHeals
        {
            get
            {
                return this.Where(rec => rec.Source.Name == LogOwner
                                         && rec.Effect.Effect == LogEffect.HealString
                                         && !rec.Quantity.IsCrit)
                           .Sum(rec => rec.Quantity.Quantity);
            }
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
            get { return HealsGiven != 0 ? (double) CritHeals/(double) HealsGiven : 0; }
        }

        public long HealsTaken
        {
            get
            {
                return
                    this.Where(rec => rec.Target.Name == LogOwner && rec.Effect.Effect == LogEffect.HealString)
                        .Sum(rec => rec.Quantity.Quantity);
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
                               .Sum(
                                   rec =>
                                   rec.Quantity.Quantity -
                                   (rec.Threat*2/(rec.HealThreadMultiplier*(rec.Guarded ? 0.75 : 1))));
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
            get { return HealsGiven != 0 ? (double) EffectiveHeal/(double) HealsGiven : 0; }
        }

        public override string ToString()
        {
            return string.Format("{0:T} - {1:T} ({2:g})", Start, End, Duration);
        }

        #region Reports Generation

        public string GetBattleLog()
        {
            var text = string.Format("Combat {0:T} - {1:T}, duration {2:g}, enter: {3}, exit: {4}", Start, End,
                                     Duration, Enter.Effect.Effect, Exit.Effect.Effect);
            text += string.Format(Environment.NewLine + "Damage: {0}", Damage);
            text += string.Format(Environment.NewLine + Environment.NewLine + "Heals Given: {0}", HealsGiven);
            text += string.Format(Environment.NewLine + "DPS: {0:0.##}", DPS);
            text += string.Format(Environment.NewLine + "HPS: {0:0.##}", HPS);
            return text;
        }

        public string GetHealLog()
        {
            var text = string.Empty;

            text += string.Format(Environment.NewLine + Environment.NewLine + "Heals Given: {0}", HealsGiven);
            text += string.Format(Environment.NewLine + "Overheal: {0}", Overheal);
            text += string.Format(Environment.NewLine + "Normal Heal: {0}", NormalHeals);
            text += string.Format(Environment.NewLine + "Crit Heal: {0}", CritHeals);
            text += string.Format(Environment.NewLine + "Crit Heals %: {0:0.##}%", CritHealsPercent * 100);
            text += string.Format(Environment.NewLine + "HPS: {0:0.##}", HPS);
            text += string.Format(Environment.NewLine + "EHPS: {0:0.##}", EHPS);
            text += string.Format(Environment.NewLine + "Effective healing%: {0:0.##}%", EffectiveHealsPercent * 100);
            return text;
        }

        public string GetDamageLog()
        {
            var text = string.Empty;
            text += string.Format(Environment.NewLine + "Damage: {0}", Damage);
            text += string.Format(Environment.NewLine + "DPS: {0:0.##}", DPS);
            text += string.Format(Environment.NewLine + "Crit Damage: {0}", CritDamage);
            text += string.Format(Environment.NewLine + "Crit Damage %: {0: 0.##}", CritDamagePercent * 100);
            return text;
        }

        #endregion
    }
}
