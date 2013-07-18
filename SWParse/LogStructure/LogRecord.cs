using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure
{
    public struct LogRecord
    {
        public DateTime When;
        public LogPerson Source;
        public LogPerson Target;
        public LogSkill Skill;
        public LogEffect Effect;
        public LogQuantity Quantity;
        public int Threat;
        public bool Guarded;
        internal double HealThreatMultiplier;

        public LogRecord(List<string> record)
        {
            When = LogParser.ParseTime(record[0]);
            Source = new LogPerson(record[1]);
            Target = new LogPerson(record[2]);
            Skill = new LogSkill(record[3]);
            Effect = new LogEffect(record[4]);
            Quantity = new LogQuantity(record[5]);
            Threat = 0;
            Guarded = false;
            HealThreatMultiplier = 1;
            int.TryParse(record[6], out Threat);
        }
    }
}
