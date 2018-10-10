using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MathFactsApplication
{
    public class MathFactsManager
    {
        public List<Problem> CurrentProblemSet { get; set; }

        public Dictionary<string, string> UserData { get; set; }


        public MathConfiguration Configuration { get; set; }

        public Problem CurrentProblem { get; set; }

  
        public int longestStreak = 0;
        private int currentStreak = 0;

        public MathFactsManager(MathConfiguration configuration)
        {
            Configuration = configuration;
            ConfigureSettings();
        }

        public Problem PopulateWithProblemDetails()
        {
            var unanswered = CurrentProblemSet.Where(x => !x.AnsweredCorrectly).ToList();
            if (unanswered.Count > 0)
            {
                var currentProblemIndex = CommonUtilities.GetRandom(0, unanswered.Count() - 1);
                CurrentProblem = unanswered[currentProblemIndex];
                CurrentProblem.StartTime = DateTime.Now;
                return CurrentProblem;
            }
            return null;
        }



        public void GenerateProblemSet()
        {
            MathFactsGenerator mathFactsGenerator = new MathFactsGenerator();
            int numberOfProblems = Configuration.NumberOfProblems;
            Func<int, int, int> mathToUse = null;
            string operatorSign = Configuration.Operator;
            switch (operatorSign)
            {
                case "x":
                    mathToUse = (x, y) => { return x * y; };
                    break;
                case "/":
                    mathToUse = (x, y) => { return x / y; };
                    break;
                case "-":
                    mathToUse = (x, y) => { return x - y; };
                    break;
                case "+":
                    mathToUse = (x, y) => { return x + y; };
                    break;
                default:
                    break;
            }
            CurrentProblemSet = mathFactsGenerator.GenerateProblemSet(Configuration.FactNumber, numberOfProblems, operatorSign, mathToUse);
            
        }

        private void ConfigureSettings()
        {
            object result = null;
            try
            {
                result = Properties.Settings.Default["UserData"];
            }
            catch (Exception)
            {
            }
            if (result == null || String.IsNullOrEmpty(result.ToString()))
            {
                Properties.Settings.Default["UserData"] = JsonConvert.SerializeObject(new Dictionary<string, string>());
                UserData = new Dictionary<string, string>();
            }
            else
            {
                UserData = JsonConvert.DeserializeObject<Dictionary<string, string>>(result.ToString());
            }
        }

        public Result CheckSolution(string selectedAnswer)
        {
            Result response = new Result();
            double timeToRun = DateTime.Now.Subtract(CurrentProblem.StartTime).TotalSeconds;
            CurrentProblem.TimeAnsweredIn = timeToRun;
            if (selectedAnswer == CurrentProblem.Solution.ToString())
            {
                CurrentProblem.AnsweredCorrectly = true;
                currentStreak++;
                if (currentStreak > longestStreak)
                {
                    longestStreak = currentStreak;
                }
                response.AnsweredCorrectly = true;
                response.DisplayMessage = $"Correct! Current streak is: {currentStreak}" + Environment.NewLine + "Time to answer: " + (int)timeToRun + " total seconds";
;
            }
            else
            {
                currentStreak = 0;
                response.DisplayMessage = "Incorrect! " + CurrentProblem.Question + " = " + CurrentProblem.Solution;
            }

            return response;
        }


    }
}
