﻿

#pragma checksum "D:\OneDrive\Documents\Visual Studio 2015\Projects\ZJULife\ZJULife\SearchPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "705AF97436C4F92DB68CCEAEF224FF1F"
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
    partial class SearchPage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock ResultsTextBlock; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard SearchIconPressedStoryboard; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard SearchIconReleasedStoryboard; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBox SearchTextBox; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Image SearchIcon; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.TranslateTransform AnimatedTransform; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///SearchPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            ResultsTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("ResultsTextBlock");
            SearchIconPressedStoryboard = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("SearchIconPressedStoryboard");
            SearchIconReleasedStoryboard = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("SearchIconReleasedStoryboard");
            SearchTextBox = (global::Windows.UI.Xaml.Controls.TextBox)this.FindName("SearchTextBox");
            SearchIcon = (global::Windows.UI.Xaml.Controls.Image)this.FindName("SearchIcon");
            AnimatedTransform = (global::Windows.UI.Xaml.Media.TranslateTransform)this.FindName("AnimatedTransform");
        }
    }
}



