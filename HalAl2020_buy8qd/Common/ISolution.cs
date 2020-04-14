using System;
using System.Collections.Generic;
using System.Text;

namespace HalAl2020_buy8qd.Common
{
    public interface ISolution<TSolFragment> : IComparable, ICloneable
    {
        float Fitness { get; set; }
        IList<TSolFragment> SolutionFragments { get; set; }

        string VictoryRide();
    }
}
