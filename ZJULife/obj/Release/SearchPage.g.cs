﻿

#pragma checksum "d:\OneDrive\documents\visual studio 2015\Projects\ZJULife\ZJULife\SearchPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "705AF97436C4F92DB68CCEAEF224FF1F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZJULife
{
    partial class SearchPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 167 "..\..\SearchPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ListView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 154 "..\..\SearchPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).GotFocus += this.SearchTextBox_GotFocus;
                 #line default
                 #line hidden
                #line 154 "..\..\SearchPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).KeyDown += this.SearchTextBox_KeyDown;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 156 "..\..\SearchPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.SearchIcon_Tapped;
                 #line default
                 #line hidden
                #line 156 "..\..\SearchPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.SearchIcon_PointerPressed;
                 #line default
                 #line hidden
                #line 156 "..\..\SearchPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerReleased += this.SearchIcon_PointerReleased;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


