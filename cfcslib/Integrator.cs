using System;

using System.Collections.Generic;
using System.Text;

namespace Cfcslib {
    internal class Integrator : PlainIntegrator {

        private double _outMin;
        private double _outMax;

        public Integrator(double k, double outMax, double outMin)
            : base(k) {
            _outMax = outMax;
            _outMin = outMin;
        }

        public Integrator(double k) : this(k, double.MaxValue, double.MinValue) { }
        public Integrator() : this(1.0) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="lim">Gibt an ob der Integrator ins Limit gelaufen ist.</param>
        /// <returns></returns>
        public double Integrate(double input, out bool lim) {
            Integrate(input);
            if(_y >= _outMax){
                _y = _outMax;
                lim = true;
            }
            else if (_y <= _outMin) {
                _y = _outMin;
                lim = true;
            }
            else {
                lim = false;
            }
            return _y;
        }

        public override double Integrate(double input) {
            bool lim;
            return this.Integrate(input, out lim);
        }

        public void Reset() {
            _y = 0.0;
        }
    }

    internal class PlainIntegrator {
        private DateTime _last;
        private bool _init;
        private double _xLast;
        protected double _y;

        protected double _k;

        public PlainIntegrator() : this(1.0) { }

        public PlainIntegrator(double k) {
            _k = k;
        }

        public virtual double Integrate(double x) {
            DateTime now = DateTime.Now;
            if (!_init) {
                _init = true;
                _xLast = x;
            }
            else {
                _y = (x + _xLast) * 0.5e-3 * (now - _last).Ticks * _k + _y;
                _xLast = x;
            }
            _last = now;
            return _y;
        }
    }
}
