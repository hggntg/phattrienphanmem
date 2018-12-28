using System;
using System.ComponentModel.DataAnnotations.Schema;
using Base;

namespace FileService.Model
{
    public class File : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string UniqueName { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [NotMapped]
        public string Url
        {
            get
            {
                string[] NameSegments = Name.Split(".");
                return "http://localhost:6000/api/file/" + this.UniqueName + "." + NameSegments[NameSegments.Length - 1];
            }
        }

        public File(string Name, string Location, string UserId, DateTime? CreatedDate, DateTime? UpdatedDate) : base(CreatedDate, UpdatedDate)
        {
            this.Name = Name;
            this.Location = Location;
            this.UserId = UserId;
            this.UniqueName = "F_1_" + DateTime.Now.Ticks.ToString() + "FXM_" + DateTime.Now.Ticks.ToString();
        }

        public File(DateTime? CreatedDate, DateTime? UpdatedDate): base(CreatedDate, UpdatedDate)
        {
            this.UniqueName = "F_1_" + DateTime.Now.Ticks.ToString() + "FXM_" + DateTime.Now.Ticks.ToString();
        }

        public File(): base()
        {
            this.UniqueName = "F_1_" + DateTime.Now.Ticks.ToString() + "FXM_" + DateTime.Now.Ticks.ToString();
        }
    }
}
