namespace TravelGenie_WeB_APP.Server.Services
{
    public interface ITravel
    {
        Task<List<string>> GetCountryNames();//done
        Task<List<string>> GetCities(string country);//done
        Task<string> GetPlanDetail(string propt);
    }
}
