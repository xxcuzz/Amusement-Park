using System.ComponentModel;

namespace Lab06.Web.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        [DisplayName("First-Name")]
        public string FirstName { get; set; }


        [DisplayName("Surname")]
        public string Surname { get; set; }


        [DisplayName("Username")]
        public string Username { get; set; }


        [DisplayName("Email")]
        public string Email { get; set; }
    }
}
