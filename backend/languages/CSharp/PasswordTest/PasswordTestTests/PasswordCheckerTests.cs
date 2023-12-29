using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class PasswordCheckerTests
    {
        [TestMethod()]
        public void T01_checkPassword_ok()
        {
            string password = "1234567";
            Assert.IsTrue(PasswordChecker.CheckPassword(password));
        }

        [TestMethod()]
        public void T02_checkPassword_invalid()
        {
            Assert.IsTrue(PasswordChecker.CheckPassword("1"));
            Assert.IsTrue(PasswordChecker.CheckPassword("12345"));
            Assert.IsTrue(PasswordChecker.CheckPassword("12345678910"));
        }
    }
}