﻿

#pragma checksum "D:\OneDrive\Documents\GitRepos\ZJULife\ZJULife\PivotPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CCC08DF342F272C3B9A62603D9E16FA0"
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
    partial class PivotPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 16 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.FeedBackButton_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 17 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.CommentButton_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 18 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AboutButton_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 19 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.MoreAppButton_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 96 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Pivot)(target)).SelectionChanged += this.pivot_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 132 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 163 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.SecondPivot_Loaded;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 185 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.ThirdPivot_Loaded;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 207 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.ForThPivot_Loaded;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 218 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 197 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 12:
                #line 80 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Image_Tapped;
                 #line default
                 #line hidden
                #line 80 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.Image_PointerPressed;
                 #line default
                 #line hidden
                #line 80 "..\..\..\PivotPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerReleased += this.Image_PointerReleased;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


