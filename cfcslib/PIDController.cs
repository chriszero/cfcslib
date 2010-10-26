using Cfcslib.NumMath;

namespace Cfcslib {
    public class PIDController {
        private readonly Derviator _diff;
        private readonly LimIntegrator _integ;
        private readonly double _kp;
        private readonly double _tn;
        private double _limH = 10;
        private double _limL = -10;
        private double _tv;

        public PIDController(double kp, double tn, double tv) {
            _integ = new LimIntegrator(1.0/tn, 20, -20);
            _diff = new Derviator(tv);
            _tn = tn;
            _tv = tv;
            _kp = kp;
        }

        public bool Calculate(double input, ref double output) {
            bool lim = false;
            double old = output;
            if (_tn > 0) {
                lim = _integ.Integrate(input, ref old);
            }
            double diff = _diff.Calculate(input);
            output = (old + diff + input)*_kp;
            return lim;
        }
    }
}