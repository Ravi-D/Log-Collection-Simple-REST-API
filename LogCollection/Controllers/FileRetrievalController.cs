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
        /// Allow user to send a GET request for the contents of a specific log file. Optionally, they can reduce the result set with a max line count and/or keyword filter.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="linesToReturn"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-log-by-name")]
        public string GetLogByName(string fileName, int? linesToReturn, string? searchTerm)
        {
            //string filePath = SOURCE_DIRECTORY + fileName;
            string fullPath = STG_SOURCE_DIRECTORY + fileName;
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
            }
            return logResult;
        }
    }
}