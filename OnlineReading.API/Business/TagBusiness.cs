using Base;
using Newtonsoft.Json.Linq;
using OnlineReading.API.Infrastructure;
using OnlineReading.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineReading.API.Business
{
    public class TagBusiness
    {
        private readonly StoryContext StoryContextInstance;
        public TagBusiness(StoryContext StoryContextInstance)
        {
            this.StoryContextInstance = StoryContextInstance;
        }

        public Tag Detail(int Id)
        {
            return this.StoryContextInstance.Tags.SingleOrDefault(t => t.Id == Id);
        }

        public List<Tag> List(List<int> Ids)
        {
            if(Ids.Count == 0)
            {
                return this.StoryContextInstance.Tags.ToList();
            }
            else
            {
                List<Tag> NewTags = new List<Tag>();
                int IdLength = Ids.Count;
                for(int i = 0; i < IdLength; i++)
                {
                    Tag TempTag = this.StoryContextInstance.Tags.SingleOrDefault(t => t.Id == Ids[i]);
                    if(TempTag != null)
                    {
                        NewTags.Add(TempTag);
                    }
                }
                return NewTags;
            }
        }

        public List<Tag> Initiate()
        {
            List<JObject> Tags = new List<JObject>();
            Tags.Add(new JObject(new JProperty("Name", "Tiểu thuyết"), new JProperty("Key", "tieu-thuyet")));
            Tags.Add(new JObject(new JProperty("Name", "Truyện ngắn"), new JProperty("Key", "truyen-ngan")));
            Tags.Add(new JObject(new JProperty("Name", "Hài hước"), new JProperty("Key", "hai-huoc")));
            Tags.Add(new JObject(new JProperty("Name", "Tình cảm"), new JProperty("Key", "tinh-cam")));
            Tags.Add(new JObject(new JProperty("Name", "Học đường"), new JProperty("Key", "hoc-duong")));
            Tags.Add(new JObject(new JProperty("Name", "Kiếm hiệp"), new JProperty("Key", "kiem-hiep")));
            Tags.Add(new JObject(new JProperty("Name", "Viễn tưởng"), new JProperty("Key", "vien-tuong")));
            Tags.Add(new JObject(new JProperty("Name", "Trinh thám"), new JProperty("Key", "trinh-tham")));
            Tags.Add(new JObject(new JProperty("Name", "Hành động"), new JProperty("Key", "hanh-dong")));

            Utilities.Initiate<Tag>(this.StoryContextInstance.Tags, Tags);

            this.StoryContextInstance.SaveChanges();

            return this.StoryContextInstance.Tags.ToList();
        }
    }
}
