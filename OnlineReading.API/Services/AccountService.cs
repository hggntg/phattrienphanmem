using AutoMapper;
using OnlineReading.API.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Base.APIBuilder;
using System.Collections.Generic;
using Base;

namespace OnlineReading.API.Services
{
    public interface AccountService
    {
        Task<User> Detail(string Token);
    }

    public class AccountServiceImp : AccountService
    {
        private readonly HttpClient HttpClientInstance;
        private readonly Builder APIBuilder;


        public AccountServiceImp(HttpClient HttpClientInstance, IMapper Mapper)
        {
            this.HttpClientInstance = HttpClientInstance;
            this.APIBuilder = new Builder(this.HttpClientInstance, Mapper, "User Service");
        }

        public async Task<User> Detail(string Token)
        {
            List<string> Params = new List<string>();
            string Fields = Utilities.ClassToSelectedFields<User>();
            Params.Add(Fields);
            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return await this.APIBuilder.Get<User>("detail?fields=$1", Params, AuthenObjectInstance);
        }
    }
}
