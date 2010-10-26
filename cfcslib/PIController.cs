using Cfcslib.NumMath;

namespace Cfcslib {
    public class PIController {
        private readonly LimIntegrator _integ;
        private readonly double _kp;
        private const double LimH = 1e38;
        private const double LimL = -1e38;

        public PIController(double kp, double ki) {
            _integ = new LimIntegrator(ki, LimH, LimL);
            _kp = kp;
        }

        public bool Calculate(double input, ref double output) {
            bool lim = _integ.Integrate(input, ref output);
            output = _kp*input + output;
            return lim;
        }
    }
}