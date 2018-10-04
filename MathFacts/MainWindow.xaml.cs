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

namespace MathFacts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static Button currentSelection;

        private static List<Problem> CurrentProblemSet{get;set;}

        private Dictionary<string, string> UserData { get; set; }

        private static Random GetRandom = new Random();

        public Dictionary<string, string> Configuration { get; set; }

        private Problem currentProblem;
        private int currentProblemIndex;
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

        private int longestStreak = 0;
        private int currentStreak = 0;

        public MainWindow()
        {
            InitializeComponent();
            ConfigureWindow configureWindow = new ConfigureWindow();
            configureWindow.Show();
            this.Close();
        }

        public MainWindow(Dictionary<string, string> configuration)
        {
            InitializeComponent();
            Configuration = configuration;
            object result = null;
            try
            {
                result = Properties.Settings.Default["UserData"];
            }
            catch (Exception)
            {
            }
            if(result == null || String.IsNullOrEmpty(result.ToString()))
            {
                Properties.Settings.Default["UserData"] = JsonConvert.SerializeObject(new Dictionary<string, string>());
                UserData = new Dictionary<string, string>();
            }
            else
            {
                UserData = JsonConvert.DeserializeObject<Dictionary<string, string>>(result.ToString());

            }

            BeginApplicationAsync(Configuration["factNumber"]);
        }

        private void BeginApplicationAsync(string inputNumber)
        {
            label_count.SetBinding(ContentProperty, new Binding("ProblemCount"));
            label_count.DataContext = this;

            MathFactsGenerator mathFactsGenerator = new MathFactsGenerator();
            int inputParsed;
            if (int.TryParse(inputNumber, out inputParsed))
            {
                int numberOfProblems = int.Parse(Configuration["numberOfProblems"]);
                switch (Configuration["operator"])
                {
                    case "x":
                        CurrentProblemSet = mathFactsGenerator.GenerateMultiplication(inputParsed, numberOfProblems);
                        break;
                    case "/":
                        CurrentProblemSet = mathFactsGenerator.GenerateDivision(inputParsed, numberOfProblems);
                        break;
                    case "-":
                        CurrentProblemSet = mathFactsGenerator.GenerateSubtraction(inputParsed, numberOfProblems);
                        break;
                    case "+":
                        CurrentProblemSet = mathFactsGenerator.GenerateAddition(inputParsed, numberOfProblems);
                        break;
                    default:
                        break;
                }
                label_totalNumberOfProblems.Content = "/ " + CurrentProblemSet.Count;
                PopulateWithProblemDetails();
            }


        }

        private void PopulateWithProblemDetails()
        {
            ProblemCount += 1;
            var unanswered = CurrentProblemSet.Where(x => !x.Answered).ToList() ;
            if (unanswered.Count > 0)
            {
                currentProblemIndex = GetRandom.Next(0, unanswered.Count() - 1);
                currentProblem = unanswered[currentProblemIndex];
                calculationLabel.Content = currentProblem.Question;
                List<int> answerIndex = new List<int>() { 0, 1, 2, 3 };
                Button[] buttons = { buttonOption1, buttonOption2, buttonOption3, buttonOption4 };
                for (int i = 0; i < 4; i++)
                {
                    int indexToUse = GetRandom.Next(0, answerIndex.Count);
                    buttons[i].Content = currentProblem.AvailableAnswers[answerIndex[indexToUse]];
                    answerIndex.RemoveAt(indexToUse);
                }
                currentProblem.StartTime = DateTime.Now;

            }
            else
            {
                FinalResults();
            }
        }

        private void FinalResults()
        {
            double totalTime = 0;
            CurrentProblemSet.ForEach(x => totalTime += x.TimeAnsweredIn);
            currentSelection = null;
            calculationLabel.Content = "";
            buttonOption1.Content = "";
            buttonOption2.Content = "";
            buttonOption3.Content = "";
            buttonOption4.Content = "";
            currentProblem = null;
            string previousStreak;
            string previousAverageTime = null;
        
            if (UserData.TryGetValue(Configuration["name"] + "_streak)_" + Configuration["factNumber"], out previousStreak))
            {
                previousAverageTime = UserData[Configuration["name"] + "_averageTime_" + Configuration["factNumber"]];
            }
            else
            {
                UserData[Configuration["name"] + "_streak_" + Configuration["factNumber"]] = longestStreak.ToString();
                UserData[Configuration["name"] + "_averageTime_" + Configuration["factNumber"]] = (totalTime / CurrentProblemSet.Count).ToString();
            }

            string previousInfo = "";
            if(previousAverageTime != null && !String.IsNullOrEmpty(previousAverageTime.ToString()))
            {
                previousInfo = "Your previous streak was " + longestStreak + Environment.NewLine +
                "Your average time to answer was " + totalTime / CurrentProblemSet.Count + Environment.NewLine;

                if(int.Parse(previousStreak.ToString()) < longestStreak)
                {
                    UserData[Configuration["name"] + "_streak_" + Configuration["factNumber"]] = longestStreak.ToString();
                }
                if(double.Parse(previousAverageTime.ToString()) < (totalTime / CurrentProblemSet.Count))
                {
                    UserData[Configuration["name"] + "_averageTime_" + Configuration["factNumber"]] = (totalTime / CurrentProblemSet.Count).ToString();
                }
            }
    
            Properties.Settings.Default["UserData"] = JsonConvert.SerializeObject(UserData);
            Properties.Settings.Default.Save();
            MessageBox.Show(previousInfo +  
                "Your longest streak was " + longestStreak + Environment.NewLine +
                "Your average time to answer was " + totalTime / CurrentProblemSet.Count, "FinalResult");


            CurrentProblemSet = null;
            ConfigureWindow configWindow = new ConfigureWindow();
            configWindow.nameText.Text = Configuration["name"];
            configWindow.Show();
            this.Close();

        }

        private void CheckSolution()
        {
            double timeToRun = DateTime.Now.Subtract(currentProblem.StartTime).TotalSeconds;
            currentProblem.TimeAnsweredIn = timeToRun;
            var currentBrushSetting = this.Background;
            if (currentSelection.Content.ToString() == currentProblem.Solution.ToString())
            {
                currentProblem.Answered = true;
                currentStreak++;
                if (currentStreak > longestStreak)
                {
                    longestStreak = currentStreak;
                }
                this.Background = Brushes.PaleGreen;
                MessageBox.Show($"Correct! Current streak is: {currentStreak}", "Time to answer: " + (int)timeToRun + " total seconds", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                this.Background = Brushes.Red;

                currentStreak = 0;
                MessageBox.Show("Incorrect! " + currentProblem.Question + " = " + currentProblem.Solution, "Result", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            CurrentProblemSet[currentProblemIndex] = currentProblem;
            currentSelection = null;
            this.Background = currentBrushSetting;

            PopulateWithProblemDetails();
        }

        private void buttonOption1_Click(object sender, RoutedEventArgs e)
        {
            currentSelection = (Button)sender;
            CheckSolution();
        }

        private void buttonOption3_Click(object sender, RoutedEventArgs e)
        {
            currentSelection = (Button)sender;
            CheckSolution();
        }

        private void buttonOption2_Click(object sender, RoutedEventArgs e)
        {
            currentSelection = (Button)sender;
            CheckSolution();
        }

        private void buttonOption4_Click(object sender, RoutedEventArgs e)
        {
            currentSelection = (Button)sender;
            CheckSolution();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            Interlocked.CompareExchange(ref PropertyChanged, null, null)?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
