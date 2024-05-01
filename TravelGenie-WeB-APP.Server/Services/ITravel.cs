using TravelGenie_WeB_APP.Server.Dtos;

namespace TravelGenie_WeB_APP.Server.Services
{
    public interface ITravel
    {
        Task<List<string>> GetCountryNames();//done
        Task<List<string>> GetCities(string country);//done
        Task<ChatGptDto> GetPlanDetail(string propt);
    }
}
