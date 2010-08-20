using System;

using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Cfcslib.NumMath {
    /// <summary>
    /// Integrator mit Limits
    /// </summary>
    public class LimIntegrator : Integrator {

        private double _outMin;
        private double _outMax;

        public LimIntegrator(double k, double outMax, double outMin)
            : base(k) {
            this._outMax = outMax;
            this._outMin = outMin;
        }

        public LimIntegrator(double k) : this(k, double.MaxValue, double.MinValue) { }
        public LimIntegrator() : this(1.0) { }

        public new bool Integrate(double input, ref double y) {
            base.Integrate(input, ref y);
            bool lim;
            if(y > this._outMax){
                y = this._outMax;
                lim = true;
            }
            else if (y < this._outMin) {
                y = this._outMin;
                lim = true;
            }
            else {
                lim = false;
            }
            return lim;
        }
    }

    public class Integrator {
        private DateTime _last;
        private bool _init;
        private double _xLast;
        private double _yLast;

        protected double _k;

        public Integrator() : this(1.0) { }

        public Integrator(double k) {
            _k = k;
        }

        /// <summary>
        /// Integiert in Echtzeit
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void Integrate(double x, ref double y) {
            DateTime now = DateTime.Now;
            Integrate(x, ref y, (now - _last).TotalMilliseconds);
            _last = now;
        }

        /// <summary>
        /// Itegriert nach angegebendem dT
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y">In/Out des Letzten Ergebnisses</param>
        /// <param name="dT">Integierzeit in Millisekunden</param>
        public virtual void Integrate(double x, ref double y, double dT) {

            if (!_init) {
                _init = true;
                _xLast = x;
            }
            else {
                // y = error * dt + y
                _yLast += (x + this._xLast) * 0.5e-3 * dT * this._k;
                y = _yLast;
                //y += (x + this._xLast) * dT * this._k;
                _xLast = x;
            }
        }
    }
}