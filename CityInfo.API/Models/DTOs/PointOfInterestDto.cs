using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models.DTOs
{
    public class PointOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
