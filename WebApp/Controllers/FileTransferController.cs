using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApp.Filters;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class FileTransferController : ApiController
    {
        private static readonly string ServerUploadFolder = HttpContext.Current.Server.MapPath("/Web/App_Data/UploadedFiles");

        //[Route("files")]
        [ActionName("UploadFile")]
        [HttpPost]
        [ValidateMimeMultipartContentFilter]
        public async Task<FileResult> UploadSingleFile()
        {
            try
            {
                var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                return new FileResult
                {
                    FileNames = streamProvider.FileData.Select(entry => entry.LocalFileName),
                    Names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName),
                    ContentTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType),
                    Description = streamProvider.FormData["description"],
                    CreatedTimestamp = DateTime.UtcNow,
                    UpdatedTimestamp = DateTime.UtcNow,
                    DownloadLink = "TODO, will implement when file is persisted"
                };
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
                return null;
            }
        }

        // GET: api/FileTransfer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/FileTransfer/5
        public string Get(int id)
        {
            return ServerUploadFolder;
        }

        // POST: api/FileTransfer
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/FileTransfer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FileTransfer/5
        public void Delete(int id)
        {
        }
    }
}
