using System;

namespace Lab06.DAL.Entities
{
    public class UserTicket
    {
        public int Id { get; set; }

        public int ParkAttractionId { get; set; }

        public string ApplicationUserId { get; set; }

        public DateTime PurchaseTime { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ParkAttraction ParkAttraction { get; set; }
    }
}
