using SWParse.LogStructure.StatisticsCalculation;

namespace SWParse.LogBattleVisitors
{
    internal class DamageVisitor:IBattleLogVisitor
    {
        public void Apply(BattleCalculator logBattle)
        {
            Summary = logBattle.Damage.GetLog();
        }

        public string Summary { get; private set; }
        
    }
}