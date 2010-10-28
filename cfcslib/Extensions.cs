using System;
using System.Collections.Generic;
using System.Text;

namespace Cfcslib {
    internal static class Extensions {

        public const long TicksPerMicrosecond = 10;

        public static long TotalMicroseconds(this TimeSpan ts) {
            return ts.Ticks/TicksPerMicrosecond;
        }
    }
}
