using System;
using System.Collections.Generic;
using System.Text;
using Cfcslib.NumMath;

namespace Cfcslib {
    public class PIController {
        private double _kp;
        private double _limL = -1e38;
        private double _limH = 1e38;

        private LimIntegrator _integ;

        public PIController(double kp, double ki) {
            _integ = new LimIntegrator(ki, _limH, _limL);
            _kp = kp;
        }

        public bool Calculate(double input, ref double output) {
            bool lim = _integ.Integrate(input, ref output);
            output = _kp * input + output;
            return lim;
        }
    }
}
