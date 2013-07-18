using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure
{
    public struct LogQuantity
    {
        public int Quantity;
        public string Type;
        public bool IsCrit;
        public long? Id;
        public LogQuantity(string logString)
        {
            const char CritSymbol = '*';
            Quantity = 0;
            IsCrit = false;
            Type = string.Empty;
            Id = null;
            if (!string.IsNullOrEmpty(logString))
            {
                if (int.TryParse(logString, out Quantity))
                {
                    return;
                }
                if (logString.EndsWith(CritSymbol.ToString()) && int.TryParse(logString.TrimEnd(CritSymbol), out Quantity))
                {
                    IsCrit = true;
                    return;
                }

                var parsedQuantity = LogParser.ParseString(@"^(?<quantity>.*?) (.*?) \{(?<id1>\d*?)\}$", logString);
                if (parsedQuantity.Count >= 3)
                {
                    IsCrit = parsedQuantity[1].EndsWith(CritSymbol.ToString());
                    int.TryParse(parsedQuantity[1].TrimEnd(CritSymbol), out Quantity);
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
