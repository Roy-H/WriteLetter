using AppCore.SDK.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using WriteLetter.ViewModels;

namespace AppCore.SDK.Helper
{
    public class WaitProgressHelper :ViewModelBase
    {
        private static WaitProgressHelper instance;
        private static object syncRoot = new object();
        public static WaitProgressHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new WaitProgressHelper();
                        }
                    }
                }
                return instance;
            }
        }

        public void SetToBusy()
        {
            IsBusy = true;
        }

        public void UnSetToBusy()
        {
            IsBusy = false;
        }

        public WaitProgressHelper()
        {
            IsBusy = false;
        }

        //public bool IsBusy
        //{
        //    get { return (bool)GetValue(IsBusyProperty); }
        //    set { SetValue(IsBusyProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty IsBusyProperty =
        //    DependencyProperty.Register("IsBusy", typeof(bool), typeof(WaitProgressHelper), new PropertyMetadata(null, OnPropertyChanged));

        //private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if((bool)e.OldValue == true)
        //    {

        //    }
        //}
        private bool isBusy = false;
        public bool IsBusy
        {
            get
            {
                //make sure it is in single thread
                lock (syncRoot)
                {
                    return isBusy;
                }
            }
            set
            {
                //make sure it is in single thread
                lock (syncRoot)
                {
                    if (value != isBusy)
                    {
                        isBusy = value;
                        OnPropertyChanged(nameof(IsBusy));
                    }
                }                
            }
        }
    }
}
