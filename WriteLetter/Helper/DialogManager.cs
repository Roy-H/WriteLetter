using AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace AppCore.Helper
{
    public class DialogManager
    {
        private static DialogManager instance;
        private static object sR = new object();
        public static DialogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sR)
                    {
                        if (instance == null)
                        {
                            instance = new DialogManager();
                        }
                    }
                }
                return instance;
            }
        }


        public async void ShowInfoDialog(string title,string content,IUICommand commandOK, IUICommand commandCancel)
        {
            var msgDialog = new Windows.UI.Popups.MessageDialog(content) { Title = title };
            msgDialog.Commands.Add(commandOK);
            msgDialog.Commands.Add(commandCancel);
            await msgDialog.ShowAsync();
        }
        public async void ShowConfirmDialog(string title, string content)
        {
            var msgDialog = new Windows.UI.Popups.MessageDialog(content) { Title = title };
            msgDialog.Commands.Add(new UICommand(Strings.IDS_OK));
            //msgDialog.Commands.Add(commandCancel);
            await msgDialog.ShowAsync();
        }
    }
}
