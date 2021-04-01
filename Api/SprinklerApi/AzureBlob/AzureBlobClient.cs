namespace SprinklerApi.AzureBlob
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Configuration;
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using Models;

    public class AzureBlobClient
    {
        private readonly IConfig _config;

        public AzureBlobClient(IConfig config)
        {
            _config = config;
        }

        private CloudBlobContainer GetClient(string containerName)
        {
            var result = CloudStorageAccount.Parse(_config.BlobConnectionString())
                .CreateCloudBlobClient()
                .GetContainerReference(containerName);
            return result;
        }

        public async Task SaveFile(string fileName, string containerName, Stream stream)
        {
            var client = GetClient(containerName);
            var block = client.GetBlockBlobReference(fileName);
            await block.UploadFromStreamAsync(stream);
        }

        public async Task<StandardResponse<string>> RestoreFromCloud(string containerName, string fileName, string saveAsPathAndFileName)
        {
            var result = new StandardResponse<string>();

            var client = GetClient(containerName);
            var block = client.GetBlockBlobReference(fileName);
            if (block.Exists())
            {
                using (var stream = File.OpenWrite(saveAsPathAndFileName))
                {
                    await block.DownloadToStreamAsync(stream);
                }
                result.Payload = $"Restored Successfully";
            }
            else
            {
                result.ValidationMessages = new List<string>()
                {
                    $"Container:{containerName} File:{fileName} does not exist."
                };
            }

            return result;
        }
    }
}