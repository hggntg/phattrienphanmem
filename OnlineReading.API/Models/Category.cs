using Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineReading.API.Models
{
    public class Category: BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public Category() : base()
        {

        }
    }
}
