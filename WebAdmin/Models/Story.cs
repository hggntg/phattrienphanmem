using Base;
using System.Collections.Generic;

namespace WebAdmin.Models
{
    public class Category : BaseModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public Category() : base()
        {

        }
    }

    public class Tag : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }

        public Tag() : base()
        {

        }
    }

    public class Story : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }

        public string UserId { get; set; }

        public int CategoryId { get; set; }
     
        public int Lovings { get; set; }

        public Story() : base()
        {
            this.Lovings = 0;
            this.Status = "Draft";
        }
    }

    public class StoryAggregate
    {
        public Story StoryInfo { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public StoryAggregate()
        {
            this.StoryInfo = new Story();
            this.Tags = new List<Tag>();
        }
    }
}
