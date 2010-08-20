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
        /// lineare Übertragungsfunktion mit Totzone und Hys-
        /// terese. Der Ausgang entspricht dem Eingangssignal, wenn der Absolutwert 
        /// des Eingangs größer als L ist.
        /// DEAD_ZONE2 = X wenn ABS(X) > L
        /// DEAD_ZONE2 = +/- L wenn ABS(X) <= L
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y">Rückgabewert des letzten durchlaufs</param>
        /// <param name="l">Totzone</param>
        public static double DeadZone2(double x, ref double y, double l) {
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
    }
}