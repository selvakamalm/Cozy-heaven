using System.ComponentModel.DataAnnotations;

namespace CozyHaven.MVC.Models
{
    public class HotelViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        public string Description { get; set; }

        [Required]
        public int OwnerId { get; set; }
    }

}
