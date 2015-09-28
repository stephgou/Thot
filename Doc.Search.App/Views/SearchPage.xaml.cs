using Doc.Search.App.Extensions;
using Doc.Search.App.ViewModels;
using Microsoft.Azure.AppService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Doc.Search.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private SearchViewModel _searchViewModel;

        public SearchPage()
        {
            this.InitializeComponent();
            _searchViewModel = new SearchViewModel();
            this.DataContext = _searchViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Dispatcher.AcceleratorKeyActivated += OnAcceleratorKeyActivated;
            base.OnNavigatedTo(e);
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Dispatcher.AcceleratorKeyActivated -= OnAcceleratorKeyActivated;
        }

        private void OnAcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            // Ensures the ENTER key always runs the same code as your default button.
            if (args.EventType == CoreAcceleratorKeyEventType.KeyDown && args.VirtualKey == VirtualKey.Enter)
            {
                _searchViewModel.LaunchSearch();
            }
        }
    }
}
