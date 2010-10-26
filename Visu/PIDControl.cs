using System;

namespace Visu {
    public class PidControl {
        /// <summary>
        /// Maximum control value
        /// </summary>
        public double CvMax = 255.0;

        /// <summary>
        /// Minimum control value
        /// </summary>
        public double CvMin;

        /// <summary>
        /// Total Gain
        /// </summary>
        public double Gain = 1.0;

        /// <summary>
        /// Maximum integral value
        /// </summary>
        public double Imax = 10.0;

        /// <summary>
        /// Derivative Gain coeffiecient
        /// </summary>
        public double Kd = 1.5;

        /// <summary>
        /// Integral Gain coeffiecient
        /// </summary>
        public double Ki = 0.05;

        /// <summary>
        /// Proportional Gain coeffiecient
        /// </summary>
        public double Kp = 1.0;

        /// <summary>
        /// Set point
        /// </summary>
        public double Sp;

        /// <summary>
        /// Control value
        /// </summary>
        private double _cv;

        /// <summary>
        /// Integral value
        /// </summary>
        private double _i;

        private DateTime _lastIntegrate = DateTime.Now;

        /// <summary>
        /// Last process value
        /// </summary>
        private double _pvLast;

        /// <summary>
        /// Erstellt nene Instanz des Pid Reglers mit Standart Werten
        /// </summary>
        public PidControl() {
            // Setup default values
        }

        /// <summary>
        /// Erstellt nene Instanz des Pid Reglers
        /// </summary>
        /// <param name="gain">Verstärkungsfaktor</param>
        /// <param name="kp">P-Wert</param>
        /// <param name="ki">I-Wert</param>
        /// <param name="kd">D-Wert</param>
        public PidControl(double gain, double kp, double ki, double kd) {
            Gain = gain;
            Kp = kp;
            Ki = ki;
            Kd = kd;
        }

        public double Cv {
            get { return _cv; }
        }

        public double I {
            get { return _i; }
        }

        /// <summary>
        /// Berechnet die neuen Pid Werte
        /// </summary>
        /// <param name="pv">Prozess Wert (Eingang)</param>
        /// <param name="dt">Integierzeit in Millisekunden (Sampletime)</param>
        public void Update(double pv, double dt) {
            DateTime now = DateTime.Now;
            if (now - _lastIntegrate < TimeSpan.FromMilliseconds(dt)) {
                return;
            }
            _lastIntegrate = now;

            dt = dt*1.0e-3; // µS

            // Error = setpoint - process value
            double e = Sp - pv;

            // Calculate integral term
            _i += e*dt;

            // Check integral term for upper limit
            if (Ki*_i > Imax) {
                _i = Imax/Ki;
            }

            // Check integral term for lower limit
            if (Ki*_i < -Imax) {
                _i = -Imax/Ki;
            }

            double d;
            // Derivative part = (process value - last process value) / dt
            if (dt > 1e-6) // Avoid division by zero
            {
                d = (pv - _pvLast)/dt;
            }
            else {
                d = 0.0;
            }

            // Control value = Gain * (Kp * e + Ki * _i - Kd * d)
            _cv = Gain*(Kp*e + Ki*_i - Kd*d);

            // Add minimum to control value
            if (_cv >= 0.0 && _cv < CvMin) {
                _cv += CvMin;
            }
            else if (_cv < 0.0 && _cv > -CvMin) {
                _cv -= CvMin;
            }

            // Check control value upper limits
            if (_cv > CvMax) {
                _cv = CvMax;
            }

            if (_cv < -CvMax) {
                _cv = -CvMax;
            }

            // Store reference value for next control cycle
            _pvLast = pv;
        }
    }
}