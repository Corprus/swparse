using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure
{
    public enum LogEffectType
    {
        Event,
        ApplyEffect,
        RemoveEffect
    }

    public struct LogEffect
    {
        private const string EventString = "Event";
        private const string ApplyEffectString = "ApplyEffect";
        private const string RemoveEffectString = "RemoveEffect";
        public const string DamageString = "Damage";
        public const string HealString = "Heal";
        public const string EnterCombatString = "EnterCombat";
        public const string ExitCombatString = "ExitCombat";
        public const string DeathString = "Death";
        public const string GuardString = "Guard";

        public LogEffectType Type;
        public int? TypeId;
        public string Name;
        public int? EffectId;
        public LogEffect(string logString)
        {
            Type = LogEffectType.Event;
            TypeId = null;
            Name = string.Empty;
            EffectId = null;
            if (logString != string.Empty)
            {
                var parsedEffect = LogParser.ParseString(@"^(?<type>.*?) \{(?<typeId>\d*?)\}: (?<effect>.*?) \{(?<effectId>\d*?)\}$", logString);
                switch (parsedEffect[0])
                {
                    case ApplyEffectString : Type = LogEffectType.ApplyEffect; break;
                    case RemoveEffectString: Type = LogEffectType.RemoveEffect; break;
                    case EventString: Type = LogEffectType.Event; break;
                    default: Type = LogEffectType.Event; break;
                }
                int typeId;
                if (int.TryParse(parsedEffect[1], out typeId))
                {
                    TypeId = typeId;
                }
                Name = parsedEffect[2];
                int effectId;
                if (int.TryParse(parsedEffect[3], out effectId))
                {
                    EffectId = effectId;
                }
            }
        }
    }
}
