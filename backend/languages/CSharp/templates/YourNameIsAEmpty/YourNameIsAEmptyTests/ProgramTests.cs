using Microsoft.VisualStudio.TestTools.UnitTesting;
using YourNameIsAEmpty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourNameIsAEmpty.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void YourNameIsATest()
        {
            Program program = new Program();
            Assert.AreEqual("Your name is A", program.YourNameIsA());
        }
    }
}