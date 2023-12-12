using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P03WPFWeatherForecast.Services
{
    class WeatherForecastService
    {
        const string url = $"https://www.google.com/search?q=pogoda";
        const string regexTemplate = "<div class=\"BNeawe iBp4i AP7Wnd\">(-{0,1}\\d{1,3}).[CF]<\\/div>";

        public int GetTemperature(string city)
        {                    
                WebClient wc = new WebClient();
                string data = wc.DownloadString(url + " " + city);
                try
                {
                    Regex rx = new Regex(regexTemplate);
                    Match match = rx.Match(data);

                    string result = match.Groups[1].Value;

                    return Convert.ToInt32(result);
                }
                catch (Exception)
                {
                    throw new Exception("Nie udało się pobrać temperatury");               
                }
            }
        }
    }

