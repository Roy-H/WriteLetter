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
using WriteLetter.ViewModels;

namespace WriteLetter.Views
{
    public sealed partial class LetterView : Page
    {
        public LetterView()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        MonthViewModel viewModel = null;

        //The Parameter is MonthViewModel
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            //SystemNavigationManager.GetForCurrentView().BackRequested += LetterView_BackRequested;
            if (e.Parameter != null)
            {
                viewModel = e.Parameter as MonthViewModel;
                this.DataContext = viewModel;
            }
            base.OnNavigatedTo(e);
        }

        private void LetterView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            var frame = Window.Current.Content as Frame;
            if (frame != null&& frame.CanGoBack)
            {
                frame.GoBack();   
            }
        }
    }
}
