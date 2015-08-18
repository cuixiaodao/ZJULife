using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app
// is first launched.

namespace ZJULife.Data
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class DataItem
    {
        public DataItem(String uniqueId, String title, String description, String imagePath, String dataPath)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Description = description;
            this.ImagePath = imagePath;
            this.DataPath = dataPath;
        }

        public string UniqueId { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string ImagePath { get; private set; }

        public string DataPath { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class DataGroup
    {
        public DataGroup(String uniqueId, String title)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Items = new ObservableCollection<DataItem>();
        }

        public string UniqueId { get; private set; }

        public string Title { get; private set; }

        public ObservableCollection<DataItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    ///
    /// SampleDataSource initializes with data read from a static json file included in the
    /// project.  This provides sample data at both design-time and run-time.
    /// </summary>
    public sealed class DataSource
    {
        private static DataSource _dataSource = new DataSource();

        private ObservableCollection<DataGroup> _groups = new ObservableCollection<DataGroup>();

        public ObservableCollection<DataGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<DataGroup>> GetGroupsAsync()
        {
          //  if (_dataSource.Groups.Count == 0)
                await _dataSource.GetDataAsync();

            return _dataSource.Groups;
        }

        public static async Task<DataGroup> GetGroupAsync(string uniqueId)
        {
          //  if (_dataSource.Groups.Count == 0)
                await _dataSource.GetDataAsync();

            var matches = _dataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Any()) return matches.First();
            return null;
        }

        public static async Task<DataItem> GetItemAsync(string uniqueId)
        {
           // if (_dataSource.Groups.Count == 0)              
                await _dataSource.GetDataAsync(); 
                       
            var matches = _dataSource.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Any()) return matches.First();
            return null;
        }

        public static async Task<List<DataItem>> GetRelativeItemsAsync(string key)
        {
            //if (_dataSource.Groups.Count == 0)
                await _dataSource.GetDataAsync();

            var matches = _dataSource.Groups.SelectMany(group => group.Items).Where((item) => item.Description.Contains(key));
            if (matches.Any()) return matches.ToList<DataItem>();
            return null;
        }

        private async Task GetDataAsync()
        {
            if (this._groups.Any())
                return;
           // this._groups.Clear();

            Uri dataUri = new Uri("ms-appx:///DataModel/Data.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Groups"].GetArray();            

            foreach (JsonValue groupValue in jsonArray)
            {
                JsonObject groupObject = groupValue.GetObject();
                DataGroup group = new DataGroup(groupObject["UniqueId"].GetString(),
                                                            groupObject["Title"].GetString());

                foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                {
                    JsonObject itemObject = itemValue.GetObject();
                    group.Items.Add(new DataItem(itemObject["UniqueId"].GetString(),
                                                       itemObject["Title"].GetString(),
                                                       itemObject["Description"].GetString(),
                                                       itemObject["ImagePath"].GetString(),
                                                       itemObject["DataPath"].GetString()));
                }
                this.Groups.Add(group);
            }
        }
    }
}