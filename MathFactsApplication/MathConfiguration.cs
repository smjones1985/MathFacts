using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MathFactsApplication
{
    public class MathConfiguration
    {
        public int FactNumber { get; set; }
        public string Name { get; set; }
        public string Operator { get; set; }
        public int NumberOfProblems { get; set; }
        public int StreakEscape { get; set; }
        public Brush DefaultBrush { get; set; }

    }
}
