using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Linq;
using static System.IO.File;
using System.Text;
using static LogCollection.Constants;
using LogCollection.Interfaces;

namespace LogCollection.Controllers
{
    [ApiController]
    [Route("[controller]/api/")]
    public class FileRetrieverController : ControllerBase
    {
        private readonly ILogger<FileRetrieverController> _logger;
        private IFileHandler<LogRequest> _fileHandler;

        public FileRetrieverController(ILogger<FileRetrieverController> logger, IFileHandler<LogRequest> fileHandler)
        {
            _logger = logger;
            _fileHandler = fileHandler;
        }

        [HttpGet]
        [Route("get-log-by-name")]
        public string GetLogByName(string fileName, int? lines, string? filter)
        {
            string filePath = SOURCE_DIRECTORY + fileName;
            string logResult = String.Empty;

            try
            {
                logResult = _fileHandler.ProcessRequest(new LogRequest(filePath, fileName, lines, filter));
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"Function: {nameof(GetLogByName)}\n ID: {_fileHandler.GetCorrelationId()}\n Exception: {ex}");
            }
            return logResult;
        }

        #region hide for now


        [HttpGet]
        [Route("TEST")]
        public string TEST()
        {
            string logSmall = "log-small.txt";
            string log = "log.txt";

            string logPathSmall = $@"C:\Users\Ravi\Desktop\temp\{logSmall}";
            string logPath = $@"C:\Users\Ravi\Desktop\temp\{log}";

            int lines = 0;
            string filter = String.Empty;
            string logResult = String.Empty;

            try
            {
                logResult = _fileHandler.ProcessRequest(new LogRequest(logPath, log, lines, filter));
                //logResult = _fileHandler.ProcessRequest(new LogRequest(logPathSmall, logSmall, lines, filter));
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"{nameof(TEST)}, {_fileHandler.GetCorrelationId()}, threw {ex}");
            }
            return logResult;
        }
        //May not need ReadLogHelper, as logic is already delegated to FileHandler .
        #region Helper Methods
        private string ReadLogHelper()
        {
            //string logPath = "";
            StringBuilder resultAsString = new StringBuilder();
            using (MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(TEST_LOG))
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
#endregion