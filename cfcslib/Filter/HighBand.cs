using System;

namespace Cfcslib.Filter {
    public class HighBand {
        private readonly double _k;
        private readonly LowBand1 _t1 = new LowBand1();

        public HighBand(double k) {
            _k = k;
        }

        public HighBand() : this(1.0) {
        }

        public double Calculate(double input, TimeSpan t) {
            double o = _t1.Calculate(input, t);
            return (input - o)*_k;
        }
    }
}