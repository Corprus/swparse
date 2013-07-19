using System;
using SWParse.LogStructure.StatisticsCalculation;

namespace SWParse.LogBattleVisitors
{
    internal class HealVisitor:IBattleLogVisitor
    {
        public void Apply(BattleCalculator logBattle)
        {
            Summary = logBattle.Heals.GetLog();
        }

        public string Summary { get; private set; }
    }
}