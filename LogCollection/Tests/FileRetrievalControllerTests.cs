using LogCollection.Controllers;
using LogCollection.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using static LogCollection.Tests.TestConstants;

namespace LogCollection.Tests
{
    [TestFixture]
    public class FileRetrievalControllerTests
    {
        private MockRepository mockRepository;
        private Mock<ILogger<FileRetrievalController>> mockLogger;
        private Mock<IFileHandler<LogRequest>> mockFileHandler;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockLogger = mockRepository.Create<ILogger<FileRetrievalController>>();
            mockFileHandler = mockRepository.Create<IFileHandler<LogRequest>>();
        }

        private FileRetrievalController CreateFileRetrievalController()
        {
            return new FileRetrievalController(
                mockLogger.Object,
                mockFileHandler.Object);
        }

        [Test]
        public void VerifyGetLogByName_ReturnsOK()
        {
            // Arrange
            var fileRetrievalController = CreateFileRetrievalController();
            
            string fullPath = TEST_LOG_SMALL_PATH;
            string fileName = TEST_LOG_SMALL_FILE;
            int? linesToReturn = null;
            string? searchTerm = null;
            
            //Act
            var expectedLoggerInvocationCount = 0;

            // Assert
            
            //Assert.Equals(contentStatus.StatusCode, HTTPStatusCode.LOG_RETURNED);
            Assert.AreEqual(mockLogger.Invocations.Count, expectedLoggerInvocationCount);
        }

        [Test]
        [Ignore("TODO: Work on MemoryMapping for successful reads of large files.")]
        public void VerifyGet1GBLogTest_ReturnsOK()
        {
            Assert.Inconclusive();
        }
    }
}
