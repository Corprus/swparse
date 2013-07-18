using System;
using SWParse.LogStructure;

namespace SWParse.LogProcessors
{
    public interface IBaseProcessor
    {
        void ProcessEntry(LogRecord record);
    }


    public abstract class ProcessorBase:IBaseProcessor
    {
        protected string Owner = null;
        protected DateTime CombatStart = DateTime.MinValue;
        protected DateTime CombatEnd = DateTime.MaxValue;

        public void ProcessEntry(LogRecord record)
        {
            if (Owner == null)
            {
                if (record.Source.IsPlayer)
                {
                    Owner = record.Source.Name;
                }
                else if (record.Target.IsPlayer)
                {
                    Owner = record.Target.Name;
                }
                else
                {
                    throw new InvalidOperationException("Can't define owner by Source or Target");
                }
            }

            switch (record.Effect.Name)
            {
                case LogEffect.EnterCombatString:
                    CombatStart = DateTime.Now;
                    OnEnterCombat(record);
                    break;
                case LogEffect.DeathString:
                    CombatEnd = DateTime.Now;
                    OnDeath(record);
                    break;
                case LogEffect.ExitCombatString:
                    CombatEnd = DateTime.Now;
                    OnExitCombat(record);
                    break;
            }

            ContinueProcessRecord(record);
        }

        protected abstract void OnExitCombat(LogRecord record);

        protected abstract void OnDeath(LogRecord record);

        protected abstract void OnEnterCombat(LogRecord record);

        protected abstract void ContinueProcessRecord(LogRecord record);
    }
}