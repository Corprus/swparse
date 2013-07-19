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
            InitProperties();
        }

        protected LogBattle Battle
        {
            get { return _battle; }
        }

        protected abstract void InitProperties();

        protected Dictionary<string, ICalculable> _calculableProperties = new Dictionary<string, ICalculable>();

        public virtual void Calculate()
        {
            foreach (var property in _calculableProperties.Values)
            {
                property.Calculate();
            }
        }

        public abstract string GetLog();
    }
}
