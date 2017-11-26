using AppCore;
using AppCore.Helper;
using AppCore.SDK.Controls;
using AppCore.SDK.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace WriteLetter.ViewModels
{
    public class CloudSyncControlViewModel:ViewModelBase, ICloudSyncControlViewModel
    {
        #region ICommand
        public ICommand CreateOneCommand
        {
            get
            {
                return new ViewCommand()
                {
                    CanExecuteCallback = delegate
                    {
                        return true;
                    },
                    ExecuteCallback = delegate
                    {
                        
                    }
                };
            }
        }

        public ICommand CloudButtonPressCommand
        {
            get
            {
                return new ViewCommand()
                {
                    CanExecuteCallback = delegate
                    {
                        return true;
                    },
                    ExecuteCallback = delegate
                    {

                    }
                };
            }
        }
        public ICommand UpLoadItemCommand
        {
            get
            {
                return new ViewCommand()
                {
                    CanExecuteCallback = delegate
                    {
                        return true;
                    },
                    ExecuteCallback = delegate
                    {
                        //DialogManager.Instance.ShowInfoDialog()

                    }
                };
            }
        }
        
        public ICommand DownLoadItemCommand
        {
            get
            {
                return new ViewCommand()
                {
                    CanExecuteCallback = delegate
                    {
                        return true;
                    },
                    ExecuteCallback = delegate
                    {
                        var a = Strings.IDS_APP_NAME;
                    }
                };
            }
        }
        #endregion
    }
}
