using Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineReading.API.Aggregate;
using OnlineReading.API.Business;
using OnlineReading.API.Infrastructure;
using OnlineReading.API.Models;
using OnlineReading.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineReading.API.Controllers
{
    [Route("api/[controller]")]
    public class StoryController : Controller
    {
        private readonly StoryContext StoryContextInstance;
        private readonly StoryBusiness StoryBusinessInstance;
        public StoryController(StoryContext StoryContextInstance, AccountService AccountServiceInstance)
        {
            this.StoryContextInstance = StoryContextInstance;
            this.StoryBusinessInstance = new StoryBusiness(this.StoryContextInstance, AccountServiceInstance);
        }
        public IActionResult Index()
        {
            return NotFound();
        }

        [HttpGet("public-list")]
        public IActionResult PublicList()
        {
            List<StoryAggregate> StoryAggregate = this.StoryBusinessInstance.PublicList();

            var ListResponse = CustomHttpResponse.List<StoryAggregate>();
            var ResultResponse = new JsonResult(ListResponse);
            ResultResponse.StatusCode = 200;
            ResultResponse.ContentType = "application/json";

            ListResponse.IsEnd = true;
            ListResponse.Length = StoryAggregate.Count;
            ListResponse.Page = 1;
            ListResponse.Total = StoryAggregate.Count;
            ListResponse.RecordPerPage = StoryAggregate.Count;
            ListResponse.Results = StoryAggregate;

            return ResultResponse;
        }

        [HttpGet("public-detail/{Id}")]
        public IActionResult PublicDetail(int Id)
        {
            StoryAggregate StoryAggregate = this.StoryBusinessInstance.PublicDetail(Id);

            var Response = CustomHttpResponse.Single<StoryAggregate>();
            var ResultResponse = new JsonResult(Response);
            ResultResponse.StatusCode = 200;
            ResultResponse.ContentType = "application/json";

            Response.Result = StoryAggregate;
            return ResultResponse;
        }

        [HttpGet("list")]
        public IActionResult List()
        {
            try
            {
                string Token = HttpContext.Request.Headers["Token"];

                List<StoryAggregate> StoryAggregate = this.StoryBusinessInstance.List(Token);

                var ListResponse = CustomHttpResponse.List<StoryAggregate>();
                var ResultResponse = new JsonResult(ListResponse);
                ResultResponse.StatusCode = 200;
                ResultResponse.ContentType = "application/json";

                ListResponse.IsEnd = true;
                ListResponse.Length = StoryAggregate.Count;
                ListResponse.Page = 1;
                ListResponse.Total = StoryAggregate.Count;
                ListResponse.RecordPerPage = StoryAggregate.Count;
                ListResponse.Results = StoryAggregate;

                return ResultResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine(ex.Message);

                var Response = CustomHttpResponse.State();
                var ResultResponse = new JsonResult(Response);
                ResultResponse.StatusCode = 400;
                ResultResponse.ContentType = "application/json";

                Response.Status = "BadRequest";
                Response.Message = "Missing token";
                return ResultResponse;
            }
        }

        [HttpGet("detail/{Id}")]
        public IActionResult Detail(int Id)
        {
            try
            {
                string Token = HttpContext.Request.Headers["Token"];

                StoryAggregate StoryAggregate = this.StoryBusinessInstance.Detail(Token, Id);

                var Response = CustomHttpResponse.Single<StoryAggregate>();
                var ResultResponse = new JsonResult(Response);
                ResultResponse.StatusCode = 200;
                ResultResponse.ContentType = "application/json";

                Response.Result = StoryAggregate;
                return ResultResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]StoryAggregate StoryInstance)
        {
            try
            {
                string Token = HttpContext.Request.Headers["Token"];

                StoryAggregate StoryAggregate = await this.StoryBusinessInstance.Create(Token, StoryInstance);

                var Response = CustomHttpResponse.Single<StoryAggregate>();
                var ResultResponse = new JsonResult(Response);
                ResultResponse.StatusCode = 200;
                ResultResponse.ContentType = "application/json";

                Response.Result = StoryAggregate;
                return ResultResponse;
            }
            catch(Exception ex)
            {
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
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

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]StoryAggregate StoryInstance)
        {
            try
            {
                string Token = HttpContext.Request.Headers["Token"];

                StoryAggregate StoryAggregate = this.StoryBusinessInstance.Update(Token, StoryInstance);

                var Response = CustomHttpResponse.Single<StoryAggregate>();
                var ResultResponse = new JsonResult(Response);
                ResultResponse.StatusCode = 200;
                ResultResponse.ContentType = "application/json";

                Response.Result = StoryAggregate;
                return ResultResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
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
    }
}