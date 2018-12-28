using AutoMapper;
using Base;
using Base.APIBuilder;
using WebAdmin.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAdmin.Services
{
    public interface StoryService
    {
        Task<StoryAggregate> Create(string Token, StoryAggregate StoryAggregate);
        Task<StoryAggregate> Detail(string Token, int Id);
        Task<List<StoryAggregate>> List(string Token);
        Task<StoryAggregate> Update(StoryAggregate StoryAggregate, string Token);
        Task<List<StoryAggregate>> PublicList();
        Task<StoryAggregate> PublicDetail(int Id);
    }

    public class StoryServiceImp : StoryService
    {
        private readonly HttpClient HttpClientInstance;
        private readonly Builder APIBuilder;


        public StoryServiceImp(HttpClient HttpClientInstance, IMapper Mapper)
        {
            this.HttpClientInstance = HttpClientInstance;
            this.APIBuilder = new Builder(this.HttpClientInstance, Mapper, "Story Service");
        }

        public async Task<StoryAggregate> Create(string Token, StoryAggregate StoryAggregate)
        {
            List<string> Params = new List<string>();

            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;

            return await this.APIBuilder.Post<StoryAggregate, StoryAggregate>("create", StoryAggregate, Params, AuthenObjectInstance);
        }

        public async Task<StoryAggregate> PublicDetail(int Id)
        {
            List<string> Params = new List<string>();

            AuthenObject AuthenObjectInstance = new AuthenObject();

            return await this.APIBuilder.Get<StoryAggregate>("public-detail/" + Id, Params, AuthenObjectInstance);
        }

        public async Task<StoryAggregate> Detail(string Token, int Id)
        {
            List<string> Params = new List<string>();

            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;

            return await this.APIBuilder.Get<StoryAggregate>("detail/" + Id, Params, AuthenObjectInstance);
        }

        public async Task<List<StoryAggregate>> PublicList()
        {
            List<string> Params = new List<string>();

            AuthenObject AuthenObjectInstance = new AuthenObject();

            return await this.APIBuilder.GetList<StoryAggregate>("public-list", Params, AuthenObjectInstance);
        }

        public async Task<List<StoryAggregate>> List(string Token)
        {
            List<string> Params = new List<string>();

            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;

            return await this.APIBuilder.GetList<StoryAggregate>("list", Params, AuthenObjectInstance);
        }

        public async Task<StoryAggregate> Update(StoryAggregate StoryAggregate, string Token)
        {
            List<string> Params = new List<string>();

            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return await this.APIBuilder.Put<StoryAggregate, StoryAggregate>("update", StoryAggregate, Params, AuthenObjectInstance);
        }
    }

    public interface CategoryService
    {
        Task<List<Category>> List(List<int> Ids, string Token);
    }

    public class CategoryServiceImp : CategoryService
    {
        private readonly HttpClient HttpClientInstance;
        private readonly Builder APIBuilder;


        public CategoryServiceImp(HttpClient HttpClientInstance, IMapper Mapper)
        {
            this.HttpClientInstance = HttpClientInstance;
            this.APIBuilder = new Builder(this.HttpClientInstance, Mapper, "Category Service");
        }
        public Task<List<Category>> List(List<int> Ids, string Token)
        {
            List<string> Params = new List<string>();
            string Fields = Utilities.ClassToSelectedFields<Category>();
            Params.Add(string.Join(",", Ids));
            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return this.APIBuilder.GetList<Category>("list?ids=$1", Params, AuthenObjectInstance);
        }
    }

    public interface TagService
    {
        Task<List<Tag>> List(List<int> Ids, string Token);
        Task<Tag> Detail(int Id, string Token);
    }

    public class TagServiceImp : TagService
    {
        private readonly HttpClient HttpClientInstance;
        private readonly Builder APIBuilder;


        public TagServiceImp(HttpClient HttpClientInstance, IMapper Mapper)
        {
            this.HttpClientInstance = HttpClientInstance;
            this.APIBuilder = new Builder(this.HttpClientInstance, Mapper, "Tag Service");
        }

        public Task<Tag> Detail(int Id, string Token)
        {
            List<string> Params = new List<string>();

            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return this.APIBuilder.Get<Tag>("detail/" + Id.ToString(), Params, AuthenObjectInstance);
        }

        public Task<List<Tag>> List(List<int> Ids, string Token)
        {
            List<string> Params = new List<string>();
            Params.Add(string.Join(",", Ids));
            AuthenObject AuthenObjectInstance = new AuthenObject();
            AuthenObjectInstance.IsAuthen = true;
            AuthenObjectInstance.Token = Token;
            return this.APIBuilder.GetList<Tag>("list?ids=$1", Params, AuthenObjectInstance);
        }
    }
}
