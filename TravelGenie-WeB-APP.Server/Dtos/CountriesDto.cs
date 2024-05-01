using Newtonsoft.Json;

namespace TravelGenie_WeB_APP.Server.Dtos
{
    public class CountriesDto
    {
        [JsonProperty("0")]
        public List<CountryDto> countries { get; set; }
    }
}
