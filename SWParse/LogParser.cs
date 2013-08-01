using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SWParse.LogStructure;

namespace SWParse
{
    class LogParser
    {
        public static List<LogRecord> Parse(string logPath)
        {
            string[] log  = File.ReadAllLines(logPath);
            return log.AsParallel().AsOrdered().Select(logString => ParseString(@"^\[(?<date>.*?)\] \[(?<from>.*?)\] \[(?<to>.*?)\] \[(?<skill>.*?)\] \[(?<effect>.*?)\] \((?<quantity>.*?)\)\D*(?<threat>\d+)?.*$", logString)).Select(parseResult => new LogRecord(parseResult)).ToList();
        }        

        public static List<LogBattle> DivideIntoBattlesAndApplyGuards(List<LogRecord> log)
        {
            string logOwner = log.First(rec => rec.Effect.Type == LogEffectType.Event).Source.Name;
            var battles = new List<LogBattle>();
            LogBattle currentBattle = null;
            bool guarded = false;
            IEnumerable<LogRecord> heals = log.Where(rec => rec.Source.Name == logOwner && rec.Effect.Name == LogEffect.HealString);
            var healRecords = heals as IList<LogRecord> ?? heals.ToList();
            double healMultiplier = healRecords.Any()
                                        ? healRecords.Max(rec =>
                                            {
                                                var d = ((double) rec.Threat*2/(rec.Guarded ? 0.75 : 1))/
                                                        rec.Quantity.Value;
                                                return d < 0.95 ? d : 0;
                                            })
                                        : 1;
            if (healMultiplier == 0) healMultiplier = 1;

            foreach (var record in log)
            {
                switch (record.Effect.Name)
                {
                    case LogEffect.GuardString:
                        {
                            switch (record.Effect.Type)
                            {
                                case LogEffectType.ApplyEffect:
                                    guarded = true;
                                    break;
                                case LogEffectType.RemoveEffect:
                                    guarded = false;
                                    break;
                            }
                        }
                        break;
                    case LogEffect.ExitCombatString:
                        if (currentBattle != null)
                        {
                            currentBattle.Add(record);
                            battles.Add(currentBattle);
                            currentBattle = null;
                        }
                        break;
                    case LogEffect.DeathString:
                        if (currentBattle != null)
                        {
                            currentBattle.Add(record);
                            if (record.Target.Name == logOwner)
                            {
                                battles.Add(currentBattle);
                                currentBattle = null;
                                guarded = false;
                            }
                        }
                        break;
                    case LogEffect.EnterCombatString:
                        currentBattle = new LogBattle(logOwner) {record};
                        break;
                    default:
                        if (currentBattle != null)
                        {
                            record.Guarded = guarded;
                            record.HealThreatMultiplier = healMultiplier;
                            currentBattle.Add(record);
                        }
                        break;
                }
            }
            return battles;
        }

        public static DateTime ParseTime(string swtorTime)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(swtorTime, "HH:mm:ss.fff", provider);
        }

        public static List<string> ParseString(string template, string str)
        {            

            Regex r = new Regex(template);
            Match m = r.Match(str);

            List<string> ret = new List<string>();

            for (int i = 1; i < m.Groups.Count; i++)
            {
                ret.Add(m.Groups[i].Value);
            }

            return ret;
        }
    }
}
