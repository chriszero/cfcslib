using System;
using System.Collections.Generic;
using System.Text;

namespace Cfcslib.Filter {
    /// <summary>
    /// Tiefpass 1. Ordnung
    /// </summary>
    public class LowBand1 {
        protected double _out;
        protected DateTime _last;
        protected double _k;
        protected bool _init;

        public LowBand1(double k) {
            _k = k;
        }

        public LowBand1() : this(1.0){}

        public virtual double Calculate(double input, TimeSpan t) {
            DateTime tx = DateTime.Now;
            if(!_init || t == TimeSpan.Zero) {
                _init = true;
                _out = _k*input;
            }
            else {
                _out = _out + (input*_k - _out)*(tx - _last).Ticks/t.Ticks *1.0e-3;
            }
            _last = tx;
            return _out;
        }

        public double K {
            get { return _k; }
            set { _k = value;}
        }
    }
}
