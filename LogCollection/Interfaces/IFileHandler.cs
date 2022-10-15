namespace LogCollection.Interfaces
{
    public interface IFileHandler<LogRequest>
    {
        public string ProcessRequest(LogRequest logRequest);
        public string ProcessRequest_1GB_Test(LogRequest logRequest);
        public Guid GetCorrelationId();
    }
}