using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFactsApplication
{
    public class MathFactsGenerator
    {
        private Problem GenerateOptions(Problem problem, int factNumber)
        {
            int numberOfOptions = 4;
            problem.AvailableAnswers = new int[numberOfOptions];
            int solutionIndex = CommonUtilities.GetRandom(0, 3);
            problem.AvailableAnswers[solutionIndex] = problem.Solution;
            OptionGenerator optionGenerator = new OptionGenerator(problem.Solution);
            for (int i = 0; i < numberOfOptions; i++)
            {
                if(i != solutionIndex)
                {
                    int option = optionGenerator.Generate(factNumber);
                    problem.AvailableAnswers[i] = option;
                }
            }
            return problem;
        }

        internal List<Problem> GenerateProblemSet(int numberForFacts, int numberOfProblems, string operatorSign, Func<int, int, int> mathToUse)
        {
            List<Problem> problems = new List<Problem>();
            for (int i = 0, x = 0; i < numberOfProblems; i++, x++)
            {
                if (x > 10)
                {
                    x = 0;
                }
                Problem problem = new Problem
                {
                    Question = numberForFacts + $" {operatorSign} " + x,
                    Solution = mathToUse(numberForFacts, x)
                };
                problem = GenerateOptions(problem, numberForFacts);
                problems.Add(problem);
            }
            return problems;
        }
    }
}
