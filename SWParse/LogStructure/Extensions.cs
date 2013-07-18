using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWParse.LogStructure
{
    public static class Extensions
    {
        public static string ToCombatLengthFormat(this TimeSpan ts)
        {
            var sb = new StringBuilder();

            if ((int)ts.TotalHours > 0)
            {
                sb.Append((int)ts.TotalHours);
                sb.Append("h ");
            }
            if ((int) ts.TotalMinutes > 0)
            {
                sb.Append(ts.ToString("%m"));
                sb.Append("m ");
            }
            sb.Append(ts.ToString("ss\\.f"));
            sb.Append("s");
            return sb.ToString();
        }
    }
}
