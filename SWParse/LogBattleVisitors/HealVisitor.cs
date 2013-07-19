using System;
using System.Windows.Documents;
using SWParse.LogStructure.StatisticsCalculation;

namespace SWParse.LogBattleVisitors
{
    internal class HealVisitor:IBattleLogVisitor
    {
        public void Apply(BattleCalculator logBattle)
        {
            Summary = logBattle.Heals.GetLog();
            Table = logBattle.Heals.GetHealTable();
        }

        public string Summary { get; private set; }

        public Table Table { get; private set; }
    }
}