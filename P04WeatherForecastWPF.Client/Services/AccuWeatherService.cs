
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using P04WeatherForecastWPF.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace P04WeatherForecastWPF.Client.Services
{
    internal class AccuWeatherService
    {
        private const string base_url = "http://dataservice.accuweather.com/";
        private const string autoComplite_endpoint = "locations/v1/cities/autocomplete?apikey={0}&q={1}&language={2}";
        private const string current_conditions_endpoint = "currentconditions/v1/{0}?apikey={1}&language={2}";

        // private const string language = "pl";
        private string language; // teraz to zzmienna  
        //private const string api_key = "hjL55jHARudyfE7JBk4GwGhZO6szC3Qj";
        private string api_key;

        public AccuWeatherService()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<App>()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            language = configuration["default_language"];
            api_key = configuration["api_key"];
        }

        public async Task<City[]> GetLocations(string locationName)
        {
            string url = base_url + "/" + string.Format(autoComplite_endpoint, api_key, locationName, language);
            using (HttpClient clinet = new HttpClient())
            {
                var response = await clinet.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();
                City[] cities = JsonConvert.DeserializeObject<City[]>(json);
                return cities;
            }
        }

        public async Task<Weather> GetCurentConditions(string cityKey)
        {
            string url = base_url + "/" + string.Format(current_conditions_endpoint,cityKey, api_key, language);
            using (HttpClient clinet = new HttpClient())
            {
                var response = await clinet.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();
                Weather[] weathers = JsonConvert.DeserializeObject<Weather[]>(json);
                return weathers.FirstOrDefault();
            }
        }

    }
}
