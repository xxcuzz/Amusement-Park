using System.ComponentModel;

namespace Lab06.Web.ViewModels
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        [DisplayName("Park-Attraction-Name")]
        public string Name { get; set; }

        [DisplayName("Image")]
        public string Image { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }
    }
}
