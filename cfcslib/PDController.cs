using Cfcslib.NumMath;

namespace Cfcslib {
    public class PDController {
        private readonly Derviator _diff;
        private readonly double _kp = 1.0;
        private double _tv = 1.0;

        public PDController(double kp, double tv) {
            _diff = new Derviator(tv);
            _kp = kp;
        }

        public double Calculate(double input) {
            double y = _diff.Calculate(input);
            y = _kp*(y + input);
            return y;
        }
    }
}