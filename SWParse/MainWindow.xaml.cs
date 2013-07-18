using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
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

            openFileDialog1.InitialDirectory = "g:\\temp\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                var result = LogParser.Parse(openFileDialog1.FileName);
                _battles = LogParser.DivideIntoBattlesAndApplyGuards(result);
                ListBattles.Items.Clear();

                foreach (var battle in _battles)
                {
                    ListBattles.Items.Add(battle.ToString());
                }
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
            LogText.IsEnabled = true;
            LogText.Document = new FlowDocument(new Paragraph(new Run(battle.GetBattleLog())));
            LogHeal.Document = new FlowDocument(new Paragraph(new Run(battle.GetHealLog())));
            LogDamageDealt.Document = new FlowDocument(new Paragraph(new Run(battle.GetDamageLog())));
            LogDamageTaken.Document = new FlowDocument(new Paragraph(new Run(battle.GetDamageTakenLog())));
            LogThreat.Document = new FlowDocument(new Paragraph(new Run(battle.GetThreatLog())));
            LogThreat.Document.Blocks.Add(battle.GetThreatTable());
        }
    }
}
