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
                        return true;
                    },
                    ExecuteCallback =async delegate
                    {
                        if (OneDriveHelper.Instance.OneDriveClient == null)
                        {
                            WaitProgressHelper.Instance.SetToBusy();
                            await OneDriveHelper.Instance.InitializeClient(OneDriveHelper.ClientType.ConsumerUwp);
                            WaitProgressHelper.Instance.UnSetToBusy();
                        }                           
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
                    ExecuteCallback =async delegate
                    {
                        //DialogManager.Instance.ShowInfoDialog()
                        WaitProgressHelper.Instance.SetToBusy();
                        if (OneDriveHelper.Instance.OneDriveClient == null)
                        {
                            await OneDriveHelper.Instance.InitializeClient(OneDriveHelper.ClientType.ConsumerUwp);
                        }
                        if (OneDriveHelper.Instance.OneDriveClient == null)
                        {
                            WaitProgressHelper.Instance.UnSetToBusy();
                            return;
                        }
                            
                        await DataManager.Instance.UploadDataToOneDrive();
                        await DialogManager.Instance.ShowConfirmDialog(Strings.IDS_INFO, Strings.IDS_UPLOAD_CLOUD_DATA_OK);
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
                        return true;
                    },
                    ExecuteCallback =async delegate
                    {
                        WaitProgressHelper.Instance.SetToBusy();
                        if (OneDriveHelper.Instance.OneDriveClient == null)
                        {
                            await OneDriveHelper.Instance.InitializeClient(OneDriveHelper.ClientType.ConsumerUwp);
                        }
                        if (OneDriveHelper.Instance.OneDriveClient == null)
                        {
                            WaitProgressHelper.Instance.UnSetToBusy();
                            return;
                        }
                            
                        var data= await DataManager.Instance.GetDataFromOneDrive();
                        if (data == null)
                        {
                            //inform user fail to load data
                            await DialogManager.Instance.ShowConfirmDialog(Strings.IDS_WARNING,Strings.IDS_LOAD_CLOUD_DATA_FAIL);
                            WaitProgressHelper.Instance.UnSetToBusy();
                            return;
                        }
                        //load data
                        DataManager.Instance.Data = data;
                        DataManager.Instance.OnDataChanged();
                        await DialogManager.Instance.ShowConfirmDialog(Strings.IDS_INFO, Strings.IDS_LOAD_CLOUD_DATA_OK);
                        WaitProgressHelper.Instance.UnSetToBusy();
                    }
                };
            }
        }
        #endregion
    }
}
