using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Linq;
using System.Text;

namespace LogCollection.Controllers
{
    [ApiController]
    [Route("[controller]/api/")]
    public class PlaceholderController : ControllerBase
    {
        private readonly ILogger<PlaceholderController> _logger;

        public PlaceholderController(ILogger<PlaceholderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Placeholder")]
        public IEnumerable<Placeholder> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Placeholder
            {
                Date = DateTime.Now,
                Id = 0,
                Guid = Guid.NewGuid(),
            })
            .ToArray();
        }
      
        [HttpGet]
        [Route("RetrieveLogFile")]
        public string GetLog(string fileName)
        {
            int length = 2048 * 10;
            string logPath = @"C:\Users\Ravi\Desktop\temp\file-big.txt";

            StringBuilder resultAsString = new StringBuilder();
            using (MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(logPath))
            using (MemoryMappedViewStream memoryMappedViewStream = memoryMappedFile.CreateViewStream(0, length))
            {
                for (int i = 0; i < length; i++)
                {
                    //Read byte the stream and move one byte forward until the end of the stream
                    int result = memoryMappedViewStream.ReadByte();

                    if (result == -1)
                    {
                        break;
                    }
                    resultAsString.Append((char) result);
                }
            }
            return resultAsString.ToString();
        }
    }
}