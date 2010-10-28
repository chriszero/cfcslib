using Cfcslib.NumMath;

namespace Cfcslib.Controller {
    /// <summary>
    /// PID-Regler mit dynamischen Wind-Up Reset der nach folgender Formel arbeitet:<br/>
    /// Y = KP * ( IN + 1/TN * INTEG(IN) + TV *DERIV(IN))
    /// </summary>
    public class PidWL {
        private readonly Derivator _diff;
        protected readonly double Kp;
        protected readonly double LimitHigh;
        protected readonly double LimitLow;

        private readonly Piwl _piwl;
        protected readonly double Tn;
        protected readonly double Tv;
        protected bool _limit;

        /// <summary>
        /// Die Regelparameter werden in der Form KP, TN und TV angegeben, falls<br/>
        /// die Parameter als KP, KI und KD vorliegen können sie entsprechend der<br/>
        /// folgenden Formel umgerechnet werden:<br/>
        /// TN = KP/KI und TV = KD/KP
        /// </summary>
        /// <param name="kp">Proportionaler Anteil</param>
        /// <param name="tn">Nachstellzeit des Reglers in Sekunden (kp/ki)</param>
        /// <param name="tv">Vorhaltezeit des Reglers in Sekunden (kd/kp)</param>
        public PidWL(double kp, double tn, double tv)
            : this(kp, tn, tv, -1.0e38, 1.0e38) {
        }

        /// <summary>
        /// Die Regelparameter werden in der Form KP, TN und TV angegeben, falls<br/>
        /// die Parameter als KP, KI und KD vorliegen können sie entsprechend der<br/>
        /// folgenden Formel umgerechnet werden:<br/>
        /// TN = KP/KI und TV = KD/KP
        /// </summary>
        /// <param name="kp">Proportionaler Anteil</param>
        /// <param name="tn">Nachstellzeit des Reglers in Sekunden (kp/ki)</param>
        /// <param name="tv">Vorhaltezeit des Reglers in Sekunden (kd/kp)</param>
        /// <param name="limitLow">untere Ausgangsbegrenzung des Integrators</param>
        /// <param name="limitHigh">obere Ausgangsbegrenzung des Integrators</param>
        public PidWL(double kp, double tn, double tv, double limitLow, double limitHigh) {
            Kp = kp;
            Tn = tn;
            Tv = tv;
            LimitLow = limitLow;
            LimitHigh = limitHigh;

            // we need to check if _tn = 0 and do alternative calls
            _piwl = Tn == 0 ? new Piwl(Kp, 0, LimitLow, LimitHigh) : new Piwl(Kp, 1.0/Tn, LimitLow, LimitHigh);
            _diff = new Derivator(Kp*Tv);
        }

        /// <summary>
        /// True, wenn der Ausgang ein Limit erreicht hat.
        /// </summary>
        public virtual bool Limit {
            get { return _limit; }
        }

        public virtual double Calculate(double input) {
            // run Piwl controller first
            double y = _piwl.Calculate(input);

            // run differentiator and add to output
            y += _diff.Calculate(input);

            // limit the output
            _limit = false;
            if (y < LimitLow) {
                _limit = true;
                y = LimitLow;
            }
            else if (y > LimitHigh) {
                _limit = true;
                y = LimitHigh;
            }
            return y;
        }

        /// <summary>
        /// Resetet den Controller beim nächsten Aufruf
        /// </summary>
        public virtual void Reset() {
            _piwl.Reset();
        }
    }
}