namespace LogCollection.Interfaces
{
    public interface IFileHandler<LogRequest>
    {
        public string ProcessRequest(LogRequest logRequest);//LogRequest logRequest


       // public string ReadFile(LogRequest logRequest);//LogRequest logRequest

        public Guid GetCorrelationId();
    }
}