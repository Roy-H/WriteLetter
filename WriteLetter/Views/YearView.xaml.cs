using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
using Windows.Storage.Pickers;
using AppCore.SDK.OneDrive;
using AppCore.SDK.Controls;

namespace WriteLetter.Views
{
    
    public sealed partial class YearView : Page
    {
        private int adCount;
        private Grid cloudSyncControlPlaceHolder => CloudSyncControlPlaceHolder;
        public DataViewModel Data
        {
            get
            {
                return DataManager.Instance.Data;
            }
            set
            {
                DataManager.Instance.Data = value;
            }
        }        

        public YearView()
        {
            this.Loaded += YearView_Loaded;
            this.Unloaded += YearView_Unloaded;
            this.InitializeComponent();
            Initialize();
            
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            //NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void YearView_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveCloudSyncControl();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //if(e.Parameter != null)
        }

        private void Initialize()
        {            
            Load();
        }

        private async void Load()
        {
            try
            {
                var data = await DataManager.Instance.LoadData();
                if (data is DataViewModel)
                {
                    Data = data as DataViewModel;
                }
                if (Data == null)
                    Data = new DataViewModel();
            }
            catch (Exception)
            {
                var msgDialog = new Windows.UI.Popups.MessageDialog("之前的数据将会丢失") { Title = "载入数据失败" };
                msgDialog.Commands.Add(new Windows.UI.Popups.UICommand("确定", uiCommand => {}));
                await msgDialog.ShowAsync();
                Data = new DataViewModel();
            }
            if (Data.YearViewModels.Count == 0)
                Data.AddYear(new YearViewModel(DateTime.Now));
            this.DataContext = Data;
            
        }

        private void YearView_Loaded(object sender, RoutedEventArgs e)
        {

            AddCloudSyncControl();
        }

        

        private void OnErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            NotifyUser($"An error occurred. {e.ErrorCode}: {e.ErrorMessage}", NotifyType.ErrorMessage);
        }

        private void OnAdRefreshed(object sender, RoutedEventArgs e)
        {
            adCount++;
            NotifyUser($"Advertisement #{adCount}", NotifyType.StatusMessage);
        }
        public void NotifyUser(string strMessage, NotifyType type)
        {
            // If called from the UI thread, then update immediately.
            // Otherwise, schedule a task on the UI thread to perform the update.
            //if (Dispatcher.HasThreadAccess)
            //{
            //    UpdateStatus(strMessage, type);
            //}
            //else
            //{
            //    var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => UpdateStatus(strMessage, type));
            //}
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {

        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void uploadfile_Click(object sender, RoutedEventArgs e)
        {
            
            var picker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };
            picker.FileTypeFilter.Add("*");
            var file = await picker.PickSingleFileAsync();
            using (var stream = await file.OpenStreamForReadAsync())
            {
                //var item = await _client.Drive.Special.AppRoot.ItemWithPath(file.Name).Content.Request().PutAsync<Item>(stream);  // Save for the GetLink demo  _savedId = item.Id;}
                var item = await OneDriveHelper.Instance.UpLoadFile(stream, file.Name);
            }
        }

        private void savefile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void makefolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void download_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Cloud Sync Control
        private void AddCloudSyncControl()
        {
            cloudSyncControlPlaceHolder.Children.Clear();
            var control = OneDriveHelper.Instance.GetCloudSyncControl();
            if (control == null)
                throw new Exception("cannot find cloudSyncControl");
            control.DataContext = new CloudSyncControlViewModel();
            cloudSyncControlPlaceHolder.Children.Add(control);
        }

        private void RemoveCloudSyncControl()
        {
            cloudSyncControlPlaceHolder.Children.Clear();
        }
        #endregion

    }
    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    }
}
