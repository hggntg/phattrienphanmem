using System.Net.Http;
using WebAdmin.Models;
using Base.APIBuilder;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Base;
using WebAdmin.ViewModel;

namespace WebAdmin.Services
{
    public interface AccountService
    {
        Task<List<User>> List(string Token);
        Task<User> Detail(string Token);
        Task<User> UserDetail(string Id, string Token);
        Task<User> Update(User User, string Token);
        Task<User> Create(CreateUser User, string Token);
    }
    public class AccountServiceImp : AccountService
    {
        private readonly HttpClient HttpClientInstance;
        private readonly Builder APIBuilder;

        public AccountServiceImp(HttpClient HttpClientInstance, IMapper Mapper)
        {
            this.HttpClientInstance = HttpClientInstance;
            this.APIBuilder = new Builder(this.HttpClientInstance, Mapper, "AccountService");
        }

        public async Task<User> UserDetail(string Id, string Token)
        {
            List<string> Params = new List<string>();
            string Fields = Utilities.ClassToSelectedFields<User>();
            Params.Add(Fields);
            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return await this.APIBuilder.Get<User>("detail/" + Id + "?fields=$1", Params, AuthenObjectInstance);
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

        public async Task<List<User>> List(string Token)
        {
            List<string> Params = new List<string>();
            string Fields = Utilities.ClassToSelectedFields<User>();
            Params.Add(Fields);
            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return await this.APIBuilder.GetList<User>("list?fields=$1", Params, AuthenObjectInstance);
        }

        public async Task<User> Update(User User, string Token)
        {
            List<string> Params = new List<string>();
            string Fields = Utilities.ClassToSelectedFields<User>();
            Params.Add(Fields);

            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return await this.APIBuilder.Put<User, User>("update", User, Params, AuthenObjectInstance);
        }

        public async Task<User> Create(CreateUser User, string Token)
        {
            List<string> Params = new List<string>();

            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return await this.APIBuilder.Post<CreateUser, User>("signup", User, Params, AuthenObjectInstance);
        }
    }
}
