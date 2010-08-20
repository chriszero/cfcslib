﻿using System;

namespace Visu {
    public class PidControl {
        /// <summary>
        /// Total Gain
        /// </summary>
        public double Gain = 1.0;
        /// <summary>
        /// Proportional Gain coeffiecient
        /// </summary>
        public double Kp = 1.0;
        /// <summary>
        /// Integral Gain coeffiecient
        /// </summary>
        public double Ki = 0.05;
        /// <summary>
        /// Derivative Gain coeffiecient
        /// </summary>
        public double Kd = 1.5;
        /// <summary>
        /// Set point
        /// </summary>
        public double SP;
        /// <summary>
        /// Last process value
        /// </summary>
        private double _pvLast;
        /// <summary>
        /// Control value
        /// </summary>
        private double _cv;
        /// <summary>
        /// Minimum control value
        /// </summary>
        public double CvMin = 0;
        /// <summary>
        /// Maximum control value
        /// </summary>
        public double CvMax = 255.0;
        /// <summary>
        /// Integral value
        /// </summary>
        private double _i;
        /// <summary>
        /// Maximum integral value
        /// </summary>
        public double Imax = 10.0;

        private DateTime _lastIntegrate = DateTime.Now;

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
            this.Gain = gain;
            this.Kp = kp;
            this.Ki = ki;
            this.Kd = kd;
        }

        /// <summary>
        /// Berechnet die neuen Pid Werte
        /// </summary>
        /// <param name="pv">Prozess Wert (Eingang)</param>
        /// <param name="dt">Integierzeit in Millisekunden (Sampletime)</param>
        public void Update(double pv, double dt) {
            DateTime now = DateTime.Now;
            if (now - _lastIntegrate  < TimeSpan.FromMilliseconds(dt)) {
                return;
            }
            this._lastIntegrate = now;

            dt = dt * 1.0e-3; // µS

            // Error = setpoint - process value
            double e = this.SP - pv;

            // Calculate integral term
            this._i += e * dt;

            // Check integral term for upper limit
            if (this.Ki * this._i > this.Imax) {
                this._i = this.Imax / this.Ki;
            }

            // Check integral term for lower limit
            if (this.Ki * this._i < -this.Imax) {
                this._i = -this.Imax / this.Ki;
            }

            double d;
            // Derivative part = (process value - last process value) / dt
            if (dt > 1e-6) // Avoid division by zero
            {
                d = (pv - this._pvLast) / dt;
            }
            else {
                d = 0.0;
            }

            // Control value = Gain * (Kp * e + Ki * _i - Kd * d)
            this._cv = this.Gain * (this.Kp * e + this.Ki * this._i - this.Kd * d);

            // Add minimum to control value
            if (this._cv >= 0.0 && this._cv < this.CvMin) {
                this._cv += this.CvMin;
            }
            else if (this._cv < 0.0 && this._cv > -this.CvMin) {
                this._cv -= this.CvMin;
            }

            // Check control value upper limits
            if (this._cv > this.CvMax) {
                this._cv = this.CvMax;
            }

            if (this._cv < -this.CvMax) {
                this._cv = -this.CvMax;
            }

            // Store reference value for next control cycle
            this._pvLast = pv;
        }

        public double Cv {
            get {
                return this._cv;
            }
        }

        public double I {
            get {
                return this._i;
            }
        }
    }
}