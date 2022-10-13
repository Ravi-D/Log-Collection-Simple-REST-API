using Microsoft.AspNetCore.Mvc;

namespace LogCollection
{
    public class Constants
    {
        public const string SOURCE_DIRECTORY = "/var/log/";
        public const string DESTINATION_DIRECTORY = "C:/Users/";

        public const int MEMORY_STREAM_SIZE = 10240;
        public const int MEMORY_STREAM_TIMEOUT = 300000;
    }
}
