using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubtractionEmpty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtractionEmpty.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void SubtractTest1()
        {
            int num1 = 10;
            int num2 = 5;
            int expected = 5;
            int actual = Program.Subtract(num1, num2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SubtractTest2()
        {
            int num1 = 10;
            int num2 = 2;
            int expected = 8;
            int actual = Program.Subtract(num1, num2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SubtractTest3()
        {
            int num1 = 25;
            int num2 = 5;
            int expected = 20;
            int actual = Program.Subtract(num1, num2);
            Assert.AreEqual(expected, actual);
        }

    }
}