﻿using Newtonsoft.Json;
using P04WeatherForecastWPF.Client.Models;
using System;
using System.Collections.Generic;
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
        private const string autoComplute_endpoint = "locations/v1/cities/autocomplete?apikey={0}&q={1}&language={2}";

        private const string language = "pl";
        private const string api_key = "Yv1YCDwZwW8xEGHTE5Oiupl6rtiwrU71";

        public async Task<City[]> GetLocations(string locationName)
        {
            string url = base_url + "/" + string.Format(autoComplute_endpoint, api_key, language);
            using (HttpClient clinet = new HttpClient())
            {
                var response = await clinet.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();
                City[] cities = JsonConvert.DeserializeObject<City[]>(json);
                return cities;
            }
        }

    }
}