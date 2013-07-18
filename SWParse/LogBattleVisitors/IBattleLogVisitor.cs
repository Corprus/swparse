using SWParse.LogStructure;

namespace SWParse.LogBattleVisitors
{
    internal interface IBattleLogVisitor
    {
        void Apply(LogBattle logBattle);
    }
}