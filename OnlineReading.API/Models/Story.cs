using Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineReading.API.Models
{
    public class Story : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Lovings { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }



        public Story() : base()
        {
            this.Lovings = 0;
            this.Category = new Category();
            this.Status = "Draft";
        }
    }
}
