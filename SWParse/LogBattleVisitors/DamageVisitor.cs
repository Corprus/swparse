using System;
using SWParse.LogStructure;

namespace SWParse.LogBattleVisitors
{
    internal class DamageVisitor:IBattleLogVisitor
    {
        public void Apply(LogBattle logBattle)
        {
            var text = string.Empty;
            text += string.Format(Environment.NewLine + "Damage: {0}", logBattle.Damage);
            text += string.Format(Environment.NewLine + "DPS: {0:0.##}", logBattle.DPS);
            text += string.Format(Environment.NewLine + "Crit Damage: {0}", logBattle.CritDamage);
            text += string.Format(Environment.NewLine + "Crit Damage %: {0: 0.##}", logBattle.CritDamagePercent * 100);
            Summary = text;
        }

        public string Summary { get; private set; }
    }
}