using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AppCore.Helper;
using WriteLetter.Views;
using AppCore.SDK.OneDrive;

namespace WriteLetter.ViewModels
{
    [DataContract]
    public class YearViewModel: ViewModelBase
    {
        public YearViewModel(DateTime dateTime)
        {
            this.Time = dateTime;           
        }
        [DataMember]
        private string yearText;
        
        public string YearText
        {
            get
            {
                if(yearText == null)
                    yearText = DateToChineseHelper.GetChineseDigit(Year) + "年";
                return yearText;
            }
        }
                
        public int Year { get { return Time.Year; } }

        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        private ObservableCollection<MonthViewModel> monthViewModels= new ObservableCollection<MonthViewModel>();
        
        public ObservableCollection<MonthViewModel> MonthViewModels
        {
            get
            {
                return monthViewModels;
            }
        }

        public void AddMonth(MonthViewModel month)
        {
            monthViewModels.Insert(0,month);
            OnPropertyChanged("MonthViewModels");
        }

        private void EnterYear()
        {
            OneDriveHelper.Instance.InitializeClient(OneDriveHelper.ClientType.Consumer);
            if (monthViewModels.Count == 0)
            {
                monthViewModels.Add(new MonthViewModel(DateTime.Now));
            }
            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(MonthView), this);
        }
        #region ICommand
        
        public ICommand EnterYearCommand
        {
            get
            {
                return new ViewCommand()
                {
                    CanExecuteCallback =delegate
                    {
                        return true;
                    },
                    ExecuteCallback = delegate
                    {
                        Debug.WriteLine("call command");
                        EnterYear();
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
                        frame.Navigate(typeof(EditingView), new LetterViewModel() { LetterEidtType = EidtType.Create, Time = DateTime.Now });
                    }
                };
            }
        }
        #endregion
    }
}
