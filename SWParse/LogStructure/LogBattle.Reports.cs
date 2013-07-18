using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SWParse.LogStructure
{
    internal partial class LogBattle
    {
        #region Reports Generation

        public string GetBattleLog()
        {
            var text = string.Join(Environment.NewLine, new[]
                {
                    string.Format("Combat {0:T} - {1:T}", Start, End),
                    string.Format("Duration: {0}", Duration.ToCombatLengthFormat()),
                    string.Format("Enter: {0}", Enter.Effect.Name),
                    string.Format("Exit: {0}", Exit.Effect.Name),
                    string.Format("Damage: {0}", Damage),
                    string.Format("Heals Given: {0}", HealsGiven),
                    string.Format("DPS: {0:0.##}", DPS),
                    string.Format("HPS: {0:0.##}", HPS)
                });
            return text;
        }

        public string GetHealLog()
        {
            var text = string.Join(Environment.NewLine, new[]
                {
                    string.Format("Heals: {0}", HealsGivenCount),
                    string.Format("Heals Given: {0}", HealsGiven),
                    string.Format("Overheal: {0}", Overheal),
                    string.Format("Normal Heal: {0}", NormalHeals),
                    string.Format("Crit Heal Count: {0}", CritHealsGivenCount),
                    string.Format("Crit Heals: {0}", CritHeals),
                    string.Format("Crit Heals %: {0:0.##}%", CritHealsPercent*100),
                    string.Format("HPS: {0:0.##}", HPS),
                    string.Format("EHPS: {0:0.##}", EHPS),
                    string.Format("Effective healing%: {0:0.##}%", EffectiveHealsPercent*100)
                });

            return text;
        }

        public string GetDamageLog()
        {
            var text = string.Join(Environment.NewLine, new[]
                {
                    string.Format("Hits: {0}", HitsDealt),
                    string.Format("Damage: {0}", Damage),
                    string.Format("DPS: {0:0.##}", DPS),
                    string.Format("Crit Damage: {0}", CritDamage),
                    string.Format("Crit Hits: {0}", CritHitsDealt),
                    string.Format("Crit Hits %: {0: 0.##}", CritHitsPercent*100)
                });

            return text;
        }

        public string GetDamageTakenLog()
        {
            var text = string.Join(Environment.NewLine, new[]
                {
                    string.Format("Hits: {0}", HitsTaken),
                    string.Format("Damage: {0}", DamageTaken),
                    string.Format("DPS: {0:0.##}", DPSTaken),
                    string.Format("Crit Damage: {0}", CritDamageTaken),
                    string.Format("Crit hits: {0}", CritTakenHits),
                    string.Format("Crit Damage %: {0: 0.##}", CritHitsTakenPercent*100)
                });

            return text;
        }

        public string GetThreatLog()
        {
            var text = string.Join(Environment.NewLine, new string[]
                {
                    string.Format("Threat actions: {0}", ThreatActionsCount),
                    string.Format("TotalThreat: {0}", TotalThreat),
                });

            return text;
        }

        public Table GetThreatTable()
        {
            var threatTable = new Table();
            threatTable.CellSpacing = 0;
            threatTable.Columns.Add(new TableColumn());
            threatTable.Columns.Add(new TableColumn());
            threatTable.Columns.Add(new TableColumn());
            threatTable.Columns.Add(new TableColumn());
            var headerGroup = new TableRowGroup();
            headerGroup.Rows.Add(new TableRow());
            headerGroup.Rows[0].FontWeight = FontWeights.Bold;
            var headerRow = headerGroup.Rows[0];
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Skill"))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Count"))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Threat"))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Threat %"))));
            threatTable.RowGroups.Add(headerGroup);

            var rowGroup = new TableRowGroup();
            var totalThreat = TotalThreat;
            foreach (var item in ThreatRecordsBySkill.OrderByDescending(records => records.Sum(record => record.Threat)))
            {
                var row = new TableRow();

                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Key))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Count().ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Sum(record => record.Threat).ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Sum(record => (double) record.Threat / totalThreat).ToString("0.##%")))));
                rowGroup.Rows.Add(row);
            }
            rowGroup.Rows.Add(new TableRow());
            threatTable.RowGroups.Add(rowGroup);
            return threatTable;
        }

        #endregion

    }
}