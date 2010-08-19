using System;
using System.Collections.Generic;
using System.Text;

using Cfcslib.NumMath;

namespace Cfcslib.Filter {
    /// <summary>
    /// Tiefpass 2. Ordnung
    /// </summary>
    public class LowBand2 : LowBand1{
        private double _d;
        private double _i1, _i2;

        private readonly PlainIntegrator _int1;
        private readonly PlainIntegrator _int2;

        public LowBand2(double k, double d) {
            _k = k;
            _d = d;
            _int1 = new PlainIntegrator();
            _int2 = new PlainIntegrator();
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
            if (!_init || t == TimeSpan.Zero) {
                _init = true;
                _out = _k * input;
                _i1 = _out;
            }
            else {
                double tn = t.TotalMilliseconds*1.0e-3;
                double tn2 = tn*tn;
                _int1.Integrate(input*_k/tn2 - _i1*0.5*_d/tn - _i2/tn2, ref _i1);
                _int2.Integrate(_i1, ref _i2);
                _out = _i2;
            }
            return _out;
        }
    }
}
