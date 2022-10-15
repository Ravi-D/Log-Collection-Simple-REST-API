using LogCollection.Interfaces;
using System.IO.MemoryMappedFiles;
using System.Text;
using static LogCollection.Constants;

namespace LogCollection
{
    public class CustomFileHandler : IFileHandler<LogRequest>
    {
        private Guid _correlationId => Guid.NewGuid();

        public CustomFileHandler() { }

        public Guid GetCorrelationId()
        {
            return _correlationId;
        }

        /// <summary>
        /// Because we are performing sequential file operations, StreamReader provides a significantly faster approach over MemoryMapping. 
        /// This also makes filtering and line counting much easier.
        /// Depending on your machine's RAM, this function will not have enough memory to process large files (>1GB). The rampup in RAM usage is quick, and sustained peak is much higher compared to MemoryMap.
        /// <param name="logRequest"></param>
        /// <returns>string result of log data, optionally filtered by keyword and restricted to a certain line count.</returns>
        public string ProcessRequest(LogRequest logRequest)
        {
            string logResult = String.Empty;
            string fullPath = logRequest.GetFullPath();
            string fileName = logRequest.GetFileName();
            int? linesRequested = logRequest.GetMaxLinesToReturn();
            string? keyword = logRequest.GetSearchTerm();

            long fileSize = new FileInfo(fullPath).Length;

            if (linesRequested <= 0)
            {
                return String.Empty;
            }

            bool test = string.IsNullOrWhiteSpace(keyword);

            bool returnEntireFile = ((linesRequested == null || linesRequested > fileSize) && string.IsNullOrWhiteSpace(keyword));
            bool filterRequired = !string.IsNullOrWhiteSpace(keyword);
            bool lineCountRequired = linesRequested > 0;
            bool bothOptionsPresent = (filterRequired && lineCountRequired);

            int linesAdded = 0;
            StringBuilder resultBuilder = new StringBuilder();

            //Reading the file backwards would improve this operation. Currently it's a pretty heavy O(N) since we're reading each line and reversing the order of each row.
            //Decided to be more explicit 
            foreach (string line in File.ReadLines(fullPath).Reverse())
            {
                bool keywordFound = (filterRequired && line.Contains(keyword));

                if (returnEntireFile)
                { 
                    resultBuilder.Append(line + "\n");
                }

                if (bothOptionsPresent && line.Contains(keyword))
                {
                    if (linesAdded < linesRequested)
                    {
                        resultBuilder.Append(line + "\n");
                        linesAdded += 1;
                    }
                    else
                    {
                        break; 
                    }
                }

                if (lineCountRequired && !filterRequired)
                {
                    if (linesAdded < linesRequested)
                    {
                        resultBuilder.Append(line + "\n"); 
                        linesAdded += 1;
                    } 
                    else 
                    { 
                        break; 
                    } 
                }
                
                if (keywordFound && !lineCountRequired)
                {
                    resultBuilder.Append(line + "\n");
                }
            }

            logResult = resultBuilder.ToString();
            resultBuilder.Clear();

            return logResult;

        }

        /// <summary>
        /// MemoryMapping is superior for randomly accessing sub sections of massive files.
        /// My implementation below treated the entire file as a single subset...
        /// Depending on your machine's RAM, this function will time out eventually on large files. RAM consumption gradually increases compared to StreamReader's sharp increase, sustained peak, and eventually noisy exception.
        /// </summary>
        /// <param name="logRequest"></param>
        /// <returns>string result of log data, optionally filtered by keyword and restricted to a certain line count.</returns>
        public string ProcessRequest_1GB_Test(LogRequest logRequest)
        {

            string logResult = String.Empty;
            string fullPath = logRequest.GetFullPath();
            string fileName = logRequest.GetFileName();
            int? linesRequested = logRequest.GetMaxLinesToReturn();
            string? keyword = logRequest.GetSearchTerm();

            long fileSize = new FileInfo(fullPath).Length;

            StringBuilder resultBuilder = new StringBuilder();
            using (MemoryMappedFile mappedFile = MemoryMappedFile.CreateFromFile(fullPath))
            using (MemoryMappedViewAccessor mapView = mappedFile.CreateViewAccessor(0, fileSize, MemoryMappedFileAccess.Read))
            using (MemoryMappedViewStream viewStream = mappedFile.CreateViewStream(0, fileSize, MemoryMappedFileAccess.Read))
            {
                for (int i = 0; i < fileSize; i++)
                {
                    //Read one byte at a time until the end of the stream.
                    int result = viewStream.ReadByte();

                    if (result == -1)
                    {
                        break;
                    }
                    resultBuilder.Append((char)result);
                }
            }

            logResult = resultBuilder.ToString();
            resultBuilder.Clear();

            return logResult;
        }
    }
}