using CityInfo.API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Converters;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterests(int cityId)
        {
            var city = CityDto.Cities.SingleOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound($"Couldn't find city with id {cityId}.");
            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{id}")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int id)
        {
            var city = CityDto.Cities.SingleOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound($"Couldn't find city with id {cityId}.");
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == id);
            if (pointOfInterest == null) return NotFound($"Couldn't find point of interest with id {id} in {city.Name}.");
            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult PostPointOfInterest(int cityId, PointOfInterestDtoCreate poi)
        {
            var city = CityDto.Cities.SingleOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound($"Couldn't find city with id {cityId}.");

            // the following is because we are using an in memory data storage, and will be improved upon
            var mappedPointOfInterest = new PointOfInterestDto
            {
                Id = city.PointsOfInterest.Count + 1,
                Name = poi.Name,
                Description = poi.Description
            };
            city.PointsOfInterest.Add(mappedPointOfInterest);

            return CreatedAtAction(nameof(GetPointOfInterest), new { cityId = cityId, id = mappedPointOfInterest.Id }, mappedPointOfInterest);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePointOfInterest(
            int cityId,
            int id,
            PointOfInterestDtoUpdate poi)
        {
            var city = CityDto.Cities.SingleOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound($"Couldn't find city with id {cityId}.");
            var poiStored = city.PointsOfInterest.FirstOrDefault(x => x.Id == id);
            if (poiStored == null) return NotFound($"Couldn't find point of interest with id {id} in {city.Name}.");

            // the following is because we are using an in memory data storage, and will be improved upon
            poiStored.Name = poi.Name;
            poiStored.Description = poi.Description;

            // For updates either a 204 No Content or a 200 OK with the resource updated are both valid responses
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PatchPointOfInterest(
            int cityId,
            int id,
            [FromBody] JsonPatchDocument<PointOfInterestDtoUpdate> patchDoc)
        {
            var city = CityDto.Cities.SingleOrDefault(city => city.Id == cityId);
            if (city == null) return NotFound($"Couldn't find city with id {cityId}.");
            var poiStored = city.PointsOfInterest.FirstOrDefault(x => x.Id == id);
            if (poiStored == null) return NotFound($"Couldn't find point of interest with id {id} in {city.Name}.");

            // need to map to the update dto
            var poiToPatch = new PointOfInterestDtoUpdate
            {
                Name = poiStored.Name,
                Description = poiStored.Description
            };

            patchDoc.ApplyTo(poiToPatch, ModelState);

            // Checks the model state after the patch is applied to catch whether the patch document was valid
            // Does not check if all the changes made to the resource adhere to the resource's validation rules
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Forcibly checks the patched resource's validation rules
            if (!TryValidateModel(poiToPatch)) return BadRequest(ModelState);

            // the following is because we are using an in memory data storage, and will be improved upon
            poiStored.Name = poiToPatch.Name;
            poiStored.Description = poiToPatch.Description;

            // For updates either a 204 No Content or a 200 OK with the resource updated are both valid responses
            return NoContent();
        }
    }
}
