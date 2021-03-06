﻿using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CesarBmx.Shared.Caching.Extensions;

namespace CesarBmx.Shared.Tests.Caching.Extensions
{
    [TestClass]
    public class CacheExtensionTests
    {
        [TestMethod]
        public void Test_GetCache_WithNamespace()
        {
            //Arrange
            var mockedDistributedCache = new Mock<IDistributedCache>();
            var task = new Task<string>(()=> "Test");


            //Act
            var cachedObject = mockedDistributedCache.Object.GetCache("GetCache", task,10, GetType(), "Param1", "Param2");

            //Assert
            Assert.IsNull(cachedObject.Result);
        }


    }
}
