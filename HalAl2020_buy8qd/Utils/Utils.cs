using System;

namespace HalAl2020_buy8qd.Utils
{
    public class Utils
    {
        public static Random random = new Random();

        public static bool LoopStopCondition(int current, int max)
        {
            return current > max;
        }

        public static bool NotZeroStopCondition(int current)
        {
            return current < 0;
        }
    }
}
