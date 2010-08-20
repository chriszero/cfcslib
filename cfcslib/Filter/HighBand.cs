using System;
using System.Collections.Generic;
using System.Text;

namespace Cfcslib.Filter {
    public class HighBand {
        LowBand1 _t1 = new LowBand1();
        private double _k;

        public HighBand(double k) {
            _k = k;
        }

        public HighBand() : this(1.0) { }

        public double Calculate(double input, TimeSpan t) {
            double o = _t1.Calculate(input, t);
            return (input - o) * _k;
        }
    }
}
