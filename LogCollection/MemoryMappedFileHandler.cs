using LogCollection.Interfaces;
using System.IO.MemoryMappedFiles;
using System.Text;
using static LogCollection.Constants;

namespace LogCollection
{
    public class MemoryMappedFileHandler : IFileHandler
    {
        private Guid _correlationId => Guid.NewGuid();
        private MemoryMappedFile _mappedFile;
        private MemoryMappedViewStream _viewStream;
        //private StringBuilder _stringBuilder;

        public MemoryMappedFileHandler() { }
        public MemoryMappedFileHandler(MemoryMappedFile mappedFile, MemoryMappedViewStream viewStream /*StringBuilder stringBuilder*/) 
        {
            _mappedFile = mappedFile;
            _viewStream = viewStream;
            //_stringBuilder = stringBuilder;
        }

        public string RetrieveFileBasedOnFileName(string fileName)
        {
            string logPath = $@"C:\Users\Ravi\Desktop\temp\{fileName}.txt";
            StringBuilder resultBuilder = new StringBuilder();

            using (_mappedFile = MemoryMappedFile.CreateFromFile(logPath))
            using (_viewStream = _mappedFile.CreateViewStream(0, MEMORY_STREAM_SIZE))
            {
                for (int i = 0; i < MEMORY_STREAM_SIZE; i++)
                {
                    //Read byte the stream and move one byte forward until the end of the stream
                    //For the Read N Lines function I may want to read in (1024 * N) lines at a time here?
                    int result = _viewStream.ReadByte();

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

        public string RetrieveLastNFileLinesBasedOnCount(int numEntries)
        {
            throw new NotImplementedException();
        }

        public string RetrieveFilteredFileBasedOnExpression(string filter)
        {
            throw new NotImplementedException();
        }

        public Guid GetCorrelationId()
        {
            return _correlationId;
        }
    }
}
