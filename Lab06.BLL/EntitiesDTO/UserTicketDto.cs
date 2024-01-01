using System;

namespace Lab06.BLL.EntitiesDTO
{
    public class UserTicketDto
    {
        public int Id { get; set; }

        public int ParkAttractionId { get; set; }

        public string ApplicationUserId { get; set; }

        public DateTime PurchaseTime { get; set; } 
    }
}
