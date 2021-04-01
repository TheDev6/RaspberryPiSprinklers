namespace SprinklerApi.Configuration
{
    public class Config : IConfig
    {
        public string AppInsightsCorrelationId() => "";
        public string SqlLiteConnectionString() => "Data Source=sprinkler.db;";
        public string DataLocationRelativeToBin() => "sprinkler.db";
        public string BlobConnectionString() => "";
        public string BlobContainerName() => "raz-sprinklers";
        public string AppVersion() => "1.0.0";
    }
}
