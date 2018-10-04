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



        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(inputNumberText.Text))
            {
                return;
            }

            var config = new Dictionary<string, string>
            {
                { "factNumber", inputNumberText.Text },
                { "name", nameText.Text },
                { "operator", operationComboBox.SelectedItem.ToString() },
                { "numberOfProblems", text_numberOfProblems.Text}
            };
            MainWindow mainWindow = new MainWindow(config);
            mainWindow.Show();
            this.Close();
        }
    }
}
