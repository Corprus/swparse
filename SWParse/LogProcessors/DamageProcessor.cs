using System;
using System.Collections.Generic;
using SWParse.LogStructure;

namespace SWParse.LogProcessors
{
    public class AttackEntry
    {
        public long TotalDamage;
        public int TotalHits;
        public int CriticalHits;
    }

    public class DamageProcessor:ProcessorBase
    {
        readonly IDictionary<string, AttackEntry> _attacks = new Dictionary<string, AttackEntry>();

        private long _totalDamage;
        private long _critDamage;

        public double Dps { get; private set; }

        public double TotalDamage
        {
            get { return _totalDamage; }
        }
        public double NormalDamage
        {
            get { return _totalDamage - _critDamage; }
        }

        public double CritDamagePercent
        {
            get { return (double)_totalDamage <= 0 ? _critDamage / (double)_totalDamage : 0; }
        }

        protected override void OnExitCombat(LogRecord record)
        {
            
        }

        protected override void OnDeath(LogRecord record)
        {
            
        }

        protected override void OnEnterCombat(LogRecord record)
        {
            ResetStats();
        }

        private void ResetStats()
        {
            _attacks.Clear();
        }

        protected override void ContinueProcessRecord(LogRecord record)
        {
            if (record.Source.Name == Owner && record.Effect.Name == LogEffect.DamageString)
            {
                RecordDamage(record);

                _totalDamage += record.Quantity.Value;
                if (record.Quantity.IsCrit)
                    _critDamage += record.Quantity.Value;
                Dps = _totalDamage/(DateTime.Now - CombatStart).TotalSeconds;
            }
        }

        private void RecordDamage(LogRecord record)
        {
            var attackName = record.Skill.Name;
            if (!_attacks.ContainsKey(attackName))
                _attacks.Add(attackName, new AttackEntry());

            var attackEntry = _attacks[attackName];
            if (record.Quantity.IsCrit)
                attackEntry.CriticalHits++;
            attackEntry.TotalHits++;
            attackEntry.TotalDamage += record.Quantity.Value;
        }
    }
}