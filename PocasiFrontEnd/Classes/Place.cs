using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocasiFrontEnd.Classes;

public class Place
{
    public string Name { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }

    public Place(string name, string latitude, string longitude)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }

    public override bool Equals(object obj)
    {
        if (obj is Place otherPlace)
        {
            return Name == otherPlace.Name &&
                   Latitude == otherPlace.Latitude &&
                   Longitude == otherPlace.Longitude;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Latitude, Longitude);
    }
}