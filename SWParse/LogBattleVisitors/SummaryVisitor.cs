using System;
using SWParse.LogStructure.StatisticsCalculation;

namespace SWParse.LogBattleVisitors
{
    internal class SummaryVisitor:IBattleLogVisitor
    {
        public void Apply(BattleCalculator logBattle)
        {
            Summary = logBattle.GetLog();
        }

        public string Summary { get; private set; }
    }
}