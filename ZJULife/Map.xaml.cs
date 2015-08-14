using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ZJULife.Common;
using ZJULife.MapHelper;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ZJULife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Map : Page
    {
        private Geolocator geolocator = new Geolocator
        {
            DesiredAccuracy = PositionAccuracy.Default,
            MovementThreshold = 1
        };

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();        

        public Map()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            FindRouteGrid.Width=Window.Current.Bounds.Width;

            MyMap.ZoomLevel = 16;
            backToLocationAsync();
            
            Windows.Storage.ApplicationDataContainer localsettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localsettings.Values.ContainsKey(("myCampus")))
            {
                CampusComboBox.SelectedIndex = (int)localsettings.Values["myCampus"];
            }
            else
            {
                CampusComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            //存储用户校区
            Windows.Storage.ApplicationDataContainer localsettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (!(localsettings.Values.ContainsKey("myCampus")
                && (int)localsettings.Values["myCampus"] == CampusComboBox.SelectedIndex))
            {
                localsettings.Values["myCampus"] = CampusComboBox.SelectedIndex;
            }
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            geolocator.PositionChanged += GeolocatorPositionChanged;
            this.navigationHelper.OnNavigatedTo(e);
            await drawMapIconsAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
            geolocator.PositionChanged -= GeolocatorPositionChanged;
            geolocator = null;
        }

        private void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            backToLocationAsync();
        }

        private async void backToLocationAsync()
        {
            try
            {
                checkGeolocator();
                Geoposition point = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(1),
                    timeout: TimeSpan.FromSeconds(12));
                MyMap.ZoomLevel = 18;                
                await MyMap.TrySetViewAsync(GeopositionToGeoPoint(point), MyMap.ZoomLevel, MyMap.Heading, MyMap.Pitch, MapAnimationKind.Linear);
            }
            catch (Exception)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("抱歉定位遇到错误");
                await messageDialog.ShowAsync();
                return;
            }
        }

        private async void GeolocatorPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                GeolocatorPositionChanged(args.Position); // calls back to not shared portion
            });
        }

        private const int positionZIndewxz = 4;

        private void GeolocatorPositionChanged(Geoposition point)
        {
            var pos = GeopositionToGeoPoint(point);
            DrawPositionIcon(pos);
        }

        private void DrawPositionIcon(Geopoint pos)
        {
            var PositionIcon = MyMap.MapElements.OfType<MapIcon>().FirstOrDefault(p => p.ZIndex == positionZIndewxz);
            if (PositionIcon == null)
            {
                PositionIcon = new MapIcon
                {
                    NormalizedAnchorPoint = new Point(0.5, 0.5),
                    ZIndex = positionZIndewxz
                };
                PositionIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapIcons/Position.png"));
                MyMap.MapElements.Add(PositionIcon);
            }
            PositionIcon.Location = pos;
        }

        private async Task drawMapIconsAsync()
        {
            // How to draw a new MapIcon with a label, anchorpoint and custom  icon.
            // Icon comes from shared project assets
            var anchorPoint = new Point(0.5, 0.5);
            //var buildingImage = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapIcons/Building.png"));
            //var DormitoryImage = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapIcons/Dormitory.png"));

            List<Place> places = await Place.GetPlacesAsync();
            this.DefaultViewModel["Places"] = places;

            foreach (var dataObject in places)
            {
                ///////////////////////////////////////////////////
                // Creating the MapIcon if description is not null
                //   (text, image, location, anchor point)
                if (dataObject.Description != null)
                {
                    var shape = new MapIcon
                    {
                        Title = dataObject.Name,
                        Location = dataObject.Position,
                        NormalizedAnchorPoint = anchorPoint,
                        ZIndex = 5
                    };
                    shape.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapIcons/" + dataObject.Kind.ToString() + ".png"));
                    //switch (dataObject.Kind)
                    //{
                    //    case Kinds.Building:
                    //        shape.Image = buildingImage;
                    //        break;
                    //    case Kinds.Dorm:
                    //        shape.Image = DormitoryImage;
                    //        break;
                    //    default:
                    //        shape.Image = buildingImage;
                    //        break;
                    //}

                    shape.AddData(dataObject.Description);
                    MyMap.MapElements.Add(shape);
                }
            }
        }

        #endregion NavigationHelper registration

        private void PlacesButton_Click(object sender, RoutedEventArgs e)
        {
            MyPlacesGrid.Visibility = (MyPlacesGrid.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var position = ((Place)e.ClickedItem).Position;
            MyMap.ZoomLevel = 18;
            await MyMap.TrySetViewAsync(position, MyMap.ZoomLevel, MyMap.Heading, MyMap.Pitch, MapAnimationKind.Linear);
        }

        private void DrawRoute(MapRoute route)
        {
            var previousRoutes = MyMap.MapElements.Where(p => p.ZIndex == 2);
            if (previousRoutes.Any())
            {
                MyMap.MapElements.Remove(previousRoutes.First());
            }

            //Draw a semi transparent fat green line
            var color = Colors.Green;
            color.A = 128;
            var line = new MapPolyline
            {
                StrokeThickness = 11,
                StrokeColor = color,
                StrokeDashed = false,
                ZIndex = 2
            };

            // Route has a Path containing all points to draw the route
            line.Path = new Geopath(route.Path.Positions);
            MyMap.MapElements.Add(line);
        }

        //private bool DeleteShapesFromLevel(int zIndex)
        //{
        //    // Delete shapes by z-index
        //    var shapesOnLevel = MyMap.MapElements.Where(p => p.ZIndex == zIndex);
        //    if (shapesOnLevel.Any())
        //    {
        //        foreach (var shape in shapesOnLevel.ToList())
        //        {
        //            MyMap.MapElements.Remove(shape);
        //        }
        //        return true;
        //    }
        //    return false;
        //}

        private void PositionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //FindRouteTextBox.SelectAll();
            ((TextBox)e.OriginalSource).SelectAll();
        }

        private async void FindRouteTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (StartPositionTextBox.Text !=string.Empty && EndPositionTextBox.Text != string.Empty)
                {
                    var startPlace = findPlace(StartPositionTextBox.Text);
                    var endPlace = findPlace(EndPositionTextBox.Text);

                    if (startPlace == null)
                    {
                        await giveMessageAsync("未能在校内找到该起点");
                        return;
                    }

                    if (endPlace == null)
                    {
                        await giveMessageAsync("未能在校内找到该终点");
                        return;
                    }

                    StartPositionTextBox.Text = startPlace.Name;
                    EndPositionTextBox.Text = endPlace.Name;
                    
                    var routeResult = await MapRouteFinder.GetWalkingRouteAsync(startPlace.Position, endPlace.Position);
                    try
                    {
                        DrawRoute(routeResult.Route);
                        FindRouteFlyout.Hide();
                        MyMap.ZoomLevel = 16;
                        await MyMap.TrySetViewAsync(startPlace.Position, MyMap.ZoomLevel, MyMap.Heading, MyMap.Pitch, MapAnimationKind.Linear);
                    }
                    catch (Exception)
                    {
                        await giveMessageAsync("抱歉，未能找到路线，试试自带地图吧。");
                    }
                }
                else
                {
                    await giveMessageAsync("请输入起点、终点(仅限校内地点)");
                }
            }
        }

        private Place findPlace(string placeName)
        {
            Place foundPlace;
            var places = ((List<Place>)DefaultViewModel["Places"]);
            var allFoundPlaces = from place in places
                                 where place.Name.Contains(placeName)
                                 select place;

            //改进查询算法
            if (allFoundPlaces.Any())
            {
                foundPlace = allFoundPlaces.First();
                return foundPlace;
            }
            return null;           
        }

        private async Task giveMessageAsync(string message)
        {
            var messageDialog = new Windows.UI.Popups.MessageDialog(message);
            await messageDialog.ShowAsync();
            return;
        }

        //private async void FindRouteTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        //{
        //    if (e.Key == Windows.System.VirtualKey.Enter)
        //    {
        //        checkGeolocator();
        //        Geoposition point = await geolocator.GetGeopositionAsync();
        //        var startPosition = GeopositionToGeoPoint(point);
        //        var places = ((List<Place>)DefaultViewModel["Places"]);
        //        var allEndPosition = from place in places
        //                             where place.Name.Contains(FindRouteTextBox.Text)
        //                             select place.Position;

        //        //改进查询算法
        //        if (!allEndPosition.Any())
        //        {
        //            await findNoRouteAsync();
        //            return;                    
        //        }
        //        Geopoint endPosition = allEndPosition.First();
        //        //校外查询部分，效果不佳，暂时去除。
        //        //else
        //        //{
        //        //    var result = await MapLocationFinder.FindLocationsAsync(FindRouteTextBox.Text, startPosition, 3);
        //        //    if (!(result.Status == MapLocationFinderStatus.Success&&result.Locations.Any()))
        //        //    {
        //        //        await findNoRouteAsync();
        //        //        return;
        //        //    }

        //        //    endPosition = result.Locations.First().Point;
        //        //}

        //        var routeResult = await MapRouteFinder.GetWalkingRouteAsync(startPosition, endPosition);
        //        try
        //        {
        //            DrawRoute(routeResult.Route);
        //            FindRouteFlyout.Hide();
        //        }
        //        catch (Exception)
        //        {
        //            await findNoRouteAsync();
        //        }

        //        //原始查询算法
        //        //try
        //        //{
        //        //    endPosition = allEndPosition.First();
        //        //}
        //        //catch (Exception)
        //        //{
        //        //    try
        //        //    {
        //        //        var result = await MapLocationFinder.FindLocationsAsync(FindRouteTextBox.Text, startPosition, 1);
        //        //        endPosition = result.Locations.First().Point;
        //        //    }
        //        //    catch (Exception)
        //        //    {
        //        //        var messageDialog = new Windows.UI.Popups.MessageDialog("Sorry, can't find a route.");
        //        //        await messageDialog.ShowAsync();
        //        //        return;
        //        //    }
        //        //}

        //        //var routeResult = await MapRouteFinder.GetWalkingRouteAsync(startPosition, endPosition);
        //        //try
        //        //{
        //        //    DrawRoute(routeResult.Route);
        //        //    FindRouteFlyout.Hide();
        //        //}
        //        //catch (Exception)
        //        //{
        //        //    var messageDialog = new Windows.UI.Popups.MessageDialog("Sorry, can't find a route.");
        //        //    await messageDialog.ShowAsync();
        //        //    return;
        //        //}
        //    }
        //}

        private async void BingButton_Click(object sender, RoutedEventArgs e)
        {
            string uriToLaunch = @"bingmaps:?";
            var uri = new Uri(uriToLaunch);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void checkGeolocator()
        {
            if (geolocator == null)
            {
                geolocator = new Geolocator
                {
                    DesiredAccuracy = PositionAccuracy.Default,
                    MovementThreshold = 1
                };
                geolocator.PositionChanged += GeolocatorPositionChanged;
            }
        }

        //private Geopoint GeopositionToGeoPoint(Geoposition point)
        //{
        //    return new Geopoint(new BasicGeoposition { Latitude = point.Coordinate.Point.Position.Latitude - 0.0020070243, Longitude = point.Coordinate.Point.Position.Longitude + 0.0051115839 });
        //    //加减数仅是因为mapcontrol没有针对中国的火星坐标系进行纠偏，粗略调整
        //}

        //实验纠偏
        const double pi = 3.14159265358979324;
        const double a = 6378245.0;
        const double ee = 0.00669342162296594323;
        private Geopoint GeopositionToGeoPoint(Geoposition point)
        {
            double wgLon = point.Coordinate.Point.Position.Longitude;
            double wgLat = point.Coordinate.Point.Position.Latitude;
            double dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
            double dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
            double radLat = wgLat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            //double[] latlng = { wgLat + dLat, wgLon + dLon };
            //latlng[0] = wgLat + dLat;
            //latlng[1] = wgLon + dLon;

            return new Geopoint(new BasicGeoposition { Latitude = wgLat + dLat + 0.000355, Longitude = wgLon + dLon - 0.000067 });
            //加减数仅是因为mapcontrol没有针对中国的火星坐标系进行纠偏，粗略调整
        }
        private static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }
        private static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }

        //结束实验纠偏

        private List<KeyedList<string, Place>> linqGroupedList(IEnumerable<Place> places)
        {
            ///////////////////////////////////////////////////
            //   To create the list grouped by company name, we use
            //     a Linq query in which we
            //   1) state which property we want to group by (TargetGroup)
            //   2) give a temporary IGrouping variable to group the items into (itemsByCompany)
            //   3) select those groupings into an IEnumerable of KeyedLists
            //
            //   We've essentially just created a list of lists that
            //   our UI can understand for the sake of implementing the
            //   SemanticZoom control.
            ///////////////////////////////////////////////////
            var groupedPlaces =
                    from place in places
                    orderby place.Kind
                    group place by place.Kind.ToString() into PlacesByKind
                    select new KeyedList<string, Place>(PlacesByKind);
            return groupedPlaces.ToList();
        }

        private void MyMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            var mapObjectDescription = new StringBuilder();
            //resultText.AppendLine(string.Format("Position={0},{1}", args.Position.X, args.Position.Y));
            //resultText.AppendLine(string.Format("Location={0:F9},{1:F9}", args.Location.Position.Latitude, args.Location.Position.Longitude));

            foreach (var mapObject in sender.FindMapElementsAtOffset(args.Position))
            {
                mapObjectDescription.AppendLine(mapObject.ReadData<string>());
            }
            string description = mapObjectDescription.ToString();

            //flyout1.ShowAt(MyMap);
            if (description.Length > 3)
            {
                mapObjectDescriptionPopup.IsOpen = true;
                mapObjectDescriptionTextblock.Text = description;
            }
            else
            {
                mapObjectDescriptionPopup.IsOpen = false;
            }
        }

        private async void CampusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newPlaces = await Place.GetPlacesAsync(((ComboBoxItem)CampusComboBox.SelectedItem).Tag.ToString());
            GroupedPlaces.Source = linqGroupedList(newPlaces);
        }
    }
}