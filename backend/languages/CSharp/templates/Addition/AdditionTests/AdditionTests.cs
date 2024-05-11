using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class AdditionTests
    {
        [TestMethod()]
        public void T01_AdditionCalculation_ok()
        {
            Assert.AreEqual(Addition.AdditionCalculation(5, 3), 8);
        }

        [TestMethod()]
        public void T02_AdditionCalculation_wrong()
        {
            Assert.AreEqual(Addition.AdditionCalculation(5, 3), 10);
        }

        [TestMethod()]
        public void T03_AdditionCalculation_ok()
        {
            Assert.AreEqual(Addition.AdditionCalculation(2, 1), 3);
        }
    }
}