using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [DataContract]
    public class MonthViewModel: ViewModelBase
    {
        public MonthViewModel(DateTime dateTime)
        {
            Time = dateTime;
        }
        [DataMember]
        private string yearText = string.Empty;
        
        public string YearText
        {
            get
            {
                if (yearText == null)
                    yearText = DateToChineseHelper.GetChineseDigit(Year) + "年";
                return yearText;
            }
            
        }
        [DataMember]
        private string monthText = string.Empty;
        
        public string MonthText
        {
            get
            {
                if (string.IsNullOrEmpty(monthText))
                    monthText = DateToChineseHelper.GetChineseNumber(Month) + "月";
                return monthText;
            }
        }
        
        public int Year { get { return Time.Year; } }


        public int Month { get { return Time.Month; } }

        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        private ObservableCollection<LetterViewModel> letters = new ObservableCollection<LetterViewModel>();
        
        public ObservableCollection<LetterViewModel> Letters
        {
            get
            { return letters; }
            set
            {
                letters = value;
            }
        }

        
        public ICommand EnterMonthCommand
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
                        //if(letters.Count == 0)
                         //   letters.Add(new LetterViewModel() { Title = "钗头凤", Content = "红酥手，黄藤酒，满城春色宫墙柳。" });
                        var frame = Window.Current.Content as Frame;
                        frame.Navigate(typeof(LetterView), this);
                    }
                };
            }
        }

        
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
                        var frame = Window.Current.Content as Frame;
                        frame.Navigate(typeof(EditingView),new LetterViewModel() { LetterEidtType = EidtType.Create,Time = DateTime.Now});
                    }
                };
            }
        }
    }
}
