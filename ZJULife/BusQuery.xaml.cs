using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ZJULife.BusHelper;
using ZJULife.Common;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ZJULife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BusQuery : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public BusQuery()
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            //载入用户常去地点
            Windows.Storage.ApplicationDataContainer localsettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localsettings.Values.ContainsKey("busKind"))
            {
                StartPoint.SelectedIndex = (int)localsettings.Values["startPoint"];
                EndPoint.SelectedIndex = (int)localsettings.Values["endPoint"];
                BusKind.SelectedIndex = (int)localsettings.Values["busKind"];
            }
            //预设出发时间
            TimeSpan now = DateTime.Now.TimeOfDay;
            TimeSpan earliestTime = TimeSpan.Parse("06:30");
            TimeSpan lastestTime = TimeSpan.Parse("23:30");

            MinTime.Time = (now < TimeSpan.Parse("23:10")) && (now > earliestTime) ? now : earliestTime;

            TimeSpan maxTime = MinTime.Time.Add(TimeSpan.Parse("02:00"));
            MaxTime.Time = maxTime < lastestTime ? maxTime : lastestTime;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            //存储用户常去地点
            Windows.Storage.ApplicationDataContainer localsettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (!(localsettings.Values.ContainsKey("busKind")
                  && (int)localsettings.Values["startPoint"] == StartPoint.SelectedIndex))
            {
                localsettings.Values["startPoint"] = StartPoint.SelectedIndex;
                localsettings.Values["endPoint"] = EndPoint.SelectedIndex;
                localsettings.Values["busKind"] = BusKind.SelectedIndex;
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            if (SearchResultGrid.Visibility == Visibility.Visible)
            {                
                BusSearchPanel.Visibility = Visibility.Visible;
                SearchResultGrid.Visibility = Visibility.Collapsed;
                ShowBusSearchPanelStoryboard.Begin();
            }
            else
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        #endregion NavigationHelper registration

        private void BusQueryIcon_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            BusQueryIconPressedStoryboard.Begin();
        }

        private void BusQueryIcon_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            BusQueryIconReleasedStoryboard.Begin();
        }

        //search available bus
        private async void BusQueryIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BusSearchPanel.Visibility = Visibility.Collapsed;
            SearchResultGrid.Visibility = Visibility.Visible;
            ShowSearchResultStoryboard.Begin();

            var results = await BusesDataSource.GetAvailableTripsAsync(((ComboBoxItem)StartPoint.SelectedItem).Content.ToString(),
             ((ComboBoxItem)EndPoint.SelectedItem).Content.ToString(),
            MinTime.Time, MaxTime.Time, ((ComboBoxItem)BusKind.SelectedItem).Content.ToString());

            DefaultViewModel["BusSearchResults"] = results;
            ResultsTextBlock.Text = results == null ? "无查询结果" : "查询结果";
        }

        private void ExchangeStartAndEndGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int temp = StartPoint.SelectedIndex;
            StartPoint.SelectedIndex = EndPoint.SelectedIndex;
            EndPoint.SelectedIndex = temp;
        }

        private void ExchangeStartAndEndGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ExchangeStartAndEndGridPressedStoryboard.Begin();
        }

        private void ExchangeStartAndEndGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ExchangeStartAndEndGridReleasedStoryboard.Begin();
        }
    }
}