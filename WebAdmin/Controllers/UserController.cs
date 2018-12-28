using Base.APIBuilder;
using Base.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebAdmin.Services;
using WebAdmin.Models;
using WebAdmin.ViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAdmin.Controllers
{
    public class UserController : Controller
    {
        private readonly AccountService AccountServiceInstance;
        public UserController(AccountService AccountServiceInstance)
        {
            this.AccountServiceInstance = AccountServiceInstance;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            try
            {
                var Token = HttpContext.Request.Headers["Token"];
                var Users = await this.AccountServiceInstance.List(Token);
                return View(Users);
            }
            catch (Exception ex)
            {
                if (ex is CustomHttpResponseException)
                {
                    var cex = ex as CustomHttpResponseException;
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                ErrorView Error = new ErrorView();
                Error.HasError = true;
                Error.Message = "Đã có lỗi xảy ra. Vui lòng thử lại sau";
                ViewBag.Error = Error;
                return View();
            }
        }

        public async Task<IActionResult> Detail(string Id)
        {
            try
            {
                var Token = HttpContext.Request.Headers["Token"];
                User User = await this.AccountServiceInstance.UserDetail(Id, Token);
                return View(User);
            }
            catch (Exception ex)
            {
                if (ex is CustomHttpResponseException)
                {
                    var cex = ex as CustomHttpResponseException;
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                ErrorView Error = new ErrorView();
                Error.HasError = true;
                Error.Message = "Đã có lỗi xảy ra. Vui lòng thử lại sau";
                ViewBag.Error = Error;
                return View();
            }
        }

        public IActionResult Create()
        {
            return View(new User());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUser User)
        {
            try
            {
                var Token = HttpContext.Request.Headers["Token"];
                User UserInstance = await this.AccountServiceInstance.Create(User, Token);
                return RedirectToAction("Detail", new { Id = UserInstance.Id });
            }
            catch (Exception ex)
            {
                if (ex is CustomHttpResponseException)
                {
                    CustomHttpResponseException cex = ex as CustomHttpResponseException;
                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(User User)
        {
            try
            {
                var Token = HttpContext.Request.Headers["Token"];
                await this.AccountServiceInstance.Update(User, Token);
                return RedirectToAction("Detail", new { Id = User.Id });
            }
            catch (Exception ex)
            {
                if(ex is CustomHttpResponseException)
                {
                    CustomHttpResponseException cex = ex as CustomHttpResponseException;
                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                throw ex;
            }
        }
    }
}
