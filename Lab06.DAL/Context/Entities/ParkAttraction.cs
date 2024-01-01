using System.Collections.Generic;

namespace Lab06.DAL.Entities
{
    public class ParkAttraction
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public virtual AttractionImage AttractionImage { get; set; }

        public virtual ICollection<UserTicket> UserTickets { get; set; }
    }
}
