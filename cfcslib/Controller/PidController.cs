namespace Cfcslib.Controller {
    /// <summary>
    /// PID-Regler mit dynamischen Anti Wind-Up
    /// Y = KP * ( DIFF + 1/TN * INTEG(DIFF) + TV *DERIV(DIFF)) + OFFSET
    /// </summary>
    public class PidController : PidWL{

        public PidController(double kp, double tn, double tv)
            : this(kp, tn, tv, -1000, 1000) {
        }

        /// <summary>
        /// Die Regelparameter werden in der Form KP, TN und TV angegeben, falls<br/>
        /// die Parameter als KP, KI und KD vorliegen können sie entsprechend der<br/>
        /// folgenden Formel umgerechnet werden:<br/>
        /// TN = KP/KI und TV = KD/KP
        /// </summary>
        /// <param name="kp">Verstärkung des Reglers</param>
        /// <param name="tn">Nachstellzeit des Reglers in Sekunden (kp/ki)</param>
        /// <param name="tv">Vorhaltezeit des Reglers in Sekunden (kd/kp)</param>
        /// <param name="limitLow">untere Ausgangsbegrenzung</param>
        /// <param name="limitHigh">obere Ausgangsbegrenzung</param>
        public PidController(double kp, double tn, double tv, double limitLow, double limitHigh)
            : base(kp, tn, tv, limitLow, limitHigh) {
        }

        public double Calculate(double setPoint, double actual, double noise, double offset) {
            Diff = Helpers.CtrlIn(setPoint, actual, noise);
            double y = base.Calculate(Diff);

            y = Helpers.CtrlOut(y, offset, LimitLow, LimitHigh, out _limit);
            return y;
        }

        /// <summary>
        /// Regelabweichung
        /// </summary>
        public double Diff { get; private set; }
    }
}