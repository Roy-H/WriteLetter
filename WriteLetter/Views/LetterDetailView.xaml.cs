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
using AppCore.SDK.Helper;
using AppCore;
using System.Diagnostics;
using System.Text;

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

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool isOk = true;
            try
            {
                var folder = await FolderHelper.Instance.PickupFolder();
                if (folder == null)
                    return;
                string stringToSave = ""+ ViewModel.Title+"\r\n"+ @"    " + ViewModel.Content+ "\r\n"+@"        "+ViewModel.TimeText;               
                //string fileName = folder.Path.TrimEnd('\\') + @"\" + ViewModel.Title + ViewModel.TimeText+".txt";
                var fileName = ViewModel.Title +"-"+ ViewModel.TimeText + ".txt";
                var file = await folder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.GenerateUniqueName);               
                await DataHelper.SaveTxtFile(file, stringToSave);

            }
            catch (Exception ex)
            {
                isOk = false;
                await DialogManager.Instance.ShowConfirmDialog(string.Empty, Strings.IDS_SAVE_ERROR);
                Debug.WriteLine(ex.Message);
            }
            if(isOk)
                await DialogManager.Instance.ShowConfirmDialog(string.Empty, Strings.IDS_SAVE_SUCCESSFULLY);
        }

       

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var command1 = new Windows.UI.Popups.UICommand(Strings.IDS_OK, async uiCommand => {
                var frame = Window.Current.Content as Frame;
                //var month = DataManager.GetMonthViewModelByTime(ViewModel.Time);
                frame.GoBack();
                await DataManager.Instance.DeleteLetterAndSave(ViewModel);
                DataManager.Instance.Data.OnPropertyChanged("YearViewModels");

            });
            var command2 = new Windows.UI.Popups.UICommand(Strings.IDS_CANCEL, uiCommand => { });
            DialogManager.Instance.ShowInfoDialog(
                Strings.IDS_DELETE_CONFIRM_TITLE,
                Strings.IDS_DELETE_WARNING,
                command1,
                command2
                );

            //var msgDialog = new Windows.UI.Popups.MessageDialog(Strings.IDS_DELETE_WARNING) { Title = Strings.IDS_DELETE_CONFIRM_TITLE };
            //msgDialog.Commands.Add(new Windows.UI.Popups.UICommand(Strings.IDS_OK,async uiCommand => {
            //    var frame = Window.Current.Content as Frame;                
            //    //var month = DataManager.GetMonthViewModelByTime(ViewModel.Time);
            //    frame.GoBack();
            //    await DataManager.Instance.DeleteLetterAndSave(ViewModel);
            //    DataManager.Instance.Data.OnPropertyChanged("YearViewModels");

            //}));
            //msgDialog.Commands.Add(new Windows.UI.Popups.UICommand(Strings.IDS_CANCEL, uiCommand => { }));
            //await msgDialog.ShowAsync();
        }
    }
}
