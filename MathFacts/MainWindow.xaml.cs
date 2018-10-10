using MathFactsApplication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Input;

namespace MathFacts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static Button currentSelection;
        private static List<Button> buttons;

        private int problemCount;

        public int ProblemCount
        {
            get
            {
                return problemCount;
            }
            set
            {
                problemCount = value;
                OnPropertyChanged("ProblemCount");
            }
        }

        public MathFactsManager MathFactsManagerObj { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ConfigureWindow configureWindow = new ConfigureWindow();
            configureWindow.Show();
            this.Close();
        }

        public MainWindow(MathConfiguration configuration)
        {
            InitializeComponent();
            configuration.DefaultBrush = this.Background;
            MathFactsManagerObj = new MathFactsManager(configuration);
            buttons = new List<Button>() { buttonOption2, buttonOption1, buttonOption3, buttonOption4 };
            this.WindowState = WindowState.Maximized;
            BeginApplicationAsync();
        }

        private void BeginApplicationAsync()
        {
            label_count.SetBinding(ContentProperty, new Binding("ProblemCount"));
            label_count.DataContext = this;
            MathFactsManagerObj.GenerateProblemSet();
            label_totalNumberOfProblems.Content = "/ " + MathFactsManagerObj.CurrentProblemSet.Count;
            PopulateQuestionDetails();
        }

      

        private void PopulateQuestionDetails()
        {
            var currentProblem = MathFactsManagerObj.PopulateWithProblemDetails();
            if (currentProblem != null)
            {
                calculationLabel.Content = currentProblem.Question;
                List<int> answerIndex = new List<int>() { 0, 1, 2, 3 };
                for (int i = 0; i < 4; i++)
                {
                    int indexToUse = CommonUtilities.GetRandom(0, answerIndex.Count);
                    buttons[i].Content = currentProblem.AvailableAnswers[answerIndex[indexToUse]];
                    answerIndex.RemoveAt(indexToUse);
                }
            }
            else
            {
                ProgressTracking progressTracking = new ProgressTracking();

                var response = progressTracking.FinalResults(MathFactsManagerObj.Configuration, MathFactsManagerObj.CurrentProblemSet,
                    MathFactsManagerObj.UserData, MathFactsManagerObj.longestStreak);
                MessageBox.Show(response, "Result", MessageBoxButton.OK, MessageBoxImage.None);
     
                ConfigureWindow configWindow = new ConfigureWindow(MathFactsManagerObj.Configuration);
                configWindow.Show();
                this.Close();
            }
         
        }

        private new void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if(!buttonOption1.IsEnabled)
                {
                    ActivateButtons();
                    this.Background = MathFactsManagerObj.Configuration.DefaultBrush;
                    label_spaceBar.Visibility = Visibility.Hidden;

                    PopulateQuestionDetails();
                }
            }
        }

        private void ButtonOption_Click(object sender, RoutedEventArgs e)
        {
            currentSelection = (Button)sender;
            var response = MathFactsManagerObj.CheckSolution(currentSelection.Content.ToString());
            calculationLabel.Content = response.DisplayMessage;
            if (response.AnsweredCorrectly)
            {
                ProblemCount += 1;
                this.Background = Brushes.PaleGreen;
            }
            else
            {
                this.Background = Brushes.Red;
            }
            label_spaceBar.Visibility = Visibility.Visible;
            DisableButtonsAndClearContent();
        }

       

        private void DisableButtonsAndClearContent()
        {
            foreach (var item in buttons)
            {
                item.IsEnabled = false;
                item.Content = "";
            }
        }

        private void ActivateButtons()
        {
            foreach (var item in buttons)
            {
                item.IsEnabled = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            Interlocked.CompareExchange(ref PropertyChanged, null, null)?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
