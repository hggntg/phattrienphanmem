using System.Collections.Generic;
using System.Linq;
using Base;
using Microsoft.AspNetCore.Mvc;
using OnlineReading.API.Business;
using OnlineReading.API.Infrastructure;
using OnlineReading.API.Models;

namespace OnlineReading.API.Controllers
{
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private readonly StoryContext StoryContextInstance;
        private readonly TagBusiness TagBusinessInstance;
        public TagController(StoryContext StoryContextInstance)
        {
            this.StoryContextInstance = StoryContextInstance;
            this.TagBusinessInstance = new TagBusiness(this.StoryContextInstance);
        }

        [HttpGet("detail/{Id}")]
        public ActionResult Detail(int Id)
        {
            var SingleResponse = CustomHttpResponse.Single<Tag>();
            JsonResult ResultResponse = new JsonResult(SingleResponse);
            ResultResponse.ContentType = "application/json";
            ResultResponse.StatusCode = 200;

            SingleResponse.Result = this.TagBusinessInstance.Detail(Id);

            return ResultResponse;
        }

        [HttpGet("list")]
        public ActionResult List([FromQuery(Name = "ids")]string Ids)
        {
            List<int> IdArray = new List<int>();

            if (Ids != null && Ids != "")
            {
                Ids.Split(",").ToList().ForEach(Id =>
                {
                    IdArray.Add(int.Parse(Id));
                });
            }

            var ListResponse = CustomHttpResponse.List<Tag>();
            var ResultResponse = new JsonResult(ListResponse);
            ResultResponse.ContentType = "application/json";
            ResultResponse.StatusCode = 200;

            ListResponse.IsEnd = true;
            ListResponse.Results = this.TagBusinessInstance.List(IdArray);
            ListResponse.Length = ListResponse.Results.Count;
            ListResponse.RecordPerPage = ListResponse.Results.Count;
            ListResponse.Page = 1;
            ListResponse.Total = ListResponse.Results.Count;

            return ResultResponse;
        }

        [HttpPost("initiate")]
        public ActionResult Initiate()
        {
            var ListResponse = CustomHttpResponse.List<Tag>();
            var ResultResponse = new JsonResult(ListResponse);
            ResultResponse.ContentType = "application/json";
            ResultResponse.StatusCode = 200;

            ListResponse.IsEnd = true;
            ListResponse.Results = this.TagBusinessInstance.Initiate();
            ListResponse.Length = ListResponse.Results.Count;
            ListResponse.RecordPerPage = ListResponse.Results.Count;
            ListResponse.Page = 1;
            ListResponse.Total = ListResponse.Results.Count;

            return ResultResponse;
        }
    }
}