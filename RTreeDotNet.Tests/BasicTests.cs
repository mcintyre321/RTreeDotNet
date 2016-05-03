using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RTreeDotNet;
using RTreeDotNet.rtree;

namespace TReeDotNet.Tests
{
    public class BasicTests
    {
        private Place[] Towns;
        private SpatialIndex<Place> _index;

        public class Place
        {
            public string postcode { get; }
            public int eastings { get; }
            public int northings { get; }
            public string latitude { get; }
            public string longitude { get; }
            public string town { get; }
            public string region { get; }
            public string country { get; }
            public string country_string { get; }

            public Place(string postcode, int eastings, int northings, string latitude, string longitude, string town, string region, string country, string countryString)
            {
                this.postcode = postcode;
                this.eastings = eastings;
                this.northings = northings;
                this.latitude = latitude;
                this.longitude = longitude;
                this.town = town;
                this.region = region;
                this.country = country;
                country_string = countryString;
            }

            public Point MakePoint()
            {
                return new Point(this.eastings, this.northings);
            }
        }

        [SetUp]
        public void AddCities()
        {
            Towns = new WebClient().DownloadString(
                "https://raw.githubusercontent.com/Gibbs/UK-Postcodes/master/postcodes.csv")
                .Split('\r', '\n')
                .Skip(1)
                .Where(s => s != "")
                .Select(s => s.Split(','))
                .Select(s => new Place(s[0].Trim('\"'), int.Parse(s[1].Trim('\"')), int.Parse(s[2].Trim('\"')), s[3].Trim('\"'), s[4].Trim('\"'), s[5].Trim('\"'), s[6].Trim('\"'), s[7].Trim('\"'), s[8].Trim('\"'))).GroupBy(x => x.town)
                .Select(grouping => grouping.First())
                .ToArray()
                ;
            _index = new SpatialIndex<Place>();
            _index.init();
            foreach (var place in Towns)
            {
                var point = place.MakePoint();
                _index.add(new Rectangle(point.x, point.y, point.x + 1, point.y + 1), place);
            }
        }

        [Test]
        public void X()
        {
            var place = Towns.Single(t => t.town == "Edinburgh");
            var nearest = _index.NearestN(place.MakePoint(), 10, Single.PositiveInfinity);


        }

        class Site
        {
            private int _x;
            private int _y;
            private int _w;
            private int _h;

            public Site(int x, int y, int w, int h)
            {
                this._x = x;
                this._y = y;
                this._w = w;
                this._h = h;
            }
        }
    }
}
