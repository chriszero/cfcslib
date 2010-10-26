using System;

namespace Cfcslib.NumMath {
    /// <summary>
    /// Berechnet die Ableitung (Steigung)
    /// </summary>
    public class Derviator {
        private readonly double _k;
        private bool _init;

        /// <summary>
        /// Zeitpunkt letzter Berechnung
        /// </summary>
        private DateTime _last;

        private double _old;

        private double _out;

        /// <summary>
        /// Verstrichene Zeit seit letztem Aufruf
        /// </summary>
        private TimeSpan _tc;

        public Derviator() : this(1.0) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="K">Verstärkungsfaktor bzw, Multiplikator</param>
        public Derviator(double K) {
            _k = K;
        }

        public double Calculate(double input) {
            DateTime now = DateTime.Now;
            _tc = now - _last;
            _last = now;
            return Calculate(input, _tc.TotalMilliseconds);
        }

        public double Calculate(double input, double dT) {
            if (!_init) {
                _init = true;
                _old = input;
            }
            else if (_tc > TimeSpan.Zero) {
                _out = (input - _old)/dT*_k; // laut vorlage in PicoSekunden (1e^-6 µS)
                _old = input;
            }

            return _out;
        }
    }
}