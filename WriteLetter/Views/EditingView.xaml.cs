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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WriteLetter.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EditingView : Page
    {
        

        LetterViewModel viewModel = null;
        public EditingView()
        {
            this.InitializeComponent();
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            //SystemNavigationManager.GetForCurrentView().BackRequested += EditingView_BackRequested;
            if (e.Parameter != null)
            {
                viewModel = (LetterViewModel)e.Parameter;
                if (viewModel == null)
                {
                    editEnable = false;
                    return;
                }
                Title.Text = viewModel.Title;
                Content.Document.SetText(Windows.UI.Text.TextSetOptions.None,viewModel.Content);
            }
            base.OnNavigatedTo(e);
        }

        private void EditingView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                }
            }            
        }

        private bool editEnable = true;
        public bool EditEnable
        {
            get
            {
                return editEnable;
            }
        }

        private async void Finished_Click(object sender, RoutedEventArgs e)
        {
            switch (viewModel.LetterEidtType)
            {
                case EidtType.Change:
                    viewModel.Title = Title.Text;
                    string content = string.Empty;
                    Content.Document.GetText(Windows.UI.Text.TextGetOptions.None,out content);
                    viewModel.Content = content == null?string.Empty: content;
                    await DataManager.SaveData(null);
                    DataManager.Data.OnPropertyChanged("YearViewModels");
                    break;
                case EidtType.Create:
                    string content2 = string.Empty;
                    Content.Document.GetText(Windows.UI.Text.TextGetOptions.None, out content2);
                    viewModel.Content = content2 == null ? string.Empty : content2;
                    viewModel.Title = Title.Text;
                    viewModel.Time = DateTime.Now;
                    await DataManager.AddOneLetterToDataAndSave(viewModel);
                    DataManager.Data.OnPropertyChanged("YearViewModels");
                    break;
                default:
                    break;
            }
            var frame = Window.Current.Content as Frame;
            //var month = DataManager.GetMonthViewModelByTime(viewModel.Time);
            frame.GoBack();
        }
    }
    public enum EidtType
    {
        Change = 0,
        Create,
    }
}
