using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZJULife.Common;
using ZJULife.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZJULife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class mainPage : Page
    {
        private const string FirstGroupName = "FirstGroup";
        private const string SecondGroupName = "SecondGroup";
        private const string ThirdGroupName = "ThirdGroup";
        private const string ForthGroupName = "ForthGroup";
        private const string FifthGroupName = "FifthGroup";
        
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public mainPage()
        {
            this.InitializeComponent();

            loadState();
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private async void loadState()
        {
            var dataGroup = await DataSource.GetGroupAsync("Group-1");
            this.DefaultViewModel[FirstGroupName] = dataGroup;

            dataGroup = await DataSource.GetGroupAsync("Group-2");
            this.DefaultViewModel[SecondGroupName] = dataGroup;

            dataGroup = await DataSource.GetGroupAsync("Group-3");
            this.DefaultViewModel[ThirdGroupName] = dataGroup;

            dataGroup = await DataSource.GetGroupAsync("Group-4");
            this.DefaultViewModel[ForthGroupName] = dataGroup;

            dataGroup = await DataSource.GetGroupAsync("Group-5");
            this.DefaultViewModel[FifthGroupName] = dataGroup;

        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SearchIconPressedStotyboard.Begin();
        }

        private void Image_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            SearchIconReleasedStotyboard.Begin();
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
                //default:
                //    if (!Frame.Navigate(typeof(ItemPage), itemId))
                //    //if (!Frame.Navigate(typeof(Map)))
                //    {
                //        throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                //    }
                //    break;
            }

        }
    }
}
