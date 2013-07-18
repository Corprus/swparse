using System.Collections.Generic;
using Microsoft.Win32;
using System;
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
            var parseDoc = new FlowDocument();
            var par = new Paragraph(new Run(string.Format("Total combats: {0}", _battles.Count)))
            {
                Margin = new Thickness(0)
            };
//            parseDoc.Blocks.Add(new Paragraph(new Run(string.Format("Total combats: {0}", _battles.Count))));

            string text = string.Format("Combat {0:T} - {1:T}, duration {2:g}, enter: {3}, exit: {4}", battle.Start, battle.End, battle.Duration, battle.Enter.Effect.Effect, battle.Exit.Effect.Effect);
            text += string.Format(Environment.NewLine + "Damage: {0}", battle.Damage);
            text += string.Format(Environment.NewLine + "DPS: {0:0.##}", battle.DPS);
            text += string.Format(Environment.NewLine + "Crit Damage: {0}", battle.CritDamage);
            text += string.Format(Environment.NewLine + "Crit Damage %: {0: 0.##}", battle.CritDamagePercent * 100);
            text += string.Format(Environment.NewLine + Environment.NewLine + "Heals Given: {0}", battle.HealsGiven);
            text += string.Format(Environment.NewLine + "Overheal: {0}", battle.Overheal);
            text += string.Format(Environment.NewLine + "Normal Heal: {0}", battle.NormalHeals);
            text += string.Format(Environment.NewLine + "Crit Heal: {0}", battle.CritHeals);
            text += string.Format(Environment.NewLine + "Crit Heals %: {0:0.##}%", battle.CritHealsPercent * 100);

            text += string.Format(Environment.NewLine + "HPS: {0:0.##}", battle.HPS);
            text += string.Format(Environment.NewLine + "EHPS: {0:0.##}", battle.EHPS);
            text += string.Format(Environment.NewLine + "Effective healing%: {0:0.##}%", battle.EffectiveHealsPercent * 100);
            var battleParagraph = new Paragraph(new Run(text));
            par.Margin = new Thickness(0);
            parseDoc.Blocks.Add(battleParagraph);
            LogText.Document = parseDoc;
        }
    }
}
