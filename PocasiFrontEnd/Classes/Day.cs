using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace PocasiFrontEnd.Classes;

public class Day
{
    public DateOnly Date { get; set; }
    public int WeatherCode { get; set; }
    public string IconPath { get; set; }
    public string MaxTemperature { get; set; }
    public string MinTemperature { get; set; }
    public TimeOnly Sunrise { get; set; }
    public TimeOnly Sunset { get; set; }

    private Current currentData;
    public Current CurrentData
    {
        get => currentData;
        set
        {
            currentData = value;
            currentData.IconPath = getIconPath(currentData.weather_code);
        }
    }

    public Day(Root rootData, int day,Place place)
    {
        Place = place;
        Date = DateOnly.Parse(rootData.daily.time[day]);
        WeatherCode = rootData.daily.weather_code[day];
        IconPath = getIconPath(WeatherCode);
        MaxTemperature = rootData.daily.temperature_2m_max[day] + " \u00b0C";
        MinTemperature = rootData.daily.temperature_2m_min[day] + " \u00b0C";
        CurrentData = rootData.current;
        Sunrise = TimeOnly.FromDateTime( DateTime.Parse(rootData.daily.sunrise[day]));
        Sunset = TimeOnly.FromDateTime( DateTime.Parse(rootData.daily.sunset[day]));
    }

    //TODO implementovat získání ikony podle weather code
    private string getIconPath(int iconPath)
    {
        return "icon.png";
    }

    public static Place Place { get; set; }
    public static Root GetApiData()
    {
        var apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={Place.Latitude}&longitude={Place.Longitude}&current=temperature_2m,is_day,rain,showers,snowfall,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min,sunrise,sunset&timezone=Europe%2FBerlin";
        using (var client = new HttpClient())
        {
            var response = client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode) return null;

            var responseContent = response.Content;
            var responseString = responseContent.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Root>(responseString) ?? throw new InvalidOperationException();
        }
    }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Current
{
    public string time { get; set; }
    public int interval { get; set; }
    public double temperature_2m { get; set; }
    public int is_day { get; set; }
    public double rain { get; set; }
    public double showers { get; set; }
    public double snowfall { get; set; }
    public int weather_code { get; set; }
    public string IconPath { get; set; }
}

public class CurrentUnits
{
    public string time { get; set; }
    public string interval { get; set; }
    public string temperature_2m { get; set; }
    public string is_day { get; set; }
    public string rain { get; set; }
    public string showers { get; set; }
    public string snowfall { get; set; }
    public string weather_code { get; set; }
}

public class Daily
{
    public List<string> time { get; set; }
    public List<int> weather_code { get; set; }
    public List<double> temperature_2m_max { get; set; }
    public List<double> temperature_2m_min { get; set; }
    public List<string> sunrise { get; set; }
    public List<string> sunset { get; set; }
}

public class DailyUnits
{
    public string time { get; set; }
    public string weather_code { get; set; }
    public string temperature_2m_max { get; set; }
    public string temperature_2m_min { get; set; }
    public string sunrise { get; set; }
    public string sunset { get; set; }
}

public class Root
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double generationtime_ms { get; set; }
    public int utc_offset_seconds { get; set; }
    public string timezone { get; set; }
    public string timezone_abbreviation { get; set; }
    public double elevation { get; set; }
    public CurrentUnits current_units { get; set; }
    public Current current { get; set; }
    public DailyUnits daily_units { get; set; }
    public Daily daily { get; set; }
    //public int day { get; set; }
    
}