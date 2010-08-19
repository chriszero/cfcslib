using System;

using System.Collections.Generic;
using System.Text;

namespace Cfcslib.NumMath {
    internal class Integrator : PlainIntegrator {

        private double _outMin;
        private double _outMax;

        public Integrator(double k, double outMax, double outMin)
            : base(k) {
            this._outMax = outMax;
            this._outMin = outMin;
        }

        public Integrator(double k) : this(k, double.MaxValue, double.MinValue) { }
        public Integrator() : this(1.0) { }

        public new bool Integrate(double input, ref double y) {
            base.Integrate(input, ref y);
            bool lim;
            if(y >= this._outMax){
                y = this._outMax;
                lim = true;
            }
            else if (y <= this._outMin) {
                y = this._outMin;
                lim = true;
            }
            else {
                lim = false;
            }
            return lim;
        }
    }

    internal class PlainIntegrator {
        private DateTime _last;
        private bool _init;
        private double _xLast;

        protected double _k;

        public PlainIntegrator() : this(1.0) { }

        public PlainIntegrator(double k) {
            this._k = k;
        }

        public virtual void Integrate(double x, ref double y) {
            DateTime now = DateTime.Now;
            if (!this._init) {
                this._init = true;
                this._xLast = x;
            }
            else {
                y = (x + this._xLast)*0.5e-3*(now - this._last).TotalMilliseconds*1.0e-3*this._k + y;
                this._xLast = x;
            }
            this._last = now;
        }
    }
}