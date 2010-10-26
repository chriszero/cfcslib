using System;

namespace Cfcslib.Filter {
    /// <summary>
    /// Tiefpass 1. Ordnung
    /// </summary>
    public class LowBand1 {
        protected bool Init;
        protected double _k;
        protected DateTime Last;
        protected double Out;

        public LowBand1(double k) {
            _k = k;
        }

        public LowBand1() : this(1.0) {
        }

        public double K {
            get { return _k; }
            set { _k = value; }
        }

        public virtual double Calculate(double input, TimeSpan t) {
            DateTime tx = DateTime.Now;
            if (!Init || t == TimeSpan.Zero) {
                Init = true;
                Out = _k*input;
            }
            else {
                Out += (input*_k - Out)*(tx - Last).TotalSeconds/t.TotalSeconds*1.0e-3;
            }
            Last = tx;
            return Out;
        }
    }
}