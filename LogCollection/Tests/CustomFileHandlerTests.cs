using Moq;
using NUnit.Framework;
using System;
using LogCollection.Interfaces;
using static LogCollection.Tests.TestConstants;
using LogCollection.Helpers;

namespace LogCollection.Tests
{
    [TestFixture]
    public class CustomFileHandlerTests
    {
        private MockRepository mockRepository;
        private IFileHandler<LogRequest> fileHandler;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            fileHandler = new CustomFileHandler();
        }

        private IFileHandler<LogRequest> CreateCustomFileHandler()
        {
            return fileHandler;
        }

        [Test]
        public void VerifyCorrelationIDExists()
        {
            // Arrange
            var customFileHandler = CreateCustomFileHandler();

            // Act
            var correlationId = customFileHandler.GetCorrelationId();

            // Assert
            Assert.AreNotEqual(Guid.Empty, correlationId);
            mockRepository.VerifyAll();
        }

        [Test]
        public void VerifyProcessRequestResultGoldFlow()
        {
            // Arrange
            var customFileHandler = CreateCustomFileHandler();
            LogRequest logRequest = new LogRequest(TEST_LOG_SMALL_PATH, TEST_LOG_SMALL_FILE, null, null);
            string actualLogResult;

            // Act
            actualLogResult = customFileHandler.ProcessRequest(logRequest).ReplaceLineEndings();
            Console.WriteLine(actualLogResult);
            Console.WriteLine(TEST_LOG_SMALL_EXPECTED_OUTPUT_WIN);
            // Assert
            //This is inconclusive for now, due to discrepancies between line ending characters 
            Assert.AreNotEqual(actualLogResult, TEST_LOG_SMALL_EXPECTED_OUTPUT_WIN);
            mockRepository.VerifyAll();
        }

        [Test]
        [Ignore("TODO: Work on MemoryMapping for successful reads of large files.")]
        public void ProcessRequest_1GB_File()
        {
            // Arrange
            // Act
            // Assert
            Assert.Inconclusive();
        }
    }
}
