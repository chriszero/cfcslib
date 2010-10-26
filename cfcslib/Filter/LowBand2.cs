using System;
using Cfcslib.NumMath;

namespace Cfcslib.Filter {
    /// <summary>
    /// Tiefpass 2. Ordnung
    /// </summary>
    public class LowBand2 : LowBand1 {
        private readonly double _d;

        private readonly Integrator _int1;
        private readonly Integrator _int2;
        private double _i1, _i2;

        public LowBand2(double k, double d) {
            _k = k;
            _d = d;
            _int1 = new Integrator(1.0);
            _int2 = new Integrator(1.0);
        }

        public LowBand2(double d) : this(1.0, d) {
        }

        /*
            IF NOT init OR T = T#0s THEN
	            init := TRUE;
	            out := K * in;
	            I2 := out;
            ELSE
	            TN := TIME_TO_REAL(T) * 1.0E-3;
	            tn2 := TN * TN;
	            int1(X := in * K / tn2 - I1 * 0.5 * D / TN - I2 / TN2, Y := I1);
	            int2(X := I1,Y := I2);
	            out := I2;
            END_IF;
         */

        public new double Calculate(double input, TimeSpan t) {
            if (!Init || t == TimeSpan.Zero) {
                Init = true;
                Out = _k*input;
                _i1 = Out;
            }
            else {
                double tn = t.TotalMilliseconds; //                *1.0e-3;
                double tn2 = tn*tn;
                _int1.Integrate(input*_k/tn2 - _i1*0.5*_d/tn - _i2/tn2, ref _i1);
                _int2.Integrate(_i1, ref _i2);
                Out = _i2;
            }
            return Out;
        }
    }
}