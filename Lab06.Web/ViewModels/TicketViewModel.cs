using System;
using System.ComponentModel;

namespace Lab06.Web.ViewModels
{
    public class TicketViewModel
    {
        public string UserId { get; set; }

        public int AttractionId { get; set; }

        [DisplayName("Image")]
        public string ImagePath { get; set; }

        [DisplayName("Park-Attraction-Name")]
        public string AttractionName { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Purchase-Time")]
        public DateTime PurchaseTime { get; set; }
    }
}
