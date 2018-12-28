using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.APIBuilder;
using Base.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAdmin.Models;
using WebAdmin.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAdmin.Controllers
{
    public class StoryController : Controller
    {
        private readonly StoryService StoryServiceInstance;
        private readonly CategoryService CategoryServiceInstance;
        private readonly TagService TagServiceInstance;

        public StoryController(StoryService StoryServiceInstance, CategoryService CategoryServiceInstance, TagService TagServiceInstance)
        {
            this.StoryServiceInstance = StoryServiceInstance;
            this.CategoryServiceInstance = CategoryServiceInstance;
            this.TagServiceInstance = TagServiceInstance;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return NoContent();
        }

        public async Task<IActionResult> List()
        {
            try
            {
                var Token = HttpContext.Request.Headers["Token"];
                List<StoryAggregate> Stories = await this.StoryServiceInstance.List(Token);
                return View("Index", Stories);

            }
            catch (Exception ex)
            {
                if (ex is CustomHttpResponseException)
                {
                    var cex = ex as CustomHttpResponseException;
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex));
                }
                ErrorView Error = new ErrorView();
                Error.HasError = true;
                Error.Message = "Đã có lỗi xảy ra. Vui lòng thử lại sau";
                ViewBag.Error = Error;
                return View("Index");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Update(StoryAggregate Story, int[] Tags)
        {

            var Token = HttpContext.Request.Headers["Token"];

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(JsonConvert.SerializeObject(Story));

            int TagLength = Tags.Length;
            Story.Tags = new List<Tag>();
            for (int i = 0; i < TagLength; i++)
            {
                Tag Tag = await this.TagServiceInstance.Detail(Tags[i], Token);
                Story.Tags.Add(Tag);
            }
            try
            {
                StoryAggregate StoryInstance = Story;
                return RedirectToAction("Detail", new { Id = StoryInstance.StoryInfo.Id });
            }
            catch (Exception ex)
            {
                if (ex is CustomHttpResponseException)
                {
                    var cex = ex as CustomHttpResponseException;
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex));
                }
                ErrorView Error = new ErrorView();
                Error.HasError = true;
                Error.Message = "Đã có lỗi xảy ra. Vui lòng thử lại sau";
                ViewBag.Error = Error;
                return RedirectToAction("Detail", new { Id = Story.StoryInfo.Id });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(StoryAggregate Story, int[] Tags)
        {

            var Token = HttpContext.Request.Headers["Token"];

            int TagLength = Tags.Length;
            for (int i = 0; i < TagLength; i++)
            {
                Tag Tag = await this.TagServiceInstance.Detail(Tags[i], Token);
                Story.Tags.Add(Tag);
            }
            try
            {
                ViewBag.Tags = await this.TagServiceInstance.List(new List<int>(), Token);
                StoryAggregate StoryInstance = await this.StoryServiceInstance.Create(Token, Story);
                return RedirectToAction("Detail", new { Id = StoryInstance.StoryInfo.Id });
            }
            catch (Exception ex)
            {
                if (ex is CustomHttpResponseException)
                {
                    var cex = ex as CustomHttpResponseException;
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex));
                }
                ErrorView Error = new ErrorView();
                Error.HasError = true;
                Error.Message = "Đã có lỗi xảy ra. Vui lòng thử lại sau";
                ViewBag.Error = Error;
                return View();
            }
        }

        public async Task<IActionResult> Detail(int Id)
        {
            try
            {
                var Token = HttpContext.Request.Headers["Token"];
                StoryAggregate Story = await this.StoryServiceInstance.Detail(Token, Id);

                List<Tag> Tags = await this.TagServiceInstance.List(new List<int>(), Token);
                ViewBag.Tags = Tags;

                List<int> SelectedTags = new List<int>();
                Story.Tags.ToList().ForEach(Tag =>
                {
                    SelectedTags.Add(Tag.Id);
                });

                ViewBag.SelectedTags = SelectedTags;

                return View(Story);
            }
            catch (Exception ex)
            {
                if (ex is CustomHttpResponseException)
                {
                    var cex = ex as CustomHttpResponseException;
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex));
                }
                ErrorView Error = new ErrorView();
                Error.HasError = true;
                Error.Message = "Đã có lỗi xảy ra. Vui lòng thử lại sau";
                ViewBag.Error = Error;
                return View();
            }
        }

        public async Task<IActionResult> Create([FromQuery(Name = "cate")]int Cate)
        {
            try
            {
                var Token = HttpContext.Request.Headers["Token"];
                StoryAggregate Story = new StoryAggregate();

                Story.StoryInfo.CategoryId = Cate;

                List<Tag> Tags = await this.TagServiceInstance.List(new List<int>(), Token);
                ViewBag.Tags = Tags;

                return View(Story);
            }
            catch (Exception ex)
            {
                if (ex is CustomHttpResponseException)
                {
                    var cex = ex as CustomHttpResponseException;
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex));
                }
                ErrorView Error = new ErrorView();
                Error.HasError = true;
                Error.Message = "Đã có lỗi xảy ra. Vui lòng thử lại sau";
                ViewBag.Error = Error;
                return View();
            }
        }

        public async Task<IActionResult> ChooseCategory()
        {
            var Token = HttpContext.Request.Headers["Token"];
            List<Category> Categories = await this.CategoryServiceInstance.List(new List<int>(), Token);
            return View(Categories);
        }
    }
}
