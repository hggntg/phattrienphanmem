using OnlineReading.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineReading.API.Aggregate
{
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
