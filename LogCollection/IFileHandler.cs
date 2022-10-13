namespace LogCollection
{
    public interface IFileHandler
    {
        public string RetrieveFileBasedOnFileName(string fileName);
        public string RetrieveLastNFileLinesBasedOnCount(int numEntries);
        public string RetrieveFilteredFileBasedOnExpression(string filter);
        public Guid GetCorrelationId();
    }
}