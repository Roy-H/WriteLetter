using AppCore;
using AppCore.Helper;
using AppCore.SDK.Controls;
using AppCore.SDK.Helper;
using AppCore.SDK.OneDrive;
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
                        if (OneDriveHelper.Instance.OneDriveClient == null)
                            OneDriveHelper.Instance.InitializeClient(OneDriveHelper.ClientType.ConsumerUwp);
                        return OneDriveHelper.Instance.OneDriveClient!=null;
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
                        return OneDriveHelper.Instance.OneDriveClient != null;
                    },
                    ExecuteCallback =async delegate
                    {
                        //DialogManager.Instance.ShowInfoDialog()
                        WaitProgressHelper.Instance.SetToBusy();
                        await DataManager.Instance.UploadDataToOneDrive();
                        WaitProgressHelper.Instance.UnSetToBusy();
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
                        return OneDriveHelper.Instance.OneDriveClient != null;
                    },
                    ExecuteCallback =async delegate
                    {
                        WaitProgressHelper.Instance.SetToBusy();
                        var data= await DataManager.Instance.GetDataFromOneDrive();

                        if (DataManager.Instance.Data != null)
                        {
                            DataManager.Instance.Data.YearViewModels = data.YearViewModels;
                        }
                        WaitProgressHelper.Instance.UnSetToBusy();
                    }
                };
            }
        }
        #endregion
    }
}
