namespace LogCollection
{
    public class LogRequest
    {
        public string _fullPath { get; set; }
        public string _fileName { get; set; }
        private int? _lines { get; set; }
        private string? _filter { get; set; }

        public LogRequest(string fullPath, string fileName, int? lines, string? filter)
        {
            _fullPath = fullPath;
            _fileName = fileName;
            _lines = lines;
            _filter = filter;
        }

        public string GetFullPath()
        { 
            return _fullPath;
        }
        public string GetFileName()
        {
            return _fileName;
        }
        public int? GetMaxLinesToReturn()
        {
            return _lines;
        }
        public string? GetFilterExpression()
        {
            return _filter;
        }

    }
}