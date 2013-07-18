using System;
using SWParse.LogStructure;

namespace SWParse.LogBattleVisitors
{
    internal class HealVisitor:IBattleLogVisitor
    {
        public void Apply(LogBattle logBattle)
        {
            var text = string.Empty;

            text += string.Format(Environment.NewLine + Environment.NewLine + "Heals Given: {0}",logBattle. HealsGiven);
            text += string.Format(Environment.NewLine + "Overheal: {0}", logBattle.Overheal);
            text += string.Format(Environment.NewLine + "Normal Heal: {0}", logBattle.NormalHeals);
            text += string.Format(Environment.NewLine + "Crit Heal: {0}", logBattle.CritHeals);
            text += string.Format(Environment.NewLine + "Crit Heals %: {0:0.##}%", logBattle.CritHealsPercent * 100);
            text += string.Format(Environment.NewLine + "HPS: {0:0.##}", logBattle.HPS);
            text += string.Format(Environment.NewLine + "EHPS: {0:0.##}", logBattle.EHPS);
            text += string.Format(Environment.NewLine + "Effective healing%: {0:0.##}%", logBattle.EffectiveHealsPercent * 100);
            Summary = text;
        }

        public string Summary { get; private set; }
    }
}