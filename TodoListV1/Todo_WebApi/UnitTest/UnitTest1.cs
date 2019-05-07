using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using DataRepository.Models;
using Microsoft.EntityFrameworkCore;
using DataRepository;
using TodoWebAPI.Controllers;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Prepare
            var mock = new Mock<IEventRepository>();
            mock.Setup(p => p.GetEventById(1)).Returns("Meeting");

            //Act
            EventsController eventsController = new EventsController(mock.Object);
            string result = eventsController.GetEventById(1);

            //Assert
            Assert.AreEqual("Meeting", result);
        }
    }
}
