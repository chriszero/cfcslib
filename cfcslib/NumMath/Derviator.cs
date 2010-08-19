using System;

using System.Collections.Generic;
using System.Text;

namespace Cfcslib.NumMath {
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
            this._k = K;
        }

        public double Calculate(double input) {
            DateTime now = DateTime.Now;
            this._tc = now - this._last;
            this._last = now;

            double output = 0.0;

            if (!this._init) {
                this._init = true;
                this._old = input;
            }
            else if (this._tc > TimeSpan.Zero) {
                output = (input - this._old) / this._tc.Ticks * this._k; // laut vorlage in PicoSekunden (1e^-6 µS)
                this._old = input;
            }

            return output;
        }
    }
}