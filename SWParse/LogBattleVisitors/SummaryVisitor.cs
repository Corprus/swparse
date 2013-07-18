using System;
using SWParse.LogStructure;

namespace SWParse.LogBattleVisitors
{
    internal class SummaryVisitor:IBattleLogVisitor
    {
        public void Apply(LogBattle logBattle)
        {
            var text = string.Format("Combat {0:T} - {1:T}, duration {2:g}, enter: {3}, exit: {4}", logBattle.Start, logBattle.End,
                                     logBattle.Duration,logBattle.Enter.When,logBattle.Exit.When);
            text += string.Format(Environment.NewLine + "Damage: {0}", logBattle.Damage);
            text += string.Format(Environment.NewLine + Environment.NewLine + "Heals Given: {0}", logBattle.HealsGiven);
            text += string.Format(Environment.NewLine + "DPS: {0:0.##}", logBattle.DPS);
            text += string.Format(Environment.NewLine + "HPS: {0:0.##}", logBattle.HPS);
            Summary = text;
        }

        public string Summary { get; private set; }
    }
}