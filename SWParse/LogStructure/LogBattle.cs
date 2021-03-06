﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWParse.LogBattleVisitors;
using SWParse.LogStructure.StatisticsCalculation;

namespace SWParse.LogStructure
{
    internal class LogBattle : List<LogRecord>
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
            Statistics.Calculate();
            Parallel.ForEach(visitors, visitor => visitor.Apply(Statistics));
        }

        public override string ToString()
        {
            return string.Format("{0:T} - {1:T} ({2})", Statistics.Start, Statistics.End, Statistics.Duration.ToCombatLengthFormat());
        } 


    }
}
