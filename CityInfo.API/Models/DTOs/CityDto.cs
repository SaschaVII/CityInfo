using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models.DTOs
{
    public class CityDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();
        public int NumberOfPointsOfInterest
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }

        public static List<CityDto> Cities = new List<CityDto>
        {
            new CityDto { Id = 1, Name = "Exeter", Description = "The capitol of Devon." },
            new CityDto
            {
                Id = 2, Name = "Plymouth",
                Description = "The largest city in Devon.",
                PointsOfInterest = new List<PointOfInterestDto>
                {
                    new PointOfInterestDto
                    {
                        Id = 1, Name = "Sutton Harbour",
                        Description = "The harbour the Mayflower departed from."
                    }
                }
            },
            new CityDto { Id = 3, Name = "Graz", Description = "The capitol of Styria." }
        };
    }
}
