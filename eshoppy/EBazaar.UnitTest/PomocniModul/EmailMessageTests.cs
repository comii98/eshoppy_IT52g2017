using System;
using EShoppy.PomocniModul.Implementacija;
using EShoppy.PomocniModul.Interfejsi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EBazaar.UnitTest.PomocniModul
{
    [TestClass]
    public class EmailMessageTests
    {
        IEmailMessage EmailMessage;
        Mock<IEmailMessage> EmailMessageMock;

        [TestInitialize]
        public void TestInitialize()
        {
            EmailMessage = new EmailMessage();
            EmailMessageMock = new Mock<IEmailMessage>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SendEmail_ToInvalidTest()
        {
            EmailMessage.SendEmail(string.Empty, "Subject", "Body");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SendEmail_BodyInvalidTest()
        {
            EmailMessage.SendEmail("to@email.com", "Subject", string.Empty);
        }
    }
}
