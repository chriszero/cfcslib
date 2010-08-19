using System;

using System.Collections.Generic;
using System.Text;

namespace Cfcslib {
    /// <summary>
    /// Berechnet die Ableitung (Steigung)
    /// </summary>
    internal class Derviator {
        /// <summary>
        /// Verstrichene Zeit seit letztem Aufruf
        /// </summary>
        private TimeSpan _tc;
        /// <summary>
        /// Zeitpunkt letzter Berechnung
        /// </summary>
        private DateTime _last;

        private double _old;
        private bool _init;

        private double _out;
        private double _k;

        public Derviator() :this(1.0){}

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

            double output = 0.0;

            if (!_init) {
                _init = true;
                _old = input;
            }
            else if (_tc > TimeSpan.Zero) {
                output = (input - _old) / _tc.Ticks * _k; // laut vorlage in PicoSekunden (1e^-6 µS)
                _old = input;
            }

            return output;
        }
    }
}
