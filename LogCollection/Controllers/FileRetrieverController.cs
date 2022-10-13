using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Linq;
using System.Text;
using static LogCollection.Constants;

namespace LogCollection.Controllers
{
    [ApiController]
    [Route("[controller]/api/")]
    public class FileRetrieverController : ControllerBase
    {
        private readonly ILogger<FileRetrieverController> _logger;
        private IFileHandler _fileHandler;

        public FileRetrieverController(ILogger<FileRetrieverController> logger, IFileHandler fileHandler)
        {
            _logger = logger;
            _fileHandler = fileHandler;
        }

        [HttpGet]
        [Route("get-log-by-name")]
        public string GetLogByName(string fileName)
        {
            string logPath = $@"C:\Users\Ravi\Desktop\temp\{fileName}.txt";
            string logResult = String.Empty;

            try
            {
                logResult = _fileHandler.RetrieveFileBasedOnFileName(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"{nameof(GetLogByName)}, {_fileHandler.GetCorrelationId()}");
            }
            return logResult;
        }

        [HttpGet]
        [Route("get-last-n-lines-of-log")]
        public string GetLastNLinesOfLog(int numEntries)
        {
            string logPath = @"C:\Users\Ravi\Desktop\temp\file-big.txt";
            string logResult = String.Empty;

            try
            {
                logResult = _fileHandler.RetrieveLastNFileLinesBasedOnCount(numEntries);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"{nameof(GetLastNLinesOfLog)}, {_fileHandler.GetCorrelationId()}");
            }
            return logResult;
        }

        [HttpGet]
        [Route("get-log-by-keywords")]
        public string GetLogByKeyword (string filter)
        {
            string logPath = @"C:\Users\Ravi\Desktop\temp\file-big.txt";
            string logResult = String.Empty;

            try
            {
                logResult = _fileHandler.RetrieveFilteredFileBasedOnExpression(filter);
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"{nameof(GetLogByKeyword)}, {_fileHandler.GetCorrelationId()}");
            }
            return logResult;
        }

        //May not need ReadLogHelper, as logic is already delegated to FileHandler .
        #region Helper Methods
        private string ReadLogHelper()
        {
            string logPath = "";
            StringBuilder resultAsString = new StringBuilder();
            using (MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(logPath))
            using (MemoryMappedViewStream memoryMappedViewStream = memoryMappedFile.CreateViewStream(0, MEMORY_STREAM_SIZE))
            {
                //memoryMappedViewStream.ReadTimeout = 100;
                //byte[] buffer = new byte[MEMORY_STREAM_SIZE];
                for (int i = 0; i < MEMORY_STREAM_SIZE; i++)
                {
                    //Read byte the stream and move one byte forward until the end of the stream
                    int result = memoryMappedViewStream.ReadByte();

                    if (result == -1)
                    {
                        break;
                    }
                    resultAsString.Append((char)result);
                }
            }
            return resultAsString.ToString();
        }
        #endregion
    }
}

/*
NET types with async methods
Working with files-
JsonSerializer
StreamReader
StreamWriter
XmlReader
XmlWriter
*/