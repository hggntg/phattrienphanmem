using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using System;
using System.IO;
using Base;
using Microsoft.Extensions.Logging;
using Service;
using ViewModel;
using Base.APIBuilder;
using FileService.Business;
using Newtonsoft.Json;

namespace FileService.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly ILogger<FileController> Logger;
        private readonly FileContext FileContextInstance;
        private readonly AccountService AccountServiceInstance;
        private readonly FileBusiness FileBusinessInstance;

        public FileController(ILogger<FileController> Logger, FileContext FileContextInstance, AccountService AccountServiceInstance)
        {
            this.Logger = Logger;
            this.FileContextInstance = FileContextInstance;
            this.AccountServiceInstance = AccountServiceInstance;
            this.FileBusinessInstance = new FileBusiness(FileContextInstance, AccountServiceInstance);
        }

        // POST api/file
        [HttpPost("upload")]
        [DisableFormValueModelBinding]
        public async Task<ActionResult> Upload()
        {
            try
            {
                string Token = HttpContext.Request.Headers["Token"];

                var Folder = "";
                this.EnsureUploadFolder(out Folder);
                var FilePath = Path.Combine(Folder, DateTime.Now.Ticks.ToString());

                Model.File FileInstance = await this.FileBusinessInstance.UploadFile(Request, Token, FilePath);

                var Response = CustomHttpResponse.Single<Model.File>();
                var ResultResponse = new JsonResult(Response);
                ResultResponse.StatusCode = 200;
                ResultResponse.ContentType = "application/json";

                Response.Result = FileInstance;
                return ResultResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("=============================================");
                Console.WriteLine("=============================================");
                Console.WriteLine("=============================================");
                Console.WriteLine(JsonConvert.SerializeObject(ex));
                var Response = CustomHttpResponse.State();
                var ResultResponse = new JsonResult(Response);
                ResultResponse.StatusCode = 400;
                ResultResponse.ContentType = "application/json";

                Response.Status = "BadRequest";
                Response.Message = "Missing token";
                return ResultResponse;
            }
           
        }

        [HttpGet("list-all")]
        public ActionResult ListAll()
        {
            var Folder = "";
            this.EnsureUploadFolder(out Folder);
            return Json(Directory.EnumerateFiles(Folder, "*", SearchOption.AllDirectories));
        }

        [HttpGet("{FileName}")]
        public async Task<ActionResult> GetFile(string FileName)
        {
            FileName = FileName.Split(".")[0];
            Model.File FileInstance = this.FileBusinessInstance.GetFile(FileName);
            if(FileInstance == null)
            {
                return NotFound();
            }
            else
            {
                var Folder = "";
                this.EnsureUploadFolder(out Folder);
                var FilePath = Path.Combine(Folder, FileInstance.Location);
                return File(await System.IO.File.ReadAllBytesAsync(FilePath), "image/jpeg");
            }
        }

        [HttpDelete("clear")]
        public ActionResult Clear()
        {
            var Folder = "";
            this.EnsureUploadFolder(out Folder);
            var stateResponse = CustomHttpResponse.State();
            stateResponse.Message = "All files are clear";
            stateResponse.Status = "Done";
            foreach(var FilePath in Directory.EnumerateFiles(Folder, "*", SearchOption.AllDirectories))
            {
                System.IO.File.Delete(FilePath);
            }
            return Json(stateResponse);
        }

        [HttpGet("user")]
        public async Task<ActionResult> GetUser()
        {
            try
            {
                var Token = HttpContext.Request.Headers["Token"];
                var users = await this.AccountServiceInstance.GetUser(Token);
                var ListResponse = CustomHttpResponse.List<UserView>();
                ListResponse.Length = users.Count;
                ListResponse.Page = 1;
                ListResponse.IsEnd = true;
                ListResponse.RecordPerPage = users.Count;
                ListResponse.Total = users.Count;
                ListResponse.Results = users;
                return Json(ListResponse);
            }
            catch(CustomHttpResponseException ex)
            {
                var StateResponse = CustomHttpResponse.State();
                StateResponse.Status = ex.CustomData.Status;
                StateResponse.Message = ex.CustomData.Message;
                var Response = Json(StateResponse);
                Response.StatusCode = ex.CustomData.StatusCode;
                return Response;
            }
            catch (Exception)
            {
                var StateResponse = CustomHttpResponse.State();
                StateResponse.Status = "Internal Server Error";
                StateResponse.Message = "There is something wrong. Come back later please.";
                var Response = Json(StateResponse);
                Response.StatusCode = 500;
                return Response;
            }
        }

        private void EnsureUploadFolder(out string Folder)
        {
            if (Directory.Exists("Upload"))
            {
                Folder = Path.GetFullPath("Upload");
            }
            else
            {
                Folder = Directory.CreateDirectory("Upload").FullName;
            }
        }
    }
}
