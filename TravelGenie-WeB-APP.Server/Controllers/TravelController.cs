using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelGenie_WeB_APP.Server.Services;

namespace TravelGenie_WeB_APP.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly ITravel _travel;

        public TravelController(ITravel travel)
        {
            _travel = travel;
        }

        [HttpGet]
        [Route("Countries")]

        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var result = await _travel.GetCountryNames();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving cities: {ex.Message}");
            }

        }

        [HttpGet]
        [Route("Cities")]
        public async Task<IActionResult> GetCities(string country)
        {
            try
            {
                var cities = await _travel.GetCities(country);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving cities: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("AItripplan")]
        public async Task<IActionResult> GetTripPlanDetailForUser(string prompt)
        {
            try
            {
                var tripPlan = await _travel.GetPlanDetail(prompt);
                return Ok(tripPlan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving trip plan: {ex.Message}");
            }


        }


    }
}
