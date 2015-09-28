using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Doc.Search.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        private bool _hasLaunchedAuthentication = false;

        public ShellPage()
        {
            this.InitializeComponent();
            this.GotFocus += ShellPage_GotFocus;
        }

        private async void ShellPage_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!_hasLaunchedAuthentication)
            {
                _hasLaunchedAuthentication = true;
                await App.ViewModel.AuthenticateAsync();
                App.ViewModel.GetUserName();
            }
        }

        private void SideMenuRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShellSplitView.IsPaneOpen = !this.ShellSplitView.IsPaneOpen;
        }

        private void NavigateToPage(Type typeofPage)
        {
            var frame = this.DataContext as Frame;
            if (frame != null)
            {
                Page page = frame.Content as Page;
                if (page != null)
                {
                    if (page.GetType() != typeofPage)
                    {
                        frame.Navigate(typeofPage);
                    }
                }
            }
        }

        private void SearchRadioButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(SearchPage));
        }

        private void StorageRadioButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(StoragePage));
        }

        private void BackRadionButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = this.DataContext as Frame;
            if (frame?.CanGoBack == true)
            {
                frame.GoBack();
            }
        }
    }
}
