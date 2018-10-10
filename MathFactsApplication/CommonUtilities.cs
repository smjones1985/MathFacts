using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFactsApplication
{
    public static class CommonUtilities
    {
        private static Random random = new Random();

        public static int GetRandom(int min, int max)
        {
            return random.Next(min, max);
        }

    }
}
