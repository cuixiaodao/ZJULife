using System;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ZJULife.Common;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ZJULife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public About()
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
            ShowLogoStoryboard.Begin();
            ShowContentStoryboard.Begin();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration

        //private async void FeedBackGrid_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    Windows.ApplicationModel.Email.EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();
        //    mail.Subject = "ZJU life Windows客户端问题反馈与功能建议";
        //    mail.Body = "感谢回复！/n";
        //    mail.To.Add(new Windows.ApplicationModel.Email.EmailRecipient("3120103843@zju.edu.cn", "cuichao"));
        //    await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(mail);
        //}

        //private void FeedBackGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{
        //    FeedBackGridPressedStoryboard.Begin();
        //}

        //private void FeedBackGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    FeedbackGridReleasedStoryboard.Begin();
        //}

        //private async void CommentGrid_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    await Windows.System.Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:reviewapp?appid={0}", CurrentApp.AppId)));
        //    //await Windows.System.Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:reviewapp")));
        //}

        //private void CommentGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{
        //    CommentGridPressedStoryboard.Begin();
        //}

        //private void CommentGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    CommentGridReleasedStoryboard.Begin();
        //}
    }
}