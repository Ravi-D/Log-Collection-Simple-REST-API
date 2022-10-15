using Microsoft.AspNetCore.Mvc;

namespace LogCollection
{
    public class Constants
    {
        public const string SOURCE_DIRECTORY = ".../var/log/";
        public const string DESTINATION_DIRECTORY = "C:/Users/";

        public const string TEST_LOG = @"C:\Users\Ravi\Desktop\temp\log.txt";
        public const string TEST_LOG_SMALL = @"C:\Users\Ravi\Desktop\temp\log-small.txt";
        public const string NOT_FOUND = @"C:\Users\Ravi\Desktop\temp\asdjoqwidjw.txt";
        public const string TEST_FILE_1GB = @"C:\Users\Ravi\Desktop\temp\file-big.txt";
        
        //public const string EOF_WIN = @"\r\n"; Environment.NewLine will return correct end of file byte according to host OS type. \n for Unix, \r\n for Windows
        public const string EOF_UNIX = "\n";

        public const int ONE_GB = 1073741824; //Bytes
        public const int MEMORY_STREAM_SIZE = Int32.MaxValue;
        public const int MEMORY_STREAM_TIMEOUT = 300000;
    }
}
