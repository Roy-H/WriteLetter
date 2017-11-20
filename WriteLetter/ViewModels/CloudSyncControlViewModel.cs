using AppCore;
using AppCore.SDK.Controls;
using AppCore.SDK.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
