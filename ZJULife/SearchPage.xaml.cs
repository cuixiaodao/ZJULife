using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ZJULife.Common;
using ZJULife.Data;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ZJULife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public SearchPage()
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
            if (e.PageState != null)
            {
                if (e.PageState.ContainsKey("SearchTextBoxText"))
                {
                    SearchTextBox.Text = e.PageState["SearchTextBoxText"].ToString();
                }
                if (e.PageState.ContainsKey("PreviousSearchResult"))
                {
                    this.DefaultViewModel["RelativeItems"] = (e.PageState["PreviousSearchResult"]);
                }
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
            e.PageState["SearchTextBoxText"] = SearchTextBox.Text;
            if (this.DefaultViewModel.ContainsKey("RelativeItems"))
            {
                e.PageState["PreviousSearchResult"] = this.DefaultViewModel["RelativeItems"];
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((DataItem)e.ClickedItem).UniqueId;
            switch (itemId.ToString())
            {
                case "Group-2-Item-1":
                    if (!Frame.Navigate(typeof(Map)))
                    {
                        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;

                case "Group-2-Item-2":
                    if (!Frame.Navigate(typeof(BusQuery)))
                    {
                        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;

                default:
                    if (!Frame.Navigate(typeof(ItemPage), itemId))
                    {
                        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;
            }
        }

        private void SearchIcon_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SearchIconPressedStoryboard.Begin();
        }

        private void SearchIcon_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            SearchIconReleasedStoryboard.Begin();
        }

        private async void SearchIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var items = await DataSource.GetRelativeItemsAsync(SearchTextBox.Text);
            this.DefaultViewModel["RelativeItems"] = items;
            if (items == null)
            {
                ResultsTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                ResultsTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TappedRoutedEventArgs TappedArgs_e = new TappedRoutedEventArgs();
                TappedEventHandler handler = SearchIcon_Tapped;
                if (handler != null)
                {
                    handler(this, TappedArgs_e);
                }
                //InputPane.GetForCurrentView().TryHide();
                //ResultsTextBlock.Focus(FocusState.Programmatic);
                //var items = await DataSource.GetRelativeItemsAsync(SearchTextBox.Text);
                //this.DefaultViewModel["RelativeItems"] = items;
                //if (items == null)
                //{
                //    ResultsTextBlock.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    ResultsTextBlock.Visibility = Visibility.Collapsed;
                //}
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //SearchTextBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
            SearchTextBox.SelectAll();
        }

        //private void SearchTextBox_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    this.Focus(FocusState.Programmatic);
        //    InputPane.GetForCurrentView().TryShow();
        //}
    }
}