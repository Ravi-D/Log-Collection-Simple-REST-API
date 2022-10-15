using LogCollection.Interfaces;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using static LogCollection.Constants;

namespace LogCollection
{
    public class MemoryMappedFileHandler : IFileHandler<LogRequest>, IDisposable
    {
        private Guid _correlationId => Guid.NewGuid();
        //private MemoryMappedFile _mappedFile;
        //private MemoryMappedViewStream _viewStream;
        private bool _disposedValue;

        public MemoryMappedFileHandler() { }
        //public MemoryMappedFileHandler(MemoryMappedFile mappedFile, MemoryMappedViewStream viewStream) 
        //{
        //    _mappedFile = mappedFile;
        //    _viewStream = viewStream;
        //}

        public string RetrieveFileBasedOnFileName(string fileName)
        {
            string logPath = $@"C:\Users\Ravi\Desktop\temp\{fileName}.txt";
            StringBuilder resultBuilder = new StringBuilder();

            using (MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(logPath))
            using (MemoryMappedViewStream viewStream = memoryMappedFile.CreateViewStream(0, MEMORY_STREAM_SIZE, MemoryMappedFileAccess.Read))//Adding Read access flag to ensure we are not altering the logs.
            {
                for (int i = 0; i < MEMORY_STREAM_SIZE; i++)
                {
                    //Read byte the stream and move one byte forward until the end of the stream
                    //For the Read N Lines function I may want to read in (1024 * N) lines at a time here?
                    int result = viewStream.ReadByte();

                    if (result == -1)
                    {
                        break;
                    }
                    if (resultBuilder.EnsureCapacity(MEMORY_STREAM_SIZE) > i)
                    {
                        resultBuilder.Append((char)result);
                    }
                }
            }
            
            string logResult = resultBuilder.ToString();
            resultBuilder.Clear();//May not need. If a new string builder is being used for each method rather than passing the same one around,
                                  //this one should just get garbage collected anyway?
            return logResult;
        }

        public Guid GetCorrelationId()
        {
            return _correlationId;
        }

        public string ProcessRequest(LogRequest logRequest)
        {
            string logResult;
            
            string fullPath = logRequest.GetFullPath();
            string fileName = logRequest.GetFileName();
            int? lines = logRequest.GetMaxLinesToReturn();
            string? filter = logRequest.GetFilterExpression();


            bool useStreamReader = false;
            int lineCount = 0;
            long fileSize;


            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException();
            }

            fileSize = new FileInfo(fullPath).Length;
            if (/*fileSize < ONE_GB ||*/ lines > 0 || !string.IsNullOrWhiteSpace(filter))
            {
                //StreamReader is preferred for line-by-line counting and keyword filtering, MemoryMap is more performant for larger files.
                useStreamReader = true;
                return ProcessRequestStreamReader(logRequest);
            }
            
            //MemoryMap is extremely fast for larger files, so let's rely on it 
            MemoryMappedFile mappedFile;
            MemoryMappedViewStream viewStream;
            StringBuilder resultBuilder = new StringBuilder();

            try
            {
                using (mappedFile = MemoryMappedFile.CreateFromFile(fullPath))
                using (viewStream = mappedFile.CreateViewStream(0, MEMORY_STREAM_SIZE, MemoryMappedFileAccess.Read))//Adding Read access flag to ensure we are not altering the logs.
                {
                    for (int i = MEMORY_STREAM_SIZE; i > 0; i--)
                    {
                        //Read byte the stream and move one byte forward until the end of the stream
                        //For the Read N Lines function I may want to read in (1024 * N) lines at a time here?
                        int result = viewStream.ReadByte();

                        //string resultString = resultBuilder.ToString(i, 1);                        

                        if (result == -1)
                        {
                            break;
                        }
                        
                        resultBuilder.Append((char)result);
                    }
                }
            }
            finally
            {
            }

            //string logResult = ParseLog(resultBuilder.ToString());
            logResult = resultBuilder.ToString();
            resultBuilder.Clear();//May not need. If a new string builder is being used for each method rather than passing the same one around,
                                  //this one should just get garbage collected anyway?
            //Console.WriteLine(lineEndings);

            return logResult;
        }

        #region Helper Methods
        
        //StreamReader is less performant for larger files, but for the sake of filtering and counting line by line, it's much easier than attempting to read logs byte by byte.
        private static string ProcessRequestStreamReader(LogRequest logRequest)
        {
            string logResult = String.Empty;

            string fullPath = logRequest.GetFullPath();
            string fileName = logRequest.GetFileName();
            int? lines = logRequest.GetMaxLinesToReturn();
            string? filter = logRequest.GetFilterExpression();

            int linesFound = 0;




            foreach (string line in File.ReadLines(fullPath))
            {
                if (line.Contains(filter))
                {
                    logResult += line;
                }
            }

            using (StreamReader streamReader = new StreamReader(fullPath, true))
            {
                for (string line; (line = streamReader.ReadLine()) != null;)
                {
                    line.Contains(filter);
                    logResult += line;
                }
            }

            return "";
        }

        public string ParseLog(string results)
        {
            return "";
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                //_mappedFile.Dispose();
                //_viewStream.Dispose();

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MemoryMappedFileHandler()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
        #endregion
}
