using System;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Store;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZJULife.Common;
using ZJULife.Data;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace ZJULife
{
    public sealed partial class PivotPage : Page
    {
        private const string FirstGroupName = "FirstGroup";
        private const string SecondGroupName = "SecondGroup";
        private const string ThirdGroupName = "ThirdGroup";
        private const string ForthGroupName = "ForthGroup";

        private int previousSelectedIndex = 1;
        private int currentSelectedIndex = 1;
        private string[] ClassName = { "新生须知", "交通", "学习", "医食住购" };
        private Brush selectedColor = new SolidColorBrush(Color.FromArgb(255, 236, 240, 241));
        private Brush unSelectedColor = new SolidColorBrush(Color.FromArgb(255, 134, 132, 132));

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

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
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var dataGroup = await DataSource.GetGroupAsync("Group-1");
            this.DefaultViewModel[FirstGroupName] = dataGroup;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Loads the content for each pivot item when it is scrolled into view.
        /// </summary>
        ///
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var dataGroup = await DataSource.GetGroupAsync("Group-2");
            this.DefaultViewModel[SecondGroupName] = dataGroup;
        }

        private async void ThirdPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var dataGroup = await DataSource.GetGroupAsync("Group-3");
            this.DefaultViewModel[ThirdGroupName] = dataGroup;
        }

        private async void ForThPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var dataGroup = await DataSource.GetGroupAsync("Group-4");
            this.DefaultViewModel[ForthGroupName] = dataGroup;
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((DataItem)e.ClickedItem).UniqueId;
            switch (itemId.ToString())
            {
                case "Group-2-Item-2":
                    if (!Frame.Navigate(typeof(BusQuery)))
                    {
                        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;

                case "Group-2-Item-1":
                    if (!Frame.Navigate(typeof(Map)))
                    {
                        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;

                default:
                    if (!Frame.Navigate(typeof(ItemPage), itemId))
                    //if (!Frame.Navigate(typeof(BusQuery)))
                    {
                        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                    }
                    break;
            }
        }

        /// <summary>
        /// Loads the content for the second pivot item when it is scrolled into view.
        /// </summary>

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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Application.Current.Exit();
        }

        #endregion NavigationHelper registration

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            previousSelectedIndex = currentSelectedIndex;
            currentSelectedIndex = pivot.SelectedIndex;
            ClassNameTextBlock.Text = ClassName[currentSelectedIndex];
            changeClassIconColor(previousSelectedIndex, unSelectedColor);
            changeClassIconColor(currentSelectedIndex, selectedColor);  
            ChangeClassName.Begin();
        }

        private void changeClassIconColor(int index, Brush brush)
        {
            switch (index)
            {
                case 0:
                    this.ClassIcon1.Fill = brush;
                    break;

                case 1:
                    this.ClassIcon2.Fill = brush;
                    break;

                case 2:
                    this.ClassIcon3.Fill = brush;
                    break;

                case 3:
                    this.ClassIcon4.Fill = brush;
                    break;

                default:
                    break;
            }
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(SearchPage)))            
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SearchIconPressedStoryboard.Begin();
        }

        private void Image_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            SearchIconReleasedStoryboard.Begin();
        }

        private async void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:reviewapp?appid={0}", CurrentApp.AppId)));
        }

        private async void FeedBackButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Email.EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();
            mail.Subject = "ZJULife Windows客户端问题反馈与功能建议";
            mail.Body = "感谢参与！";
            mail.To.Add(new Windows.ApplicationModel.Email.EmailRecipient("cuichao@zju.edu.cn", "cuichao"));
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(About)))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void MoreAppButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(AppRecommend)))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }
    }
}