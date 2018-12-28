using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Models;
using WebAdmin.Services;

namespace WebAdmin.Controllers
{
    public class ReadingController : Controller
    {
        private readonly StoryService StoryServiceInstance;
        private readonly CategoryService CategoryServiceInstance;
        private readonly TagService TagServiceInstance;

        public ReadingController(StoryService StoryServiceInstance, CategoryService CategoryServiceInstance, TagService TagServiceInstance)
        {
            this.StoryServiceInstance = StoryServiceInstance;
            this.CategoryServiceInstance = CategoryServiceInstance;
            this.TagServiceInstance = TagServiceInstance;
        }

        [ViewLayout("_FullPageLayout")]
        public async Task<IActionResult> Index()
        {
            List<StoryAggregate> Stories = await this.StoryServiceInstance.PublicList();
            return View(Stories);
        }

        [ViewLayout("_FullPageLayout")]
        public async Task<IActionResult> Detail(int Id)
        {
            StoryAggregate Story = await this.StoryServiceInstance.PublicDetail(Id);
            return View(Story);
        }
    }
}