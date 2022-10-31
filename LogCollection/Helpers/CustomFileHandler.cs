using LogCollection.Controllers;
using LogCollection.Interfaces;
using Newtonsoft.Json.Linq;
using System.IO.MemoryMappedFiles;
using System.Text;
using static LogCollection.Constants;

namespace LogCollection.Helpers
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

            bool returnEntireFile = (linesRequested == null && string.IsNullOrWhiteSpace(keyword));
            bool filterRequired = !string.IsNullOrWhiteSpace(keyword);
            bool lineCountRequired = linesRequested > 0;
            bool bothOptionsPresent = (filterRequired && lineCountRequired);

            //Commented lines are a sneak peek of two new "Reverse" StreamReaders that would avoid the List.Add and reverse iteration operations used in this code.
            //using(ReverseTextReader rtr = new ReverseTextReader(fullPath))
            //using(ReverseFileReader rfr = new ReverseFileReader(fullPath))

            StringBuilder resultBuilder = new StringBuilder();
            List<string> linesToPrint = new List<string>();
            
            int linesAdded = 0;
            string? line;
            using (StreamReader sr = new StreamReader(fullPath))
            {
                //Could use _logger.LogTrace here for more granular reporting    
                while ((line = sr.ReadLine()) != null)
                {
                    bool keywordFound = filterRequired && line.Contains(keyword);

                    if (returnEntireFile)
                    {
                        linesToPrint.Add(line + "\n");
                    }

                    if (bothOptionsPresent && line.Contains(keyword))
                    {
                        if (linesAdded < linesRequested)
                        {
                            linesToPrint.Add(line + "\n");
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
                            linesToPrint.Add(line + "\n");
                            linesAdded += 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (keywordFound && !lineCountRequired)
                    {
                        linesToPrint.Add(line + "\n");
                    }
                }
            }

            for (int i = linesToPrint.Count - 1; i > 1; i--)
            {
                resultBuilder.Append(linesToPrint[i]);
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
            //Cut over to StreamReader since 1GB test and MemoryMapping are dormant for now.
            return ProcessRequest(logRequest);

            //string logResult = string.Empty;
            //string fullPath = logRequest.GetFullPath();
            //string fileName = logRequest.GetFileName();
            //int? linesRequested = logRequest.GetMaxLinesToReturn();
            //string? keyword = logRequest.GetSearchTerm();

            //long fileSize = new FileInfo(fullPath).Length;

            //StringBuilder resultBuilder = new StringBuilder();
            //using (MemoryMappedFile mappedFile = MemoryMappedFile.CreateFromFile(fullPath))
            //using (MemoryMappedViewAccessor mapView = mappedFile.CreateViewAccessor(0, fileSize, MemoryMappedFileAccess.Read))
            //using (MemoryMappedViewStream viewStream = mappedFile.CreateViewStream(0, fileSize, MemoryMappedFileAccess.Read))
            //{
            //    for (int i = 0; i < fileSize; i++)
            //    {
            //        //Read one byte at a time until the end of the stream.
            //        int result = viewStream.ReadByte();

            //        if (result == -1)
            //        {
            //            break;
            //        }
            //        resultBuilder.Append((char)result);
            //    }
            //}

            //logResult = resultBuilder.ToString();
            //resultBuilder.Clear();

            //return logResult;
        }
    }
}