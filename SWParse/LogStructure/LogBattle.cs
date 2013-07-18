using System;
using System.Collections.Generic;
using System.Linq;
using SWParse.LogBattleVisitors;
using SWParse.LogStructure.StatisticsCalculation;

namespace SWParse.LogStructure
{
    internal partial class LogBattle : List<LogRecord>
    {
        public LogBattle(string logOwner)
        {
            LogOwner = logOwner;
            Statistics = new BattleCalculator(this);
        }

        internal string LogOwner { get; set; }

        public BattleCalculator Statistics { get; private set; }


        public void Visit(params IBattleLogVisitor[] visitors)
        {
            foreach (var battleLogVisitor in visitors)
            {
                battleLogVisitor.Apply(Statistics);
            }
            
        }

        public override string ToString()
        {
            return string.Format("{0:T} - {1:T} ({2})", Statistics.Start, Statistics.End, Statistics.Duration.ToCombatLengthFormat());
        } 


    }
}
