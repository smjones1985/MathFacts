using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFactsApplication
{
    public class MathFactsGenerator
    {
        public static Random GetRandom = new Random();
        public List<Problem> GenerateMultiplication(int numberForFacts, int numberOfProblems)
        {
            List<Problem> problems = new List<Problem>();
            for (int i = 0; i < numberOfProblems; i++)
            {
                Problem problem = new Problem();
                problem.Question = numberForFacts + " * " + i;
                problem.Solution = i * numberForFacts;
                problem = GenerateOptions(problem, numberForFacts);
                problems.Add(problem);
            }
            return problems;
        }

        private Problem GenerateOptions(Problem problem, int factNumber)
        {
            int numberOfOptions = 4;
            problem.AvailableAnswers = new int[numberOfOptions];
            int solutionIndex = GetRandom.Next(0, 3);
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

        public List<Problem> GenerateDivision(int numberForFacts, int numberOfProblems)
        {
            List<Problem> problems = new List<Problem>();
            for (int i = 0; i < numberOfProblems; i++)
            {
                Problem problem = new Problem();
                problem.Question = numberForFacts + " / " + i;
                problem.Solution = numberForFacts / i;
                problem = GenerateOptions(problem, numberForFacts);
                problems.Add(problem);
            }
            return problems;
        }

        public List<Problem> GenerateSubtraction(int numberForFacts, int numberOfProblems)
        {
            List<Problem> problems = new List<Problem>();
            for (int i = 0; i < numberOfProblems; i++)
            {
                Problem problem = new Problem();
                problem.Question = numberForFacts + " - " + i;
                problem.Solution = numberForFacts - i;
                problem = GenerateOptions(problem, numberForFacts);
                problems.Add(problem);
            }
            return problems;
        }

        public List<Problem> GenerateAddition(int numberForFacts, int numberOfProblems)
        {
            List<Problem> problems = new List<Problem>();
            for (int i = 0; i < numberOfProblems; i++)
            {
                Problem problem = new Problem();
                problem.Question = numberForFacts + " + " + i;
                problem.Solution = i + numberForFacts;
                problem = GenerateOptions(problem, numberForFacts);
                problems.Add(problem);
            }
            return problems;
        }
    }
}
