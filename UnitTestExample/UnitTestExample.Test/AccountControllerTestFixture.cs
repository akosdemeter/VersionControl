using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {
        [
            Test,
            TestCase("abcd1234", false),
            TestCase("irf@uni-corvinus", false),
            TestCase("irf.uni-corvinus.hu", false),
            TestCase("irf@uni.corvinus.hu", true)
        ]
        private static void TestValidateEmail(string email, bool expectedResult) {
            //Arrange
            var accountController = new AccountController();
            //Act
            var actualResult = accountController.ValidateEmail(email);
            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [
            Test,
            TestCase("irf@uni.corvinus.hu", "ABCDefgh", false),
            TestCase("irf@uni.corvinus.hu", "ABCDEFGH12", false),
            TestCase("irf@uni.corvinus.hu", "abcdefghg12", false),
            TestCase("irf@uni.corvinus.hu", "ABcd12", false),
            TestCase("irf@uni.corvinus.hu", "ABCDefgh12", true)
        ]
        private static void TestValidatePassword(string email, string password, bool expectedResult) {
            //Arrange
            var accountController = new AccountController();
            //Act
            var actualResult = accountController.Register(email, password);
            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [
            Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "Abcd1234567")
        ]
        public void TestRegisterHappyPath(string email, string password) {
            //Arrange
            var accountController = new AccountController();
            //Act
            var actualResult = accountController.Register(email, password);
            //Assert
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreEqual(Guid.Empty, actualResult.ID);
        }
    }
}
