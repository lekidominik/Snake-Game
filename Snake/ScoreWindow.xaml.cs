using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Snake
{
    public partial class ScoreWindow : Window
    {
        private List<ScoreEntry> scores;
        public ScoreWindow()
        {
            InitializeComponent();

            scores = ScoreManager
                .LoadScores()
                .OrderByDescending(s => s.Score)
                .ToList();

            ScoresDataGrid.ItemsSource = scores;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScoresDataGrid.SelectedItem is ScoreEntry selected)
            {
                var result = MessageBox.Show(
                    $"Delete score for {selected.Name}?",
                    "Confirm",
                    MessageBoxButton.YesNo
                );

                if (result == MessageBoxResult.Yes)
                {
                    scores.Remove(selected);

                    ScoreManager.SaveAll(scores);

                    ScoresDataGrid.ItemsSource = null;
                    ScoresDataGrid.ItemsSource = scores;
                }
            }
            else
            {
                MessageBox.Show("Select a score first.");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}