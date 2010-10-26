using System;

namespace Cfcslib.NumMath {
    /// <summary>
    /// Integrator mit Limits
    /// </summary>
    public class LimIntegrator : Integrator {
        private readonly double _outMax;
        private readonly double _outMin;

        public LimIntegrator(double k, double outMax, double outMin)
            : base(k) {
            _outMax = outMax;
            _outMin = outMin;
        }

        public LimIntegrator(double k) : this(k, double.MaxValue, double.MinValue) {
        }

        public LimIntegrator() : this(1.0) {
        }

        public new bool Integrate(double input, ref double y) {
            base.Integrate(input, ref y);
            bool lim;
            if (y > _outMax) {
                y = _outMax;
                lim = true;
            }
            else if (y < _outMin) {
                y = _outMin;
                lim = true;
            }
            else {
                lim = false;
            }
            return lim;
        }
    }

    public class Integrator {
        private bool _init;
        protected double K;
        private DateTime _last;
        private double _xLast;
        private double _yLast;

        public Integrator() : this(1.0) {
        }

        public Integrator(double k) {
            K = k;
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
                _yLast += (x + _xLast)*0.5e-3*dT*K;
                y = _yLast;
                //y += (x + this._xLast) * dT * this._k;
                _xLast = x;
            }
        }
    }
}