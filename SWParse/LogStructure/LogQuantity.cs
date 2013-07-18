using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure
{
    public struct LogQuantity
    {
        public int Value;
        public string Type;
        public bool IsCrit;
        public long? Id;
        public LogQuantity(string logString)
        {
            const char CritSymbol = '*';
            Value = 0;
            IsCrit = false;
            Type = string.Empty;
            Id = null;
            if (!string.IsNullOrEmpty(logString))
            {
                if (int.TryParse(logString, out Value))
                {
                    return;
                }
                if (logString.EndsWith(CritSymbol.ToString()) && int.TryParse(logString.TrimEnd(CritSymbol), out Value))
                {
                    IsCrit = true;
                    return;
                }

                var parsedQuantity = LogParser.ParseString(@"^(?<quantity>.*?) (.*?) \{(?<id1>\d*?)\}$", logString);
                if (parsedQuantity.Count >= 3)
                {
                    IsCrit = parsedQuantity[1].EndsWith(CritSymbol.ToString());
                    int.TryParse(parsedQuantity[1].TrimEnd(CritSymbol), out Value);
                    Type = parsedQuantity[0];
                    long id;
                    if (long.TryParse(parsedQuantity[2], out id))
                    {
                        Id = id;
                    }
                }
            }
        }
    }
}
