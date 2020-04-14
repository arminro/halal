using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HalAl2020_buy8qd.Utils
{
    // based on: https://naveed-ahmad.com/2015/12/11/c-random-ordering-of-ienumerable-using-extensions/
    public static class Extensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> coll)
        {
            var a = coll.ToList();
            var n = a.Count;

            for (var i = 0; i < n - 1; i++)
            {
                var j = Utils.random.Next(i, n);

                var temp = a[j];
                a[j] = a[i];
                a[i] = temp;
            }



            return a;
        }
    }
}
