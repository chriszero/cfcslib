using System;
using System.Collections.Generic;
using System.Text;
using Cfcslib.NumMath;

namespace Cfcslib {
    public class PIDController {
        private double _kp;
        private double _tn;
        private double _tv;
        private double _limL = -10;
        private double _limH = 10;

        private LimIntegrator _integ;
        private Derviator _diff;

        public PIDController(double kp, double tn, double tv) {
            _integ = new LimIntegrator(1.0 / tn, 20, -20);
            _diff = new Derviator(tv);
            _tn = tn;
            _tv = tv;
            _kp = kp;
        }

        public bool Calculate(double input, ref double output) {
            bool lim = false;
            double old = output;
            if(_tn > 0) {
                lim = _integ.Integrate(input, ref old);
            }
            double diff = _diff.Calculate(input);
            output = (old + diff + input) * _kp; 
            return lim;
        }
    }
}
