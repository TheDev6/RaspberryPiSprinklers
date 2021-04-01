namespace SprinklerApi.Controllers
{
    using System.Threading.Tasks;
    using AppLogic;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    [ApiController]
    public class DataBackupController : ControllerBase
    {
        private readonly DataBackupRestoreLogic _backupRestoreLogic;

        public DataBackupController(DataBackupRestoreLogic backupRestoreLogic)
        {
            _backupRestoreLogic = backupRestoreLogic;
        }

        [HttpGet]
        [Route("Backup")]
        public async Task<StandardResponse<string>> Backup()
        {
            var result = await _backupRestoreLogic.BackupDb();
            Response.StatusCode = result.StatusCode;
            return result;
        }

        [HttpPost]
        [Route("Restore")]
        public async Task<StandardResponse<string>> Restore(string filename)
        {
            var result = await _backupRestoreLogic.RestoreFromBlob(filename);
            Response.StatusCode = result.StatusCode;
            return result;
        }
    }
}