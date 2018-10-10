using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFactsApplication
{
    public class Problem
    {
        public DateTime StartTime { get; set; }

        public double TimeAnsweredIn { get; set; }

        public bool AnsweredCorrectly { get; set; }

        public string Question { get; set; }

        public int Solution { get; set; }

        public int[] AvailableAnswers { get; set; }
    }
}
