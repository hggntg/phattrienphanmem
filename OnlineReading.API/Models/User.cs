using System;
using System.ComponentModel.DataAnnotations.Schema;
using Base;

namespace OnlineReading.API.Models
{
    public class User : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User(string Id, string Email, string FirstName, string LastName, DateTime? CreatedDate, DateTime? UpdatedDate) : base(CreatedDate, UpdatedDate)
        {
            this.Id = Id;
            this.Email = Email;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
        public User(DateTime? CreatedDate, DateTime? UpdatedDate) : base(CreatedDate, UpdatedDate)
        {

        }

        public User() : base()
        {

        }
    }
}
