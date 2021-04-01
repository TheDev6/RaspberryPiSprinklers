namespace SprinklerApi.Configuration
{
    public interface IConfig
    {
        string AppInsightsCorrelationId();
        string SqlLiteConnectionString();
        string DataLocationRelativeToBin();
        string BlobConnectionString();
        string BlobContainerName();
        string AppVersion();
    }
}
