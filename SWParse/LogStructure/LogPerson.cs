using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure
{
    public struct LogPerson
    {
        public string Name;
        public bool IsPlayer;
        public long? Id1;
        public long? Id2;
        public LogPerson(string logString)
        {
            Name = logString;
            IsPlayer = false;
            Id1 = null;
            Id2 = null;
            if (logString.Length > 0)
            {                
                if (logString[0] == '@')
                {
                    IsPlayer = true;
                    Name = logString.Substring(1);
                    return;
                }
                var parsedName = LogParser.ParseString(@"^(?<name>.*?) \{(?<id1>\d*?)\}:(?<id2>\d*?)$", logString);
                Name = parsedName[0];
                IsPlayer = false;
                long id1;
                if (long.TryParse(parsedName[1], out id1))
                {
                    Id1 = id1;
                }
                long id2;
                if (long.TryParse(parsedName[1], out id2))
                {
                    Id2 = id2;
                }
            }
        }
    }
}
