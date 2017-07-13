using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WriteLetter.Views;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace WriteLetter
{
    
    sealed partial class App : Application
    {
        
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {                
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;                
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;                
                //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    
                }
                rootFrame.Navigate(typeof(YearView));
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {                    
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }                
                Window.Current.Activate();
            }
        }

        private bool isExit = false;
        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;
            if (rootFrame.CurrentSourcePageType.Name != "YearView")
            {
                if (rootFrame.CanGoBack && e.Handled == false)
                {
                    e.Handled = true;
                    rootFrame.GoBack();
                }
            }
            else if (e.Handled == false)
            {
                try
                {
                    StatusBar statusBar = StatusBar.GetForCurrentView();
                    statusBar.ShowAsync();
                    statusBar.ForegroundColor = Colors.White;
                    statusBar.BackgroundOpacity = 0.9;
                    statusBar.ProgressIndicator.Text = "再按一次返回键退出程序。";
                    statusBar.ProgressIndicator.ShowAsync();

                    //Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                    //Windows.Data.Xml.Dom.XmlNodeList elements = toastXml.GetElementsByTagName("text");
                    //elements[0].AppendChild(toastXml.CreateTextNode("再按一次返回键退出程序。"));
                    //ToastNotification toast = new ToastNotification(toastXml);                
                    //ToastNotificationManager.CreateToastNotifier().Show(toast);

                    if (isExit)
                    {
                        App.Current.Exit();
                    }
                    else
                    {
                        isExit = true;
                        Task.Run(async () =>
                        {
                            await Task.Delay(1500);
                            await rootFrame.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {

                                statusBar.ProgressIndicator.HideAsync();
                                statusBar.HideAsync();
                            });
                            isExit = false;
                        });
                        e.Handled = true;
                    }
                }
                catch (Exception)
                {

                    //throw;
                }
                
            }
        }

        private void RootFrame_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.GoBack:
                    var frame = Window.Current.Content as Frame;
                    if (frame != null&&frame.CanGoBack)
                    {
                        frame.GoBack();
                    }
                    break;
                default:
                    break;
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();            
            deferral.Complete();
        }
    }
}
