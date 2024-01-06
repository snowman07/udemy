using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Hotel
{
    public abstract class BaseHotelDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public double? Rating { get; set; } //? means nullable meaning Rating property does not need to have a value

        [Required]
        [Range(1, int.MaxValue)]
        public int CountryId { get; set; }
    }
}
