using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFactsApplication
{
    public class OptionGenerator
    {
        private List<string> Options { get; set; }
        private List<int> RecordedOptions { get; set; }
        public static Random GetRandom = new Random();
        private int OriginalSolution { get; set; }


        public OptionGenerator(int solution)
        {
            OriginalSolution = solution;
            Options = Enum.GetNames(typeof(Option)).ToList();
            RecordedOptions = new List<int>();
        }

        public int Generate(int fact)
        {
            
            int increment = 1;
            int solution = OriginalSolution;
            while (true)
            {
                int attemptedOption = solution;
                if (Options.Count != 0)
                {
                    int attemptOptionIndex = GetRandom.Next(0, Options.Count - 1);
                    switch (Enum.Parse(typeof(Option), Options[attemptOptionIndex]))
                    {
                        case Option.increment1:
                            attemptedOption += increment;
                            break;
                        case Option.decrement1:
                            attemptedOption -= increment;
                            break;
                        case Option.incrementByFact:
                            attemptedOption += fact;
                            break;
                        case Option.incrementRandom:
                            attemptedOption += GetRandom.Next(0, 20);
                            break;
                        case Option.decrementRandom:
                            attemptedOption -= GetRandom.Next(0, 20);
                            break;
                        case Option.decrementByFact:
                            attemptedOption -= fact;
                            break;
                        default:
                            break;
                    }
                    Options.RemoveAt(attemptOptionIndex);

                    if (attemptedOption != solution && !RecordedOptions.Contains(attemptedOption))
                    {
                        RecordedOptions.Add(attemptedOption);
                        return attemptedOption;
                    }
                }
                else
                {
                    solution++;
                    Options = Enum.GetNames(typeof(Option)).ToList();
                    continue;
                }
             
            }

        }

        private enum Option
        {
            increment1,
            decrement1,
            incrementByFact,
            decrementByFact,
            incrementRandom,
            decrementRandom
        }
    }
}
