using Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineReading.API.Models
{
    public class Tag: BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }

        public Tag(): base()
        {

        }
    }

    public class StoryTag : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Story")]
        public int StoryId { get; set; }
        public virtual Story Story { get; set; }

        [ForeignKey("Tag")]
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }

        public StoryTag() : base()
        {

        }
    }
}
