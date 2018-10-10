using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFactsApplication
{
    public class ProgressTracking
    {
        public string FinalResults(MathConfiguration config, List<Problem> problemSet, Dictionary<string, string> userData, int longestStreak)
        {
            double totalTime = 0;
            string responseMessage = "";
            problemSet.ForEach(x => totalTime += x.TimeAnsweredIn);
            string previousStreak;
            string previousAverageTime = null;
            string streakKey = config.Name + "_streak_" + config.FactNumber;
            string averageTimeKey = config.Name + "_averageTime_" + config.FactNumber;
            if (userData.TryGetValue(streakKey, out previousStreak))
            {
                previousAverageTime = userData[averageTimeKey];
            }
            else
            {
                userData[streakKey] = longestStreak.ToString();
                userData[averageTimeKey] = (totalTime / problemSet.Count).ToString();
            }

            if (previousAverageTime != null && !String.IsNullOrEmpty(previousAverageTime.ToString()))
            {
                responseMessage = "Your previous streak was " + longestStreak + Environment.NewLine +
                "Your average time to answer was " + totalTime / problemSet.Count + Environment.NewLine;

                if (int.Parse(previousStreak.ToString()) < longestStreak)
                {
                    userData[streakKey] = longestStreak.ToString();
                }
                if (double.Parse(previousAverageTime.ToString()) < (totalTime / problemSet.Count))
                {
                userData[averageTimeKey] = (totalTime / problemSet.Count).ToString();
                }
            }

            Properties.Settings.Default["UserData"] = JsonConvert.SerializeObject(userData);
            Properties.Settings.Default.Save();
            responseMessage = responseMessage +
                "Your longest streak was " + longestStreak + Environment.NewLine +
                "Your average time to answer was " + totalTime / problemSet.Count;

            return responseMessage;
        }
        

    }
}
