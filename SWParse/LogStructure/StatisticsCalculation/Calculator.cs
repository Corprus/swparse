using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure.StatisticsCalculation
{
    internal abstract class Calculator
    {
        private LogBattle _battle;

        internal Calculator(LogBattle battle)
        {
            _battle = battle;
        }

        protected LogBattle Battle
        {
            get { return _battle; }
        }

        public abstract void Calculate();
        public abstract string GetLog();
    }
}
