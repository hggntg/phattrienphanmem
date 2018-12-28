using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Base.APIBuilder;
using ViewModel;
using AutoMapper;
using Base;
using FileService.Model;

namespace Service
{
    public interface AccountService
    {
        Task<List<UserView>> GetUser(string Token);
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

        public async Task<List<UserView>> GetUser(string Token)
        {
            List<string> Params = new List<string>();
            Params.Add("Id,Email,LastName,FirstName");
            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return await this.APIBuilder.GetList<UserView>("api/account/list?fields=$1", Params, AuthenObjectInstance);
        }

        public async Task<User> Detail(string Token)
        {
            List<string> Params = new List<string>();
            string Fields = Utilities.ClassToSelectedFields<UserView>();
            Params.Add(Fields);
            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return await this.APIBuilder.Get<User>("detail?fields=$1", Params, AuthenObjectInstance);
        }
    }
}
