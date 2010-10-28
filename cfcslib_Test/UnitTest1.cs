using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Cfcslib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cfcslib_Test {
    /// <summary>
    /// Zusammenfassungsbeschreibung für UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1 {
        public UnitTest1() {
            //
            // TODO: Konstruktorlogik hier hinzufügen
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Ruft den Textkontext mit Informationen über
        ///den aktuellen Testlauf sowie Funktionalität für diesen auf oder legt diese fest.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Zusätzliche Testattribute
        //
        // Sie können beim Schreiben der Tests folgende zusätzliche Attribute verwenden:
        //
        // Verwenden Sie ClassInitialize, um vor Ausführung des ersten Tests in der Klasse Code auszuführen.
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Verwenden Sie ClassCleanup, um nach Ausführung aller Tests in einer Klasse Code auszuführen.
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen. 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Mit TestCleanup können Sie nach jedem einzelnen Test Code ausführen.
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void TestDeadZone() {
            double a = Helpers.DeadZone(1.5, 1);
            double b = Helpers.DeadZone(0.9, 1);
            double c = Helpers.DeadZone(-1.5, 1);
            double d = Helpers.DeadZone(-0.9, 1);

            Assert.AreEqual(1.5, a);
            Assert.AreEqual(0, b);
            Assert.AreEqual(-1.5, c);
            Assert.AreEqual(0, d);
        }

        [TestMethod]
        public void TestDeadZone2() {
            double a = Helpers.DeadZone2(1.5, 1, 0);
            double c = Helpers.DeadZone2(-1.5, 1, 0);

            double b = Helpers.DeadZone2(0.9, 1, 1);
            double d = Helpers.DeadZone2(-0.9, 1, -1);

            Assert.AreEqual(1.5, a);
            Assert.AreEqual(-1.5, c);

            Assert.AreEqual(1, b);
            Assert.AreEqual(-1, d);
        }

        [TestMethod]
        public void TestCtrlIn() {
            double a = Helpers.CtrlIn(10, 20, 0);
            Assert.AreEqual(-10, a);

            double b = Helpers.CtrlIn(10, 13, 5);
            Assert.AreEqual(0, b);
        }

        [TestMethod]
        public void TestCtrlOut() {

        }
    }
}
