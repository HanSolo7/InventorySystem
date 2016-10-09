using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InventorySystem;
using InventorySystem.Controllers;

namespace InventorySystem.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            InventoryController controller = new InventoryController();

            // Act
            string result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            InventoryController controller = new InventoryController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            InventoryController controller = new InventoryController();

            // Act
            //controller.Post("value");

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            InventoryController controller = new InventoryController();

            // Act
            //controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            InventoryController controller = new InventoryController();

            // Act
            //controller.Delete(5);

            // Assert
        }
    }
}
