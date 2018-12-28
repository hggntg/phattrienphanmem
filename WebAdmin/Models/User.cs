using System;

namespace WebAdmin.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime DOB { get; set; }

        public string FullName()
        {
            return this.FirstName + " " + this.LastName;
        }

        public string RealAvatar()
        {
            string Avatar = "";
            if(this.Avatar == null || this.Avatar == "" || this.Avatar == "unknown")
            {
                Avatar = "/images/default.png";
            }
            else
            {
                Avatar = this.Avatar;
            }
            return Avatar;
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthModel
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public long ExpiresIn { get; set; }
    }

    public class UserAggregate
    {
        public User UserInfo { get; set; }
        public string AccessToken { get; set; }
    }
}
