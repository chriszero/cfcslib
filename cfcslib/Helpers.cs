using System;

namespace Cfcslib {
    public static class Helpers {
        /// <summary>
        /// linear transfer function which follows a linear function except for x is close to 0.
        /// Y = X if abs(x) > L otherwise its 0.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static double DeadZone(double x, double l) {
            if (Math.Abs(x) > l) {
                return x;
            }
            else {
                return 0.0;
            }
        }

        /// <summary>
        /// lineare Übertragungsfunktion mit Totzone und Hysterese.
        /// Der Ausgang entspricht dem Eingangssignal, wenn der Absolutwert 
        /// des Eingangs größer als L ist.
        /// DEAD_ZONE2 = X wenn ABS(X) > L
        /// DEAD_ZONE2 = +/- L wenn ABS(X) <= L
        /// </summary>
        /// <param name="x"></param>
        /// <param name="l">Totzone</param>
        /// <param name="y"></param>
        public static double DeadZone2(double x, double l, double y) {
            if (Math.Abs(x) > l) {
                y = x;
            }
            else if (y > 0.0) {
                y = l;
            }
            else {
                y = -l;
            }
            return y;
        }

        public static double CtrlIn(double setPoint, double actual, double noise) {
            return DeadZone(setPoint - actual, noise);
        }

        private static double Limit(double limL, double y, double limH, out bool limit) {
            limit = true;
            if (y < limL) {
                return limL;
            }
            if (y > limH) {
                return limH;
            }
            limit = false;
            return y;
        }

        public static double CtrlOut(double cInput, double offset, double limitLow, double limitHigh) {
            bool limit;
            return CtrlOut(cInput, offset, 0, limitLow, limitHigh, false, out limit);
        }

        public static double CtrlOut(double cInput, double offset, double limitLow, double limitHigh, out bool limit) {
            return CtrlOut(cInput, offset, 0, limitLow, limitHigh, false, out limit);
        }

        public static double CtrlOut(double cInput, double offset, double manInput, double limitLow, double limitHigh, bool manual, out bool limit) {
            double y = (manual ? manInput : cInput) + offset;
            return Limit(limitLow, y, limitHigh, out limit);
        }

    }
}