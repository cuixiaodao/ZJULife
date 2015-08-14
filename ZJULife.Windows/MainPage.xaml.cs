using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ZJULife.Common;
using ZJULife.Data;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ZJULife
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private const string FirstGroupName = "FirstGroup";
        //private const string SecondGroupName = "SecondGroup";
        //private const string ThirdGroupName = "ThirdGroup";
        //private const string ForthGroupName = "ForthGroup";
        //private const string FifthGroupName = "FifthGroup";

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        //private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //var dataGroup = await DataSource.GetGroupAsync("Group-1");
            //this.DefaultViewModel[FirstGroupName] = dataGroup;

            //dataGroup = await DataSource.GetGroupAsync("Group-2");
            //this.DefaultViewModel[SecondGroupName] = dataGroup;

            //dataGroup = await DataSource.GetGroupAsync("Group-3");
            //this.DefaultViewModel[ThirdGroupName] = dataGroup;

            //dataGroup = await DataSource.GetGroupAsync("Group-4");
            //this.DefaultViewModel[ForthGroupName] = dataGroup;

            //dataGroup = await DataSource.GetGroupAsync("Group-5");
            //this.DefaultViewModel[FifthGroupName] = dataGroup;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="Common.SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="Common.NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        ///
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            var dataGroup = await DataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = dataGroup;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SearchIconPressedStoryboard.Begin();
        }

        private void Image_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            SearchIconReleasedStoryboard.Begin();
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter

            var itemId = ((DataItem)e.ClickedItem).UniqueId;
            switch (itemId.ToString())
            {
                //    case "Group-2-Item-2":
                //        if (!Frame.Navigate(typeof(BusQuery)))
                //        {
                //            throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                //        }
                //        break;
                //    case "Group-5-Item-1":
                //        if (!Frame.Navigate(typeof(About)))
                //        {
                //            throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                //        }
                //        break;
                //    case "Group-2-Item-1":
                //        if (!Frame.Navigate(typeof(Map)))
                //        {
                //            throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                //        }
                //        break;
                default:
                    if (!Frame.Navigate(typeof(ItemPage), itemId))
                    //if (!Frame.Navigate(typeof(Map)))
                    {
                        throw new Exception();
                    }
                    break;
            }
        }
    }
}