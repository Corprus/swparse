using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SWParse.LogStructure;
using System.Diagnostics;

namespace SWParse
{
    class LogParser
    {
        public static List<LogRecord> Parse(string logPath)
        {
            string[] log  = File.ReadAllLines(logPath);
            List<LogRecord> logResult = new List<LogRecord> ();
            foreach (var logString in log)
            {
                var parseResult = ParseString(@"^\[(?<date>.*?)\] \[(?<from>.*?)\] \[(?<to>.*?)\] \[(?<skill>.*?)\] \[(?<effect>.*?)\] \((?<quantity>.*?)\)\D*(?<threat>\d+)?.*$", logString);
                logResult.Add(new LogRecord(parseResult));
            }
            return logResult;
        }        

        public static List<LogBattle> DivideIntoBattlesAndApplyGuards(List<LogRecord> log)
        {
            string logOwner = log.Find(rec => rec.Effect.Type == LogEffectType.Event).Source.Name;
            List<LogBattle> battles = new List<LogBattle>();
            LogBattle currentBattle = null;
            bool guarded = false;
            double healMultiplier = log
                .Where(rec => rec.Source.Name == logOwner && rec.Effect.Effect == LogEffect.HealString)
                .Max(rec => { 
                    var d = ((double)rec.Threat * 2 / (guarded ? 0.75 : 1)) / (double)rec.Quantity.Quantity;
                    if (d > 0.95)
                    {
                        Debug.Write(rec.Threat); Debug.WriteLine(rec.Skill.Name); Debug.WriteLine(rec.Quantity.Quantity);
                    }
                    return d < 0.95 ? d : 0; });
            if (healMultiplier == 0) healMultiplier = 1;
            for (int i=0; i < log.Count; i++)            
            {
                var record = log[i];
                switch (record.Effect.Effect)
                {
                    case LogEffect.GuardString:
                        {
                            if (record.Effect.Type == LogEffectType.ApplyEffect)
                            {
                                guarded = true;
                            }
                            if (record.Effect.Type == LogEffectType.RemoveEffect)
                            {
                                guarded = false;
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
                        currentBattle = new LogBattle();                        
                        currentBattle.LogOwner = logOwner;
                        currentBattle.Add(record);
                        break;
                    default:
                        if (currentBattle != null)
                        {
                            record.Guarded = guarded;
                            record.HealThreadMultiplier = healMultiplier;
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
