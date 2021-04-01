namespace SprinklerApi.AppLogic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using AzureBlob;
    using Configuration;
    using Models;

    public class DataBackupRestoreLogic
    {
        private readonly AzureBlobClient _azureBlobClient;
        private readonly IConfig _config;
        private readonly string _dbFileName = "sprinklers";
        private readonly string _dbFileExtension = "db";


        public DataBackupRestoreLogic(
            IConfig config,
            AzureBlobClient azureBlobClient)
        {
            _config = config;
            _azureBlobClient = azureBlobClient;
        }

        public async Task<StandardResponse<string>> BackupDb()
        {
            var result = new StandardResponse<string>();
            var spDbFullPath = Path.GetFullPath(_config.DataLocationRelativeToBin());
            if (File.Exists(spDbFullPath))
            {
                using (var stream = File.Open(spDbFullPath, FileMode.Open))
                {

                    var fileName = $"{_dbFileName}_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.{_dbFileExtension}";

                    await _azureBlobClient.SaveFile(
                        fileName: fileName,
                        containerName: _config.BlobContainerName(),
                        stream: stream);

                    result.Payload = fileName;
                }
            }
            else
            {
                result.ValidationMessages = new List<string>() { $"File not found: {spDbFullPath}" };
            }

            return result;
        }

        public async Task<StandardResponse<string>> RestoreFromBlob(string fileName)
        {
            var result = await _azureBlobClient.RestoreFromCloud(
                containerName: _config.BlobContainerName(),
                fileName: fileName,
                saveAsPathAndFileName: Path.GetFullPath(_config.DataLocationRelativeToBin()));
            return result;
        }
    }
}
