using Base.APIBuilder;
using Newtonsoft.Json;
using OnlineReading.API.Aggregate;
using OnlineReading.API.Infrastructure;
using OnlineReading.API.Models;
using OnlineReading.API.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineReading.API.Business
{
    public class StoryBusiness
    {
        private readonly StoryContext StoryContextInstance;
        private readonly AccountService AccountServiceInstance;
        public StoryBusiness(StoryContext StoryContextInstance, AccountService AccountServiceInstance)
        {
            this.StoryContextInstance = StoryContextInstance;
            this.AccountServiceInstance = AccountServiceInstance;
        }

        public List<StoryAggregate> PublicList()
        {
            List<StoryAggregate> Stories = new List<StoryAggregate>();

            List<Story> StoryInfoes = this.StoryContextInstance.Stories.ToList();

            StoryInfoes.ForEach(StoryInfo =>
            {
                StoryAggregate Story = new StoryAggregate();
                Story.StoryInfo = StoryInfo;
                List<StoryTag> StoryTags = new List<StoryTag>();

                StoryTags.AddRange(this.StoryContextInstance.StoryTags.Where(st => st.StoryId == StoryInfo.Id).ToList());

                StoryTags.ForEach(StoryTag =>
                {
                    Story.Tags.Add(this.StoryContextInstance.Tags.SingleOrDefault(t => t.Id == StoryTag.TagId));
                });
                Stories.Add(Story);
            });
            return Stories;
        }

        public List<StoryAggregate> List(string Token)
        {
            var Handler = new JwtSecurityTokenHandler();
            Token = Token.Replace("Bearer", "").Trim();
            var JsonToken = Handler.ReadJwtToken(Token);
            string Id = JsonToken.Claims.First(c => c.Type == "id").Value;

            User User = this.StoryContextInstance.Users.SingleOrDefault(u => u.Id == Id);

            List<StoryAggregate> Stories = new List<StoryAggregate>();

            List<Story> StoryInfoes = this.StoryContextInstance.Stories.Where(s => s.UserId == User.Id).ToList();


            StoryInfoes.ForEach(StoryInfo =>
            {
                StoryAggregate Story = new StoryAggregate();
                Story.StoryInfo = StoryInfo;
               List<StoryTag> StoryTags = new List<StoryTag>();

                StoryTags.AddRange(this.StoryContextInstance.StoryTags.Where(st => st.StoryId == StoryInfo.Id).ToList());

                StoryTags.ForEach(StoryTag =>
                {
                    Story.Tags.Add(this.StoryContextInstance.Tags.SingleOrDefault(t => t.Id == StoryTag.TagId));
                });
                Stories.Add(Story);
            });       
            return Stories;
        }

        public StoryAggregate Detail(string Token, int Id)
        {
            StoryAggregate Story = new StoryAggregate();
            Story.StoryInfo = this.StoryContextInstance.Stories.SingleOrDefault(s => s.Id == Id);
            List<StoryTag> StoryTags = new List<StoryTag>();

            StoryTags.AddRange(this.StoryContextInstance.StoryTags.Where(st => st.StoryId == Id).ToList());

            StoryTags.ForEach(StoryTag =>
            {
                Story.Tags.Add(this.StoryContextInstance.Tags.SingleOrDefault(t => t.Id == StoryTag.TagId));
            });
            return Story;
        }

        public StoryAggregate PublicDetail(int Id)
        {
            StoryAggregate Story = new StoryAggregate();
            Story.StoryInfo = this.StoryContextInstance.Stories.SingleOrDefault(s => s.Id == Id);
            List<StoryTag> StoryTags = new List<StoryTag>();

            StoryTags.AddRange(this.StoryContextInstance.StoryTags.Where(st => st.StoryId == Id).ToList());

            StoryTags.ForEach(StoryTag =>
            {
                Story.Tags.Add(this.StoryContextInstance.Tags.SingleOrDefault(t => t.Id == StoryTag.TagId));
            });
            return Story;
        }

        public StoryAggregate Update(string Token, StoryAggregate StoryInstance)
        {
            try
            {
                var Handler = new JwtSecurityTokenHandler();
                Token = Token.Replace("Bearer", "").Trim();
                var JsonToken = Handler.ReadJwtToken(Token);
                string Id = JsonToken.Claims.First(c => c.Type == "id").Value;

                Console.WriteLine("============================================");
                Console.WriteLine("============================================");
                Console.WriteLine("============================================");
                Console.WriteLine(JsonConvert.SerializeObject(StoryInstance));

                Story Story = this.StoryContextInstance.Stories.SingleOrDefault(s => s.Id == StoryInstance.StoryInfo.Id);

                this.StoryContextInstance.Stories.Update(Story);

                List<Tag> Tags = new List<Tag>();
                Tags.AddRange(StoryInstance.Tags);


                Tags.ForEach(Tag =>
                {
                    StoryTag StoryTag = new StoryTag();
                    StoryTag.TagId = Tag.Id;
                    StoryTag.StoryId = Story.Id;

                    if(this.StoryContextInstance.StoryTags.SingleOrDefault(st => st.TagId == StoryTag.TagId && st.StoryId == StoryTag.StoryId) == null)
                    {
                        this.StoryContextInstance.StoryTags.Add(StoryTag);
                    }
                });

                this.StoryContextInstance.SaveChanges();

                return StoryInstance;
            }
            catch (Exception ex)
            {
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
                if (ex is CustomHttpResponseException)
                {
                    CustomHttpResponseException cex = ex as CustomHttpResponseException;
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex));
                }
                throw ex;
            }
        }

        public async Task<StoryAggregate> Create(string Token, StoryAggregate StoryInstance)
        {
            try
            {
                var Handler = new JwtSecurityTokenHandler();
                Token = Token.Replace("Bearer", "").Trim();
                var JsonToken = Handler.ReadJwtToken(Token);
                string Id = JsonToken.Claims.First(c => c.Type == "id").Value;

                User User = this.StoryContextInstance.Users.SingleOrDefault(u => u.Id == Id);

                if (User == null)
                {
                    User = await this.AccountServiceInstance.Detail("Bearer " + Token);

                    this.StoryContextInstance.Users.Add(User);

                    this.StoryContextInstance.SaveChanges();
                }

                Story Story = StoryInstance.StoryInfo;

                Story.UserId = Id;
                Story.User = User;
                Story.Category = this.StoryContextInstance.Categories.SingleOrDefault(c => c.Id == Story.CategoryId);

                this.StoryContextInstance.Stories.Add(Story);

                List<Tag> Tags = new List<Tag>();
                Tags.AddRange(StoryInstance.Tags);

                Tags.ForEach(Tag =>
                {
                    StoryTag StoryTag = new StoryTag();
                    StoryTag.TagId = Tag.Id;
                    StoryTag.StoryId = Story.Id;
                    this.StoryContextInstance.StoryTags.Add(StoryTag);
                });

                this.StoryContextInstance.SaveChanges();

                return StoryInstance;
            }
            catch(Exception ex)
            {
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
                if(ex is CustomHttpResponseException)
                {
                    CustomHttpResponseException cex = ex as CustomHttpResponseException;
                    Console.WriteLine(JsonConvert.SerializeObject(cex.CustomData));
                }
                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject(ex));
                }
                throw ex;
            }  
        }
    }
}
