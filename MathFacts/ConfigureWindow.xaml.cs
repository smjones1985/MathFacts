using MathFactsApplication;
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

namespace MathFacts
{
    /// <summary>
    /// Interaction logic for ConfigureWindow.xaml
    /// </summary>
    public partial class ConfigureWindow : Window
    {

        public ConfigureWindow()
        {
            InitializeComponent();
            operationComboBox.ItemsSource = new string[] { "x", "/", "+", "-" };
        }

        public ConfigureWindow(MathConfiguration config)
        {
            InitializeComponent();
            this.nameText.Text = config.Name;
            operationComboBox.ItemsSource = new string[] { "x", "/", "+", "-" };
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(inputNumberText.Text))
            {
                return;
            }

            if (int.TryParse(inputNumberText.Text, out int factNumber) && 
                int.TryParse(text_numberOfProblems.Text, out int numberOfProblems) &&
                int.TryParse(text_StreakGoal.Text, out int streakGoal))
            {
                MathConfiguration config = new MathConfiguration()
                {
                    Name = nameText.Text,
                    FactNumber = factNumber,
                    NumberOfProblems = numberOfProblems,
                    Operator = operationComboBox.Text,
                    StreakEscape = streakGoal
                };

                MainWindow mainWindow = new MainWindow(config);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }
    }
}
