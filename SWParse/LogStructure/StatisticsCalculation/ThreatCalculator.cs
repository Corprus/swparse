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
            get { return (_calculableProperties["ThreatRecords"] as ICalculable<IEnumerable<LogRecord>>).Value; }
        }

        public IEnumerable<IGrouping<string, LogRecord>> ThreatRecordsBySkill
        {
            get { return (_calculableProperties["ThreatRecordsBySkill"] as ICalculable<IEnumerable<IGrouping<string, LogRecord>>>).Value; }
        }

        public int ThreatActionsCount
        {
            get { return (_calculableProperties["ThreatActionsCount"] as ICalculable<int>).Value; }
        }

        public long TotalThreat
        {
            get { return (_calculableProperties["TotalThreat"] as ICalculable<long>).Value; }
        }

        public Dictionary<string, int> ThreatBySkill
        {
            get { return (_calculableProperties["ThreatBySkill"] as ICalculable<Dictionary<string, int>>).Value; }
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

        public override string GetLog()
        {
            var text = string.Join(Environment.NewLine, new string[]
                {
                    string.Format("Threat actions: {0}", ThreatActionsCount),
                    string.Format("TotalThreat: {0}", TotalThreat),
                });

            return text;
        }

        protected override void InitProperties()
        {
            _calculableProperties.Add("ThreatRecords", 
                new CalculableProperty<IEnumerable<LogRecord>>(() => Battle.Where(rec => rec.Source.Name == Battle.LogOwner && rec.Threat != 0)));
            _calculableProperties.Add("ThreatRecordsBySkill", 
                new CalculableProperty<IEnumerable<IGrouping<string, LogRecord>>>(() => ThreatRecords.ToLookup(record => record.Skill.Name)));
            _calculableProperties.Add("ThreatActionsCount", new CalculableProperty<int>(() => ThreatRecords.Count()));
            _calculableProperties.Add("TotalThreat", new CalculableProperty<long>(() => ThreatRecords.Sum(rec => rec.Threat)));
            _calculableProperties.Add("ThreatBySkill", 
                new CalculableProperty<Dictionary<string, int>>(() => ThreatRecordsBySkill.ToDictionary(records => records.Key, records => records.Sum(record => record.Threat))));
        }
    }
}
