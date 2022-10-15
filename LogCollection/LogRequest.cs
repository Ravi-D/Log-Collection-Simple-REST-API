namespace LogCollection
{
    public class LogRequest
    {
        private string _fullPath { get; set; }
        private string _fileName { get; set; }
        private int? _linesToReturn { get; set; }
        private string? _searchTerm { get; set; }

        public LogRequest(string fullPath, string fileName, int? linesToReturn, string? searchTerm)
        {
            _fullPath = fullPath;
            _fileName = fileName;
            _linesToReturn = linesToReturn;
            _searchTerm = searchTerm;
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
            return _linesToReturn;
        }
        public string? GetSearchTerm()
        {
            return _searchTerm;
        }

    }
}