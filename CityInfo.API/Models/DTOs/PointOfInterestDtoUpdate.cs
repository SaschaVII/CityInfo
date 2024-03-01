using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models.DTOs
{
    public class PointOfInterestDtoUpdate
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
    }
}
