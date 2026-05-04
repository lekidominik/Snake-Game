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
    public partial class SaveScoreWindow : Window
    {
        public string PlayerName { get; private set; } = "";

        public SaveScoreWindow(int score)
        {
            InitializeComponent();
            ScoreText.Text = $"Your score: {score}";
            NameTextBox.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Please enter your name.");
                return;
            }

            PlayerName = NameTextBox.Text.Trim();
            DialogResult = true;
            Close();
        }
    }
}
