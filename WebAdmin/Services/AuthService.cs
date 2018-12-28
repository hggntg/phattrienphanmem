using AutoMapper;
using Base.APIBuilder;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebAdmin.Models;

namespace WebAdmin.Services
{
    public interface AuthService
    {
        Task<AuthModel> Login(LoginModel LoginInstance);
    }

    public class AuthServiceImp: AuthService
    {
        private readonly HttpClient HttpClientInstance;
        private readonly Builder APIBuilder;
        public AuthServiceImp(HttpClient HttpClientInstance, IMapper Mapper)
        {
            this.HttpClientInstance = HttpClientInstance;
            this.APIBuilder = new Builder(this.HttpClientInstance, Mapper, "AuthService");
        }

        public async Task<AuthModel> Login(LoginModel LoginInstance)
        {
            AuthenObject AuthenInstance = new AuthenObject();
            return await this.APIBuilder.Post<LoginModel, AuthModel>("login", LoginInstance, new List<string>(), AuthenInstance);      
        }
    }
}
