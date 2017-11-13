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
using AppCore.Helper;
using WriteLetter.ViewModels;

namespace WriteLetter.Views
{
    public sealed partial class LetterDetailView : Page
    {
        public LetterDetailView()
        {
            this.InitializeComponent();
        }
        LetterViewModel ViewModel;

        //The Parameter is LetterViewModel
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            //SystemNavigationManager.GetForCurrentView().BackRequested += LetterDetailView_BackRequested;
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                ViewModel = (LetterViewModel)e.Parameter;
                this.DataContext = ViewModel;
            }           
            
        }

        private void LetterDetailView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            //e.Handled = true;
            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                }
            }
        }

        private void Eidt_Click(object sender, RoutedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            ViewModel.LetterEidtType = EidtType.Change;
            frame.Navigate(typeof(EditingView), ViewModel);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        //private async void ShowMessageDialog()
        //{
            
           
        //    await msgDialog.ShowAsync();
        //}

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var msgDialog = new Windows.UI.Popups.MessageDialog("确定删除这封信吗？") { Title = "删除确认" };
            msgDialog.Commands.Add(new Windows.UI.Popups.UICommand("确定",async uiCommand => {
                var frame = Window.Current.Content as Frame;                
                //var month = DataManager.GetMonthViewModelByTime(ViewModel.Time);
                frame.GoBack();
                await DataManager.Instance.DeleteLetterAndSave(ViewModel);
                DataManager.Instance.Data.OnPropertyChanged("YearViewModels");

            }));
            msgDialog.Commands.Add(new Windows.UI.Popups.UICommand("取消", uiCommand => { }));
            await msgDialog.ShowAsync();
        }
    }
}
