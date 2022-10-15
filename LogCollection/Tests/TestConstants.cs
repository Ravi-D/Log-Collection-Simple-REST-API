namespace LogCollection.Tests
{
    public class TestConstants
    {
        public const string TEST_LOG_SMALL_PATH = @"Z:\Documents\Visual Studio 2022\Projects\xxxx\LogCollection\LogCollection\Tests\TEST_LOG_SMALL.txt";
        public const string TEST_LOG_SMALL_FILE = "TEST_LOG_SMALL.txt";

        public string TEST_LOG_PATH = @"Z:\Documents\Visual Studio 2022\Projects\xxxx\LogCollection\LogCollection\Tests\TEST_LOG.txt";
        public string TEST_LOG_FILE = "TEST_LOG.txt";

        public string LOG_LINE_1 = $"[{DateTime.UtcNow.ToLongTimeString()}] The quick brown fox jumped over the lazy dog.\n";
        public string LOG_LINE_2 = $"[{DateTime.UtcNow.AddMilliseconds(1).ToLongTimeString()}] The quick brown fox jumped over the lazy dog.\n";

        public enum HTTPStatusCode 
        {
            LOG_RETURNED = 200,
        }
        public const string TEST_LOG_SMALL_EXPECTED_OUTPUT_WIN = @"[2022-10-15 21:55:13] Processing tool9 list from AppID 8913920
[2022-10-14 21:55:12] Processing tool1 list from AppID 8913920
[2022-10-13 21:54:14] Processing tool1 list from AppID 8913919
[2022-10-12 21:53:16] Processing tool2 list from AppID 8913928
[2022-10-11 21:52:18] Processing tool6 list from AppID 8913937
[2022-10-10 21:51:20] Processing tool5 list from AppID 8913946
[2022-10-09 21:59:22] Processing tool4 list from AppID 8913955
[2022-10-08 21:58:24] Processing tool3 list from AppID 8913964
[2022-10-07 21:57:26] Processing tool3 list from AppID 8913973
[2022-10-06 21:56:28] Processing tool2 list from AppID 8913982
[2022-10-05 21:55:30] Processing tool1 list from AppID 8913991
[2022-10-04 21:54:32] Ignoring tool steamlinuxruntime as it's for a different target platform linux.";


    }
}