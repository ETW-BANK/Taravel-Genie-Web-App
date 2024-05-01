using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Text;
using TravelGenie_WeB_APP.Server.Dtos;

namespace TravelGenie_WeB_APP.Server.Services
{
    public class Travel:ITravel
    {
        public async Task<List<string>> GetCountryNames()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = $"https://city-list.p.rapidapi.com/api/getCountryList";

                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "597c529c02msh24cd8fda8287734p115600jsn5390d57dc0a0");
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "city-list.p.rapidapi.com");

                    var response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();


                    var result = JsonConvert.DeserializeObject<CountriesDto>(jsonResponse);


                    var countryNames = result?.countries?.Select(c => c.cname)?.ToList();


                    return countryNames;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Could not connect to server", ex);
            }
        }

        public async Task<List<string>> GetCities(string country)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = $"https://world-citiies-api.p.rapidapi.com/cities/country/{country}";

                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "954eeaa13amsh4309e7a17a3d7a0p1370e5jsnb111604275a3");
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "world-citiies-api.p.rapidapi.com");

                    var response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var cityNames = JsonConvert.DeserializeObject<List<CitiesDto>>(jsonResponse)

                        .Select(c => c.Name).OrderBy(city => city)
                        .ToList();

                    return cityNames;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not connect to server", ex);
            }
        }

        public async Task<ChatGptDto> GetPlanDetail(string prompt)
        {
            try
            {
                var conversation = new[]
                {
            new
            {
                content = $"{prompt}",
                role = "user"
            }
        };

                using (var client = new HttpClient())
                {
                    string apiUrl = "https://chatgpt-api8.p.rapidapi.com/";

                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "591bd945f4mshbfb84bf3770c328p1c3121jsn63dbf14028d2");
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "chatgpt-api8.p.rapidapi.com");

                    var jsonContent = JsonConvert.SerializeObject(conversation);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync(apiUrl, content))
                    {
                        response.EnsureSuccessStatusCode();

                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var AiResponse = JsonConvert.DeserializeObject<ChatGptDto>(jsonResponse);

                        return AiResponse;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
