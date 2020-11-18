using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UnitTestExample.Abstractions;
using UnitTestExample.Controllers;
using UnitTestExample.Entities;

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
            var accountServiceMock = new Mock<IAccountManager>(MockBehavior.Strict);
            accountServiceMock.Setup(m => m.CreateAccount(It.IsAny<Account>())).Returns<Account>(a => a);
            var accountController = new AccountController();
            accountController.AccountManager = accountServiceMock.Object;
            //Act
            var actualResult = accountController.Register(email, password);
            //Assert
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreEqual(Guid.Empty, actualResult.ID);
            accountServiceMock.Verify(m => m.CreateAccount(actualResult), Times.Once);
        }

        [
            Test,
            TestCase("irf@uni-corvinus", "Abcd1234"),
            TestCase("irf.uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "abcd1234"),
            TestCase("irf@uni-corvinus.hu", "ABCD1234"),
            TestCase("irf@uni-corvinus.hu", "abcdABCD"),
            TestCase("irf@uni-corvinus.hu", "Ab1234"),
        ]
        public void TestRegisterValidateException(string email, string password) {
            //Arrange
            var accountController = new AccountController();
            //Act
            try
            {
                var actualResult = accountController.Register(email, password);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<ValidationException>(ex);
            }
            //Assert
        }

        [
            Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234")
        ]
        public void TestRegisterApplicationException(string newEmail, string newPassword)
        {
            // Arrange
            var accountServiceMock = new Mock<IAccountManager>(MockBehavior.Strict);
            accountServiceMock
                .Setup(m => m.CreateAccount(It.IsAny<Account>()))
                .Throws<ApplicationException>();
            var accountController = new AccountController();
            accountController.AccountManager = accountServiceMock.Object;

            // Act
            try
            {
                var actualResult = accountController.Register(newEmail, newPassword);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<ApplicationException>(ex);
            }

            // Assert
        }
    }
}
