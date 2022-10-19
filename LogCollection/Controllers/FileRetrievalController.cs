using Microsoft.AspNetCore.Mvc;
using static LogCollection.Constants;
using LogCollection.Interfaces;

namespace LogCollection.Controllers
{
    [ApiController]
    [Route("[controller]/api/")]
    public class FileRetrievalController : ControllerBase
    {
        private readonly ILogger<FileRetrievalController> _logger;
        private IFileHandler<LogRequest> _fileHandler;

        public FileRetrievalController(ILogger<FileRetrievalController> logger, IFileHandler<LogRequest> fileHandler)
        {
            _logger = logger;
            _fileHandler = fileHandler;
        }

        /// <summary>
        /// Allow user to send a GET request for the contents of a specific log file. Optionally, they can filter the result set with a max line limit and/or keyword expression.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="linesToReturn"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-log-by-name")]
        public ContentResult GetLogByName(string fileName, int? linesToReturn, string? searchTerm)
        {
            string fullPath = PROD_SOURCE_DIRECTORY + fileName;
            string logResult = String.Empty;

            try
            {
                logResult = _fileHandler.ProcessRequest(new LogRequest(fullPath, fileName, linesToReturn, searchTerm));
            }

            //TODO Future Work - Add more intentional error handling/custom exceptions
            //FileNotFound, UnauthorizedAccess, IndexOutOfBounds etc are all caught by the file handler rather than being explicitly checked for and handled that way.
            //I wanted to minimize the work explicitly being done in this function. Let the _fileHandler deal with it while our GET just prints our logfile results.
            catch (Exception ex)
            {
                _logger.LogTrace($"Function: {nameof(GetLogByName)}\n ID: {_fileHandler.GetCorrelationId()}\n Exception: {ex}");
                Console.WriteLine(ex.Message);
                HttpContext.Response.StatusCode = 404;

                return new ContentResult() { Content = ex.Message, StatusCode = HttpContext.Response.StatusCode };
            }
            
            Console.WriteLine(logResult);
            return new ContentResult() { Content = logResult, StatusCode = HttpContext.Response.StatusCode };
        }

        /// <summary>
        /// Allow user to send a GET request without parameters to test performant MemoryMapped file operations on a 1 gigabyte file. No filtering or line counting is performed.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="linesToReturn"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/perftest/get-1gb-log")]
        public ContentResult Get1GBLogTest()
        {   
            string fullPath = TEST_FILE_1GB;
            string logResult = String.Empty;

            try
            {
                logResult = _fileHandler.ProcessRequest_1GB_Test(new LogRequest(fullPath, "file-big.txt", null, null));
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"Function: {nameof(Get1GBLogTest)}\n ID: {_fileHandler.GetCorrelationId()}\n Exception: {ex}");
                Console.WriteLine(ex.Message);
                HttpContext.Response.StatusCode = 500;
                
                return new ContentResult() { Content = ex.Message, StatusCode = HttpContext.Response.StatusCode };
            }
            
            Console.WriteLine(logResult);
            return new ContentResult() { Content = logResult, StatusCode = HttpContext.Response.StatusCode };
        }
    }
}