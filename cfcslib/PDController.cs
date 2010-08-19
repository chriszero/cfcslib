using System;

using System.Collections.Generic;
using System.Text;

using Cfcslib.NumMath;

namespace Cfcslib {
    public class PDController {
        private double _kp = 1.0;
        private double _tv = 1.0;

        private Derviator _diff;

        public PDController(double kp, double tv) {
            _diff = new Derviator(tv);
            _kp = kp;
        }

        public double Calculate(double input) {
            double y = _diff.Calculate(input);
            y = _kp * (y + input);
            return y;
        }
    }
}
