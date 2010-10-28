using System;
using System.Collections.Generic;
using System.Text;

namespace Cfcslib.Controller {
    /// <summary>
    /// Pi Controller with dynamic anti Wind-Up
    /// </summary>
    internal class Piwl {
        private readonly double _kp;
        private readonly double _ki;
        private readonly double _limitLow;
        private readonly double _limitHigh;

        private TimeSpan _tCycle;
        private DateTime _tLast;
        private double _inLast;
        private double _i;
        private double _p;
        private bool _init;

        public Piwl(double kp, double ki, double limitLow, double limitHigh) {
            _kp = kp;
            _ki = ki;
            _limitLow = limitLow;
            _limitHigh = limitHigh;
            _init = true;
        }

        public double Calculate(double input) {
            if (_init) {
                _init = false;
                Init(input);
            }
            // read last cycle time
            DateTime tNow = DateTime.Now;
            _tCycle = tNow - _tLast;
            _tLast = tNow;

            // calculate proportional part
            _p = _kp*input;

            // run integrator
            _i = (input + _inLast)*0.5e-6*_ki*_tCycle.TotalMicroseconds() + _i;
            _inLast = input;

            // calculate output
            double y = _p + _i;

            // check output for limits
            Limit = false;
            if (y >= _limitHigh) {
                y = _limitHigh;
                if (_ki != 0) {
                    _i = _limitHigh - _p;
                }
                else {
                    _i = 0;
                }
                Limit = true;
            }
            else if (y <= _limitLow) {
                y = _limitLow;
                if (_ki != 0) {
                    _i = _limitLow - _p;
                }
                else {
                    _i = 0;
                }
                Limit = true;
            }

            return y;
        }

        /// <summary>
        /// Initialisiert den Controller
        /// </summary>
        /// <param name="input"></param>
        private void Init(double input) {
            _inLast = input;
            _tLast = DateTime.Now;
            _i = 0;
            _tCycle = TimeSpan.Zero;
        }

        /// <summary>
        /// Resetet den Controller beim nächsten Aufruf
        /// </summary>
        public void Reset() {
            _init = true;
        }

        /// <summary>
        /// True, wenn der Ausgang das Limit erreicht hat
        /// </summary>
        public bool Limit { get; private set; }

    }
}
