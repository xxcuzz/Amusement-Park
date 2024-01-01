using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Lab06.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<UserTicket> UserTickets { get; set; }
    }
}
