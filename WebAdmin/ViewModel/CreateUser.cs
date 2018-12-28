using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.ViewModel
{
    public class CreateUser
    {
        public string UserName { get; set; }
        public string Password
        {
            get
            {
                return "P@ssword123";
            }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime DOB { get; set; }
    }
}
