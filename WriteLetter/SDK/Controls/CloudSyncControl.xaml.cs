using AppCore.SDK.OneDrive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppCore.SDK.Controls
{
    public sealed partial class CloudSyncControl : UserControl
    {
        public CloudSyncControl()
        {
            this.InitializeComponent();
            this.Loaded += CloudSyncControl_Loaded;
        }

        private void CloudSyncControl_Loaded(object sender, RoutedEventArgs e)
        {
            var state = OneDriveHelper.Instance.GetState();
            switch (state)
            {
                case OneDriveHelper.CloudSyncState.Offline:
                    break;
                case OneDriveHelper.CloudSyncState.Active:
                    IsButtonVisible = true;
                    break;
                case OneDriveHelper.CloudSyncState.Unavailable:
                    IsButtonVisible = false;
                    break;
                default:
                    break;
            }
        }

        public bool IsButtonVisible
        {
            get { return (bool)GetValue(IsButtonVisibleProperty); }
            set { SetValue(IsButtonVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsButtonVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsButtonVisibleProperty =
            DependencyProperty.Register("IsButtonVisible", typeof(bool), typeof(CloudSyncControl), new PropertyMetadata(false));

    }

    public interface ICloudSyncControlViewModel
    {
        ICommand CloudButtonPressCommand { get; }

        ICommand UpLoadItemCommand { get; }

        ICommand DownLoadItemCommand { get; }
    }
}
