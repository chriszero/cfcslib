using System;

namespace Cfcslib.NumMath {
    /// <summary>
    /// calculates the derivate over the signal "in" with Faktor "K".
    /// a run input enables or stops the calculation, if left unconnected its true and therfore the calculation is executed.
    /// if K is not specified the default is 1.
    /// </summary>
    public class Derivator {
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
        private TimeSpan _tCycle;

        public Derivator() : this(1.0) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k">Verstärkungsfaktor bzw, Multiplikator</param>
        public Derivator(double k) {
            _k = k;
        }

        public double Calculate(double input) {
            DateTime now = DateTime.Now;
            _tCycle = now - _last;
            _last = now;
            return Calculate(input, _tCycle.TotalMicroseconds());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="dT">In Mikrosekunden (µS)</param>
        /// <returns></returns>
        public double Calculate(double input, double dT) {
            if (!_init) {
                _init = true;
                _old = input;
            }
            else if (_tCycle > TimeSpan.Zero) {
                _out = (input - _old)/dT*1000000*_k; // laut vorlage in PicoSekunden (1e^-6 µS)
                _old = input;
            }
            else {
                _out = 0;
            }

            return _out;
        }
    }
}