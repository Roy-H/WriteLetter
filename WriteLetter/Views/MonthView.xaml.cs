using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WriteLetter.Helper;
using WriteLetter.ViewModels;

namespace WriteLetter.Views
{    
    public sealed partial class MonthView : Page
    {
        public MonthView()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        YearViewModel ViewModel = null;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            //SystemNavigationManager.GetForCurrentView().BackRequested += MonthView_BackRequested; ;
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                ViewModel = (YearViewModel)e.Parameter;
                this.DataContext = ViewModel;                
            }
        }

        private void MonthView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            var frame = Window.Current.Content as Frame;
            if (frame != null && frame.CanGoBack)
            {
                frame.GoBack();
            }
        }
    }
}
