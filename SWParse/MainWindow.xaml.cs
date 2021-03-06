﻿using System.Collections.Generic;
using System.IO;

using Microsoft.Win32;
using System.Windows;
using System.Windows.Documents;
using SWParse.LogBattleVisitors;
using SWParse.LogStructure;

namespace SWParse
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<LogBattle> _battles;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            const string initialDirectory = "g:\\temp\\";

            if (Directory.Exists(initialDirectory))
                openFileDialog1.InitialDirectory = initialDirectory;

            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                var result = LogParser.Parse(openFileDialog1.FileName);
                _battles = LogParser.DivideIntoBattlesAndApplyGuards(result);
                ListBattles.Items.Clear();


                ListBattles.BeginInit();
                foreach (var battle in _battles)
                {
                    ListBattles.Items.Add(battle.ToString());
                }
                ListBattles.EndInit();
            }
        }

        private void ListBattles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && _battles.Count > ListBattles.Items.IndexOf(e.AddedItems[0]))
            {
                ShowBattleLog(_battles[ListBattles.Items.IndexOf(e.AddedItems[0])]);
            }
            else
            {
                ShowBattleLog(null);
            }
        }

        private void ShowBattleLog(LogBattle battle)
        {
            if (battle == null)
            {
                LogText.Document = new FlowDocument();
                LogText.IsEnabled = false;
                return;
            }
            
            SummaryVisitor summaryVisitor = new SummaryVisitor();
            HealVisitor healVisitor = new HealVisitor();
            DamageVisitor damageVisitor = new DamageVisitor();

            battle.Visit(summaryVisitor,healVisitor,damageVisitor);

            LogText.IsEnabled = true;
            LogText.Document = new FlowDocument(new Paragraph(new Run(summaryVisitor.Summary)));
            LogHeal.Document = new FlowDocument(new Paragraph(new Run(healVisitor.Summary)));
            LogHeal.Document.Blocks.Add(healVisitor.Table);
            LogDamageDealt.Document = new FlowDocument(new Paragraph(new Run(damageVisitor.Summary)));
            LogDamageTaken.Document = new FlowDocument(new Paragraph(new Run(battle.Statistics.DamageTaken.GetLog())));
            LogThreat.Document = new FlowDocument(new Paragraph(new Run(battle.Statistics.Threat.GetLog())));
            LogThreat.Document.Blocks.Add(battle.Statistics.Threat.GetThreatTable());
        }
    }
}
