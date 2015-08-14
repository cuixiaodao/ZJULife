using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace ZJULife.BusHelper
{
    public class Trip
    {
        public string UniqueId { get; private set; }

        public string StartStop { get; private set; }

        public string PassedStop { get; private set; }

        public string EndStop { get; private set; }

        public TimeSpan StartTime { get; private set; }

        public TimeSpan EndTime { get; private set; }

        public string Note { get; private set; }

        public Trip(String uniqueId, String startStop, String passedStop, String endStop, TimeSpan startTime, TimeSpan endTime, string note)
        {
            this.UniqueId = uniqueId;
            this.StartStop = startStop;
            this.PassedStop = passedStop;
            this.EndStop = endStop;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Note = note;
        }
    }

    public class Bus
    {
        public Bus(String uniqueId, String name, string stops, string note)
        {
            this.UniqueId = uniqueId;
            this.Name = name;
            this.Stops = stops;
            this.Note = note;
            this.Trips = new List<Trip>();
        }

        public Bus(Bus bus, List<Trip> trips)
        {
            this.Name = bus.Name;
            this.Note = bus.Note;
            this.Trips = trips;
        }

        public string UniqueId { get; private set; }

        public string Name { get; private set; }

        public string Stops { get; private set; }

        public string Note { get; private set; }

        public List<Trip> Trips { get; private set; }
    }

    public sealed class BusesDataSource
    {
        private static BusesDataSource busesDataSource = new BusesDataSource();

        private List<Bus> buses = new List<Bus>();

        public List<Bus> Buses
        {
            get { return this.buses; }
        }

        //the following code is for checking whether the bus data has changed.
        //public static async Task<List<Bus>> GetAvailableTripsAsync(string startPoint, string endPoint, TimeSpan minTime, TimeSpan maxTime, string busKind)
        //{
        //    await busesDataSource.GetDataAsync();
        //    var matches = from bus in busesDataSource.Buses
        //              orderby bus.UniqueId
        //              select bus;

        //    return matches.ToList();
        //}

        //the following code is for getting available trips.
        public static async Task<List<Bus>> GetAvailableTripsAsync(string startPoint, string endPoint, TimeSpan minTime, TimeSpan maxTime, string busKind)
        {
            await busesDataSource.GetDataAsync();
            // Simple linear search is acceptable for small data sets
            //var matches = busesDataSource.Buses.SelectMany(group => group.Trips).Where((trip) => trip.StartStop.Contains("玉泉"));
            var matches = from bus in busesDataSource.Buses
                          where bus.Stops.Contains(startPoint) && bus.Stops.Contains(endPoint)
                          let trips = from trip in bus.Trips
                                      where !trip.EndStop.Contains(startPoint) && !trip.StartStop.Contains(endPoint)
                                            && trip.StartTime > minTime && trip.StartTime < maxTime
                                            && !(busKind == "学生车" && trip.Note.Contains("学生请勿"))
                                            //&& (!trip.Note.Contains("学生请勿") | busKind != "学生车")
                                            && ((trip.StartStop == startPoint && (trip.PassedStop.Contains(endPoint) | trip.EndStop == endPoint))
                                            | (trip.EndStop == endPoint && trip.PassedStop.Contains(startPoint)))
                                      select trip
                          select new Bus(bus, trips.ToList());
            matches = from bus in matches
                      where bus.Trips.Count != 0
                      select bus;

            if (matches.Count() != 0) return matches.ToList();
            return null;
        }

        private async Task GetDataAsync()
        {
            if (this.buses.Count != 0)
                return;

            Uri dataUri = new Uri("ms-appx:///BusHelper/Buses.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Buses"].GetArray();

            foreach (JsonValue busValue in jsonArray)
            {
                JsonObject busObject = busValue.GetObject();
                Bus bus = new Bus(busObject["UniqueId"].GetString(),
                                  busObject["Name"].GetString(),
                                  busObject["Stops"].GetString(),
                                  busObject["Note"].GetString());

                foreach (JsonValue tripValue in busObject["Trips"].GetArray())
                {
                    JsonObject tripObject = tripValue.GetObject();
                    bus.Trips.Add(new Trip(tripObject["UniqueId"].GetString(),
                                            tripObject["StartStop"].GetString(),
                                            tripObject["PassedStop"].GetString(),
                                            tripObject["EndStop"].GetString(),
                                            TimeSpan.Parse(tripObject["StartTime"].GetString()),
                                            TimeSpan.Parse(tripObject["EndTime"].GetString()),
                                            tripObject["Note"].GetString()));
                }
                this.Buses.Add(bus);
            }
        }
    }
}