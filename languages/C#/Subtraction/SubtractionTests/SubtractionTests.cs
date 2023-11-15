using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class SubtractionTests
    {
        [TestMethod()]
        public void subtractTest_OK()
        {
            Subtraction subtraction = new Subtraction();
            int result = subtraction.subtract(7, 5);
            Assert.AreEqual(result,2);
        }

        [TestMethod()]
        public void subtractTest_NotOk()
        {
            Subtraction subtraction = new Subtraction();
            int result = subtraction.subtract(7, 5);
            Assert.AreNotEqual(result,3);
        }
    }
}