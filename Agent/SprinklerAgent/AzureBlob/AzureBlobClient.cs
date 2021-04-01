namespace SprinklerAgent.AzureBlob
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using App.Models;
    using Configuration;
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using Newtonsoft.Json;

    public class AzureBlobClient : IAzureBlobClient
    {
        private readonly IConfig _config;
        private const string _fileName = "schedule.json";

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

        //public async Task<RunSchedule> GetSchedule()
        //{
        //    RunSchedule result = null;
        //    var client = GetClient();
        //    var block = client.GetBlockBlobReference(_fileName);
        //    if (block.Exists())
        //    {
        //        var json = await block.DownloadTextAsync();
        //        if (!string.IsNullOrWhiteSpace(json))
        //        {
        //            result = JsonConvert.DeserializeObject<RunSchedule>(json);
        //        }
        //    }
        //    return result;
        //}

        //public async Task SaveSchedule(RunSchedule runSchedule)
        //{
        //    if (runSchedule != null)
        //    {
        //        var json = JsonConvert.SerializeObject(runSchedule);
        //        var client = GetClient();
        //        var block = client.GetBlockBlobReference(_fileName);
        //        block.DeleteIfExists();
        //        using (var stream = new MemoryStream(Encoding.Default.GetBytes(json), false))
        //        {
        //            await block.UploadFromStreamAsync(stream);
        //        }
        //    }
        //}
    }
}
