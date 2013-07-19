using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SWParse.LogStructure.StatisticsCalculation
{
    internal class HealCalculator : Calculator
    {
        public HealCalculator(LogBattle battle)
            :base(battle)
        {            
        }

        private IEnumerable<LogRecord> HealsGivenRecords
        {
            get { return (_calculableProperties["HealsGivenRecords"] as ICalculable<IEnumerable<LogRecord>>).Value; }
        }

        private IEnumerable<LogRecord> CritHealsGivenRecords
        {
            get { return (_calculableProperties["CritHealsGivenRecords"] as ICalculable<IEnumerable<LogRecord>>).Value; }
        }

        private IEnumerable<IGrouping<string, LogRecord>> HealRecordsBySkill
        {
            get { return (_calculableProperties["HealRecordsBySkill"] as ICalculable<IEnumerable<IGrouping<string, LogRecord>>>).Value; }
        }

        public int HealsGivenCount
        {
            get { return (_calculableProperties["HealsGivenCount"] as ICalculable<int>).Value; }
        }

        public int CritHealsGivenCount
        {
            get { return (_calculableProperties["CritHealsGivenCount"] as ICalculable<int>).Value; }
        }

        public double CritHealsPercent
        {
            get { return (_calculableProperties["CritHealsPercent"] as ICalculable<double>).Value; }
        }

        public int HealsTaken
        {
            get { return (_calculableProperties["HealsTaken"] as ICalculable<int>).Value; }
        }

        public double HPS
        {
            get { return (_calculableProperties["HPS"] as ICalculable<double>).Value; }
        }

        public int Overheal
        {
            get { return (_calculableProperties["Overheal"] as ICalculable<int>).Value; }
        }

        public int EffectiveHeal
        {
            get { return (_calculableProperties["EffectiveHeal"] as ICalculable<int>).Value; }
        }

        public double EHPS
        {
            get { return (_calculableProperties["EHPS"] as ICalculable<double>).Value; }
        }

        public double EffectiveHealsPercent
        {
            get { return (_calculableProperties["EffectiveHealsPercent"] as ICalculable<double>).Value; }
        }

        public int HealsGiven
        {
            get { return (_calculableProperties["HealsGiven"] as ICalculable<int>).Value; }
        }

        public int NormalHeals
        {
            get { return (_calculableProperties["NormalHeals"] as ICalculable<int>).Value; }
        }

        public int CritHeals
        {
            get { return (_calculableProperties["CritHeals"] as ICalculable<int>).Value; }
        }

        public override string GetLog()
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

        public Table GetHealTable()
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
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Heal"))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Total Heal %"))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Overheal"))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Overheal %"))));
            threatTable.RowGroups.Add(headerGroup);

            var rowGroup = new TableRowGroup();
            var totalHeal = HealsGiven;
            var totalOverheal = Overheal;
            foreach (var item in HealRecordsBySkill.OrderByDescending(records => records.Sum(record => record.CalculateOverheal())))
            {
                var row = new TableRow();

                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Key))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Count().ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Sum(record => record.Quantity.Value).ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Sum(record => (double)record.Quantity.Value / totalHeal).ToString("0.##%")))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Sum(record => record.CalculateOverheal()).ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(
                    (item.Sum(record => (double)record.CalculateOverheal())/item.Sum(record => record.Quantity.Value)).ToString("0.##%")))));
                rowGroup.Rows.Add(row);
            }
            rowGroup.Rows.Add(new TableRow());
            threatTable.RowGroups.Add(rowGroup);
            return threatTable;
        }


        protected override void InitProperties()
        {
            _calculableProperties.Add("HealsGivenRecords",
                new CalculableProperty<IEnumerable<LogRecord>>(() => 
                    Battle.Where(rec => rec.Source.Name == Battle.LogOwner && rec.Effect.Name == LogEffect.HealString).ToList()));
            _calculableProperties.Add("CritHealsGivenRecords",
                new CalculableProperty<IEnumerable<LogRecord>>(() => HealsGivenRecords.Where(rec => rec.Quantity.IsCrit)));
            _calculableProperties.Add("HealsGivenCount", new CalculableProperty<int>(() => HealsGivenRecords.Count()));
            _calculableProperties.Add("CritHealsGivenCount", new CalculableProperty<int>(() => CritHealsGivenRecords.Count()));
            _calculableProperties.Add("CritHealsPercent", 
                new CalculableProperty<double>(() => HealsGivenCount > 0 ? (double)CritHealsGivenCount / (double)HealsGivenCount : 0));
            _calculableProperties.Add("HealsTaken",
                                      new CalculableProperty<int>(
                                          () =>
                                          Battle.Where(
                                              rec =>
                                              rec.Target.Name == Battle.LogOwner &&
                                              rec.Effect.Name == LogEffect.HealString)
                                                .Sum(rec => rec.Quantity.Value)));

            _calculableProperties.Add("HPS", new CalculableProperty<double>(() => HealsGiven / Battle.Statistics.Duration.TotalSeconds));
            _calculableProperties.Add("Overheal",
                new CalculableProperty<int>(() => HealsGivenRecords.Sum(rec => rec.CalculateOverheal())));
            _calculableProperties.Add("EffectiveHeal", new CalculableProperty<int>(() => HealsGiven - Overheal));
            _calculableProperties.Add("EHPS", 
                new CalculableProperty<double>(() => (HealsGiven - Overheal) / Battle.Statistics.Duration.TotalSeconds));
            _calculableProperties.Add("EffectiveHealsPercent", 
                new CalculableProperty<double>(() => HealsGiven != 0 ? (double)EffectiveHeal / (double)HealsGiven : 0));
            _calculableProperties.Add("HealsGiven", 
                new CalculableProperty<int>(() => HealsGivenRecords.Sum(rec => rec.Quantity.Value)));
            _calculableProperties.Add("NormalHeals", 
                new CalculableProperty<int>(() => HealsGivenRecords.Where(rec => !rec.Quantity.IsCrit).Sum(rec => rec.Quantity.Value)));
            _calculableProperties.Add("CritHeals", 
                new CalculableProperty<int>(() => CritHealsGivenRecords.Sum(rec => rec.Quantity.Value)));
            _calculableProperties.Add("HealRecordsBySkill",
                new CalculableProperty<IEnumerable<IGrouping<string, LogRecord>>>(() => HealsGivenRecords.ToLookup(record => record.Skill.Name)));
        }
    }
}
