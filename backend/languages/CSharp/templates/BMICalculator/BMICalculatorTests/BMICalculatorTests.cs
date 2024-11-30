using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass()]
    public class BMICalculatorTests
    {
        [TestMethod()]
        public void BerechneBMITest_ValidInputs_ReturnsCorrectBMI()
        {
            // Testfall 1: Normalgewicht
            double gewicht1 = 70; // kg
            double groesse1 = 1.75; // m
            double erwarteterBmi1 = 22.86; // erwarteter BMI
            double tatsächlicherBmi1 = BMICalculator.BerechneBMI(gewicht1, groesse1);
            Assert.AreEqual(erwarteterBmi1, tatsächlicherBmi1, 0.01, "Der BMI für normales Gewicht wurde nicht korrekt berechnet.");
        }

        [TestMethod()]
        public void BerechneBMITest_ValidInputs_ReturnsCorrectBMI_2()
        {
            // Testfall 2: Übergewicht
            double gewicht2 = 85; // kg
            double groesse2 = 1.75; // m
            double erwarteterBmi2 = 27.76; // erwarteter BMI
            double tatsächlicherBmi2 = BMICalculator.BerechneBMI(gewicht2, groesse2);
            Assert.AreEqual(erwarteterBmi2, tatsächlicherBmi2, 0.01, "Der BMI für Übergewicht wurde nicht korrekt berechnet.");
        }

        [TestMethod()]
        public void BerechneBMITest_ValidInputs_ReturnsCorrectBMI_3()
        {
            // Testfall 3: Untergewicht
            double gewicht3 = 50; // kg
            double groesse3 = 1.75; // m
            double erwarteterBmi3 = 16.33; // erwarteter BMI
            double tatsächlicherBmi3 = BMICalculator.BerechneBMI(gewicht3, groesse3);
            Assert.AreEqual(erwarteterBmi3, tatsächlicherBmi3, 0.01, "Der BMI für Untergewicht wurde nicht korrekt berechnet.");
        }

    }
}
