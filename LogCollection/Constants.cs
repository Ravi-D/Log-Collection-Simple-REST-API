namespace LogCollection
{
    public class Constants
    {
        //To run this application correctly, please update the PROD_SOURCE_DIRECTORY to a location that contains your target log files to read.
        public const string PROD_SOURCE_DIRECTORY = @".../var/log/";
        public const string STG_SOURCE_DIRECTORY = @"C:\Users\Ravi\Desktop\temp\";
        
        public const string TEST_LOG = @"C:\Users\Ravi\Desktop\temp\log.txt";
        public const string TEST_LOG_SMALL = @"C:\Users\Ravi\Desktop\temp\log-small.txt";
        public const string TEST_FILE_1GB = @"C:\Users\Ravi\Desktop\temp\file-big.txt";
        public const string TEST_FILE_NOT_FOUND = @"C:\Users\Ravi\Desktop\temp\DoesNotExist.txt";
        
        public const string EOF_WIN = @"\r\n"; //Environment.NewLine will return correct end of file byte according to host OS type. \n for Unix, \r\n for Windows
        public const string EOF_UNIX = @"\n";

        //Opted against using DEFAULT_LINES, see Issue #3 for details.
        //public const int DEFAULT_LINES = 500;
        public const int DEFAULT_BUFFER = 4096;

        public const int ONE_GB = 1073741824; //Bytes
        public const int MEMORY_STREAM_SIZE = 1024*20;
        public const int MEMORY_STREAM_TIMEOUT = 300000;

        //Errors
        public const string ERR_NOT_FOUND = "File or directory not found.";
    }
}
