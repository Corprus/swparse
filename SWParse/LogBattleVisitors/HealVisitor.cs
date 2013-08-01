using System;
using System.Windows.Documents;
using SWParse.LogStructure;
using SWParse.LogStructure.StatisticsCalculation;

namespace SWParse.LogBattleVisitors
{
    internal class HealVisitor:IBattleLogVisitor
    {
        private BattleCalculator _logBattle;

        public void Apply(BattleCalculator logBattle)
        {
            _logBattle = logBattle;
            Summary = logBattle.Heals.GetLog();
        }

        public string Summary { get; private set; }

        public Table Table
        {
            get { return _logBattle.Heals.GetHealTable(); }
        }
    }
}