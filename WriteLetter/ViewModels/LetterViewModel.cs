using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AppCore.Helper;
using WriteLetter.Views;

namespace WriteLetter.ViewModels
{
    [DataContractAttribute]
    public class LetterViewModel: ViewModelBase
    {
       
        EidtType editType = EidtType.Change;
        
        public EidtType LetterEidtType
        {
            get { return editType; }
            set { editType = value; }
        }
        [DataMember]
        private string title = string.Empty;
        [DataMember]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        [DataMember]
        private string content = string.Empty;
        [DataMember]
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
        [DataMember]
        public string Location { get; set; }

        [DataMember]
        private string timeText = string.Empty;

        public string TimeText
        {
            get
            {
                if(string.IsNullOrEmpty(timeText))
                    timeText = DateToChineseHelper.GetTimeText(Time);
                return timeText;
            }
        }

        public ICommand OpenLetterCommand
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
                        var frame = Window.Current.Content as Frame;
                        frame.Navigate(typeof(LetterDetailView), this);
                    }
                };
            }
        }

        public bool Save()
        {
            return true;
        }
    }
}
