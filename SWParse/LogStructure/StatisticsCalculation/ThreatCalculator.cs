using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SWParse.LogStructure.StatisticsCalculation
{
    internal class ThreatCalculator : Calculator
    {
        public ThreatCalculator(LogBattle battle)
            : base(battle)
        {
            
        }

        private IEnumerable<LogRecord> ThreatRecords
        {
            get { return Battle.Where(rec => rec.Source.Name == Battle.LogOwner && rec.Threat != 0); }
        }

        public IEnumerable<IGrouping<string, LogRecord>> ThreatRecordsBySkill
        {
            get { return ThreatRecords.ToLookup(record => record.Skill.Name); }
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
                return ThreatRecordsBySkill.ToDictionary(records => records.Key, records => records.Sum(record => record.Threat));
            }
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
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Sum(record => (double)record.Threat / totalThreat).ToString("0.##%")))));
                rowGroup.Rows.Add(row);
            }
            rowGroup.Rows.Add(new TableRow());
            threatTable.RowGroups.Add(rowGroup);
            return threatTable;
        }


        public override void Calculate()
        {
#warning should be implemented to prevent overcalculating;
        }

        public override string GetLog()
        {
            var text = string.Join(Environment.NewLine, new string[]
                {
                    string.Format("Threat actions: {0}", ThreatActionsCount),
                    string.Format("TotalThreat: {0}", TotalThreat),
                });

            return text;
        }
    }
}
