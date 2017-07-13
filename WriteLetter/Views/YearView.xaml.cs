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
using WriteLetter.Helper;
using WriteLetter.ViewModels;

namespace WriteLetter.Views
{
    
    public sealed partial class YearView : Page
    {
        
        public DataViewModel Data
        {
            get
            {
                return DataManager.Data;
            }
            set
            {
                DataManager.Data = value;
            }
        }        

        public YearView()
        {
            this.InitializeComponent();
            Initialize();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            //NavigationCacheMode = NavigationCacheMode.Enabled;
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
                var data = await DataManager.LoadData();
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
    }
}
