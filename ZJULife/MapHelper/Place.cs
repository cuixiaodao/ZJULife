using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace ZJULife.MapHelper
{
    internal enum Kinds
    { Dorm, StudyPlace, DiningHall, Gym, Building, Spot, Market, Hospital };

    [Table("Places")]
    internal class PlaceData
    {
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Description { get; set; }

        public string Kind { get; set; }

        public string Campus { get; set; }
    }

    internal class Place
    {
        private static Dictionary<string, List<Place>> places = new Dictionary<string, List<Place>>();

        public string Name { get; set; }

        public string Description { get; set; }

        public Kinds Kind { get; set; }

        public string Campus { get; set; }

        public Geopoint Position { get; set; }

        public Place(string name, Geopoint position, string description, Kinds kind, string campus)
        {
            this.Name = name;
            this.Position = position;
            this.Description = description;
            this.Kind = kind;
            this.Campus = campus;
        }

        public static async Task<List<Place>> GetPlacesAsync()
        {
            string[] campuses = { "ZJG", "YQ", "XX", "HJC", "ZJ" }; 

            List<Place> allPlaces = new List<Place>();
            List<Place> campus = new List<Place>();
            foreach (string campusName in campuses)
            {
                campus = await GetPlacesAsync(campusName);
                allPlaces.AddRange(campus);
            }
            //List<Place> ZJGPlaces = await GetPlacesAsync("ZJG");
            //List<Place> YQPlaces = await GetPlacesAsync("YQ");
            //List<Place> XXPlaces = await GetPlacesAsync("XX");
            //List<Place> HJCPlaces = await GetPlacesAsync("HJC");
            //List<Place> HJCPlaces = await GetPlacesAsync("HJC");
            //allPlaces.AddRange(ZJGPlaces);
            //allPlaces.AddRange(YQPlaces);
            return allPlaces;
        }

        public static async Task<List<Place>> GetPlacesAsync(string campus)
        {
            if (places.ContainsKey(campus))
                return places[campus];

            places[campus] = new List<Place>();
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\Data.db", SQLiteOpenFlags.ReadOnly);
            //var placesOrign = conn.Table<PlaceOrign>();

            var querry = await (conn.Table<PlaceData>().Where(place => place.Campus == campus)).ToListAsync();
            foreach (var place in querry)
            {
                places[campus].Add(new Place(place.Name,
                                        new Geopoint(new BasicGeoposition { Latitude = place.Latitude, Longitude = place.Longitude }),
                                       place.Description, (Kinds)Enum.Parse(typeof(Kinds), place.Kind), place.Campus));
            }

            return places[campus];
        }
    }
}

//namespace ZJULife.MapHelper
//{
//    enum Kinds{ Building,Dormitory};
//    //class Place
//    //{
//    //    private static List<Place> places= new List<Place>();
//    //    public Place(string name, Geopoint position, Kinds kind, string description)
//    //    {
//    //        this.Name = name;
//    //        this.Position = position;
//    //        this.Kind = kind;
//    //        this.Description = description;
//    //    }
//    //    public string Name { get; set; }
//    //    public Geopoint Position { get; set; }
//    //    public string Description { get; set; }
//    //    public Kinds Kind { get; set; }

//    //    public static async Task<List<Place>> GetPlacesAsync()
//    //    {
//    //        if (places.Count != 0)
//    //            return places;

//    //        Uri dataUri = new Uri("ms-appx:///MapHelper/Places.json");
//    //        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
//    //        string jsonText = await FileIO.ReadTextAsync(file);
//    //        JsonObject jsonObject = JsonObject.Parse(jsonText);
//    //        JsonArray jsonArray = jsonObject["Places"].GetArray();

//    //        foreach (JsonValue place in jsonArray)
//    //        {
//    //            JsonObject placeObject = place.GetObject();
//    //            Place newPlace = new Place(placeObject["Name"].GetString(),
//    //                                    new Geopoint(new BasicGeoposition { Latitude = placeObject["Latitude"].GetNumber(), Longitude = placeObject["Longitude"].GetNumber() }),
//    //                                    (Kinds)Enum.Parse(typeof(Kinds), placeObject["Kind"].GetString()),
//    //                                    placeObject["Description"].GetString());
//    //            places.Add(newPlace);
//    //        }

//    //        return places;
//    //    }
//    //}

//    class PlaceOrign
//    {
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public Kinds Kind { get; set; }
//        public string Campus { get; set; }
//    }

//    [Table("Places")]
//    class PlaceData : PlaceOrign
//    {
//        public double Latitude { get;}
//        public double Longitude { get; }
//    }

//    class Place : PlaceOrign
//    {
//        private static Dictionary<string, List<Place>> places = new Dictionary<string, List<Place>>();

//        public Geopoint Position { get; set; }

//        public Place(string name, Geopoint position, string description,Kinds kind, string campus)
//        {
//            this.Name = name;
//            this.Position = position;
//            this.Description = description;
//            this.Kind = kind;
//            this.Campus = campus;
//        }

//        public static async Task<List<Place>> GetPlacesAsync(string campus)
//        {
//            if (places.ContainsKey(campus))
//                return places[campus];

//            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ms-appx:///DataModel/Data.db");
//            //var placesOrign = conn.Table<PlaceOrign>();
//           var querry = await (conn.Table<PlaceData>()).ToListAsync();
//           foreach (var placeOrign in querry)
//            {
//                places[campus].Add(new Place(placeOrign.Name,
//                                        new Geopoint(new BasicGeoposition { Latitude = placeOrign.Latitude, Longitude = placeOrign.Longitude }),
//                                       placeOrign.Description, placeOrign.Kind, placeOrign.Campus));

//            }

//            return places[campus];
//        }

//    }
//}