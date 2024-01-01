using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace Lab06.Web.ViewModels
{
    public class AddParkAttractionViewModel
    {
        [DisplayName("Park-Attraction-Name")]
        public string Name { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Image")]
        public IFormFile Image { get; set; }
    }
}
