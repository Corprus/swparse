using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure
{
    internal partial class LogBattle
    {
        private IEnumerable<LogRecord> ThreatRecords
        {
            get { return this.Where(rec => rec.Source.Name == LogOwner && rec.Threat != 0); }
        }

        public int ThreatActionsCount
        {
            get { return ThreatRecords.Count(); }
        }

        public long TotalThreat
        {
            get { return ThreatRecords.Sum(rec => rec.Threat); }
        }

        public Dictionary<string, int> ThreatBySkill
        {
            get
            {
                return (ThreatRecords.ToLookup(record => record.Skill.Name)).ToDictionary(records => records.Key,
                                                                                          records =>
                                                                                          records.Sum(
                                                                                              record => record.Threat));
            }
        }
    }
}
