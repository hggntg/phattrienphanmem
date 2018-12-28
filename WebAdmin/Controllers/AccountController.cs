using System;
using System.Threading.Tasks;
using Base;
using Base.Model;
using Base.APIBuilder;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Models;
using WebAdmin.Services;
using Newtonsoft.Json;
using System.Text;
using System.IO;

namespace WebAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService AuthServiceInstance;
        private readonly AccountService AccountServiceInstance;
        public AccountController(AuthService AuthServiceInstance, AccountService AccountServiceInstance)
        {
            this.AuthServiceInstance = AuthServiceInstance;
            this.AccountServiceInstance = AccountServiceInstance;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [ViewLayout("_FullPageLayout")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ViewLayout("_FullPageLayout")]
        public async Task<IActionResult> Login(LoginModel LoginInstance)
        {
            try
            {
                var key = "a00bc" + DateTime.Now.Ticks.ToString() + "ztmmn01" + DateTime.Now.Ticks.ToString();

                AuthModel AuthInstance = await this.AuthServiceInstance.Login(LoginInstance);
                UserAggregate User = new UserAggregate();
                User.AccessToken = AuthInstance.AccessToken;

                User UserInstance = await this.AccountServiceInstance.Detail("Bearer " + AuthInstance.AccessToken);
                User.UserInfo = UserInstance;
                string UserString = "";
                var Serialize = new JsonSerializer();
                using (StringWriter Writer = new Utf8StringWriter())
                {
                    Serialize.Serialize(Writer, User);
                    UserString = Writer.ToString();
                }
                Console.WriteLine(UserString);
                TempData.Add(key, UserString);
                return RedirectToAction("Passport", new { k = key });
            }
            catch(Exception ex)
            {
                ErrorView Error = new ErrorView();
                Error.HasError = true;
                if (ex is CustomHttpResponseException)
                {
                    CustomHttpResponseException cex = ex as CustomHttpResponseException;
                    Console.WriteLine("=======================================");
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                    if (cex.CustomData.StatusCode == 400)
                    {
                        Error.Message = "Tên đăng nhập hoặc mất khẩu sai";
                    }
                    else
                    {
                        Error.Message = "Đã có lỗi xảy ra";
                    }
                }
                else
                {
                    Console.WriteLine("=======================================");
                    Console.WriteLine(JsonConvert.SerializeObject(ex));
                    Error.Message = "Đã có lỗi xảy ra";
                }
                ViewBag.Error = Error;
                return View(LoginInstance);
            }
        }

        [ViewLayout("_FullPageLayout")]
        public IActionResult Passport(string k)
        {
            if (k == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                if (TempData.ContainsKey(k))
                {
                    ViewBag.User = TempData[k];
                    Console.WriteLine("================================");
                    Console.WriteLine(TempData[k]);
                    TempData.Remove(k);
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
    }
}