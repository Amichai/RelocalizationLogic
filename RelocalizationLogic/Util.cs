using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelocalizationLogic
{
    static class Util
    {
        public static int MaxIndex<T>(this IEnumerable<T> sequence,
            Func<T, IComparable> eval)
        {
            int maxIndex = -1;
            T maxValue = default(T); // Immediately overwritten anyway
            int index = 0;
            foreach (T value in sequence)
            {
                var a = eval(value);
                IComparable b = null;
                if (maxValue != null) { 
                    b = eval(maxValue);
                }

                //Debug.Print($"{a}");

                if (maxIndex == -1 || a.CompareTo(b) == 1)
                {
                    maxIndex = index;
                    maxValue = value;
                }
                index++;
            }
            return maxIndex;
        }
    }
}
