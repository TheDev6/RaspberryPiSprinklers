namespace Jobz.WebUi.AzureBlob
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using Models.SprinklerAgentTypes;
    using Newtonsoft.Json;
    using RootContracts.BehaviorContracts.Configuration;

    public class AzureBlobClient
    {
        private const string _fileName = "schedule.json";
        private readonly IConfig _config;

        public AzureBlobClient(IConfig config)
        {
            _config = config;
        }

        private CloudBlobContainer GetClient()
        {
            var result = CloudStorageAccount.Parse(_config.BlobConnectionString())
                .CreateCloudBlobClient()
                .GetContainerReference(_config.BlobContainerName());
            return result;
        }

        public async Task<RunSchedule> GetSchedule()
        {
            var result = new RunSchedule();
            var client = GetClient();
            var block = client.GetBlockBlobReference(_fileName);
            if (block.Exists())
            {
                var json = await block.DownloadTextAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    result = JsonConvert.DeserializeObject<RunSchedule>(json);
                }
            }
            return result;
        }

        public async Task SaveSchedule(RunSchedule runSchedule)
        {
            if (runSchedule != null)
            {
                var json = JsonConvert.SerializeObject(runSchedule);
                var client = GetClient();
                var block = client.GetBlockBlobReference(_fileName);
                block.DeleteIfExists();
                using (var stream = new MemoryStream(Encoding.Default.GetBytes(json), false))
                {
                    await block.UploadFromStreamAsync(stream);
                }
            }
        }
    }
}