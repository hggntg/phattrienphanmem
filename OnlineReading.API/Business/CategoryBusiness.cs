using Base;
using Newtonsoft.Json.Linq;
using OnlineReading.API.Infrastructure;
using OnlineReading.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineReading.API.Business
{
    public class CategoryBusiness
    {
        private readonly StoryContext StoryContextInstance;
        public CategoryBusiness(StoryContext StoryContextInstance)
        {
            this.StoryContextInstance = StoryContextInstance;
        }

        public Category Detail(int Id)
        {
            return this.StoryContextInstance.Categories.SingleOrDefault(c => c.Id == Id);
        }

        public List<Category> List(List<int> Ids)
        {
            if (Ids.Count == 0)
            {
                return this.StoryContextInstance.Categories.ToList();
            }
            else
            {
                List<Category> NewCategories = new List<Category>();
                int IdLength = Ids.Count;
                for (int i = 0; i < IdLength; i++)
                {
                    Category TempTag = this.StoryContextInstance.Categories.SingleOrDefault(c => c.Id == Ids[i]);
                    if (TempTag != null)
                    {
                        NewCategories.Add(TempTag);
                    }
                }
                return NewCategories;
            }
        }

        public List<Category> Initiate()
        {
            List<JObject> Categories = new List<JObject>();
            Categories.Add(
                new JObject(
                    new JProperty("Name", "Truyện Chữ"),
                    new JProperty("Key", "truyen-chu")
                    ));

            Utilities.Initiate<Category>(this.StoryContextInstance.Categories, Categories);

            this.StoryContextInstance.SaveChanges();

            return this.StoryContextInstance.Categories.ToList();
        }
    }
}
