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

        public string ProcessRequest(LogRequest logRequest)
        {
            string logResult;
            bool useStreamReader = true;

            string fullPath = logRequest.GetFullPath();
            string fileName = logRequest.GetFileName();
            int? lines = logRequest.GetMaxLinesToReturn();
            string? filter = logRequest.GetSearchTerm();

            long fileSize = new FileInfo(fullPath).Length;
            if (useStreamReader)
            {
                return ProcessRequestStreamReader(logRequest);
            }

            //TODO Future Work below - How can we get memory mapped file I/O to satisfy this assessment's constraints?
            //1. Reading byte by byte means we have to take responsibility to reliably find every end of line character and append all current characters in the line to a string/list.
            //2. How can we safely manage memory and buffer sizes for many different file size input possibilities without resorting to arbitrarily modifying MEMORY_STREAM_SIZE?
            //3. We run into a lot of UnauthorizedAccess exceptions here likely due to the discrepancy in size betwen the MEMORY_STREAM and how small the file actually is.
            //4. All in all, I found that the memory map was extremely performant for simply reading and printing out large file content when logical operations (filtering, ordering by descending date etc) were not a factor.

            //MemoryMappedFile

            StringBuilder resultBuilder = new StringBuilder();
            List<string> resultList = new List<string>();

            using (MemoryMappedFile mappedFile = MemoryMappedFile.CreateFromFile(TEST_FILE_1GB))
            using (MemoryMappedViewAccessor mapView = mappedFile.CreateViewAccessor(0, MEMORY_STREAM_SIZE, MemoryMappedFileAccess.Read))
            using (MemoryMappedViewStream viewStream = mappedFile.CreateViewStream(0, MEMORY_STREAM_SIZE, MemoryMappedFileAccess.Read))
            {
                for (int i = 0; i < MEMORY_STREAM_SIZE; i++)
                {
                    //Read one byte at a time until the end of the stream.
                    int result = viewStream.ReadByte();

                    if (result == -1)
                    {
                        break;
                    }

                    resultBuilder.Append((char)result);

                    //Scanning for end of line
                    if (i >= 1 && resultBuilder[i].ToString() + resultBuilder[i - 1].ToString() == EOF_UNIX) ;
                    {
                        resultList.Add(resultBuilder.ToString());
                    }
                }
            }

            logResult = resultBuilder.ToString();
            resultBuilder.Clear();

            return logResult;
            
        }

        /// <summary>
        /// File operations via StreamReader line by line is less performant than a MemoryMap, but much easier to work with logically. At least we're not doing ReadToEnd().
        /// </summary>
        /// <param name="logRequest"></param>
        /// <returns></returns>
        private static string ProcessRequestStreamReader(LogRequest logRequest)
        {
            string logResult = String.Empty;
            string fullPath = logRequest.GetFullPath();
            string fileName = logRequest.GetFileName();
            int? linesRequested = logRequest.GetMaxLinesToReturn();
            string? keyword = logRequest.GetSearchTerm();

            if (linesRequested <= 0)
            {
                return String.Empty;
            }

            bool standardProcessing = (linesRequested == null && string.IsNullOrWhiteSpace(keyword));
            bool filterRequired = (linesRequested == null && !string.IsNullOrWhiteSpace(keyword));
            bool lineCountRequired = (linesRequested > 0);
            
            bool bothOptionsPresent = (filterRequired && lineCountRequired);


            int linesAdded = 0;
            StringBuilder resultBuilder = new StringBuilder();
            
            //Reading the file backwards would improve this operation. Currently it's a pretty heavy O(N) since we're reading each line and reversing the order of each row.
            foreach (string line in File.ReadLines(fullPath).Reverse())
            {
                if (lineCountRequired)
                {
                    while (linesAdded < linesRequested)
                    {
                        if (filterRequired && line.Contains(keyword))
                        {
                            resultBuilder.Append(line);
                        }
                        linesAdded++;
                    }
                }
                else
                {
                    if (filterRequired && line.Contains(keyword))
                    {
                        resultBuilder.Append(line);
                    }
                }
            }
            
            logResult = resultBuilder.ToString();
            resultBuilder.Clear();

            return logResult;
        }
    }
}