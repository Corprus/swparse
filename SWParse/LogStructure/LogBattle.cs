using System;
using System.Collections.Generic;
using System.Linq;
using SWParse.LogBattleVisitors;

namespace SWParse.LogStructure
{
    internal partial class LogBattle : List<LogRecord>
    {
        public LogBattle(string logOwner)
        {
            LogOwner = logOwner;
        }

        private string LogOwner { get; set; }

        public DateTime Start
        {
            get { return Enter.When; }
        }

        public DateTime End
        {
            get { return Exit.When; }
        }

        public TimeSpan Duration
        {
            get { return End - Start; }
        }

        public LogRecord Enter
        {
            get { return this[0]; }
        }

        public LogRecord Exit
        {
            get { return this[Count - 1]; }
        }

        public void Visit(params IBattleLogVisitor[] visitors)
        {
            foreach (var battleLogVisitor in visitors)
            {
                battleLogVisitor.Apply(this);
            }
            
        }

        public override string ToString()
        {
            return string.Format("{0:T} - {1:T} ({2})", Start, End, Duration.ToCombatLengthFormat());
        } 


    }
}
