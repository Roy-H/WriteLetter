using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WriteLetter.ViewModels
{
    [DataContract]
    public class DataViewModel:ViewModelBase
    {
        [DataMember]
        private string dataVersion = "1.0.0";

        //[DataMember]
        //private DateTime lastestSaveTime = DateTime.Now;

        public string Version
        {
            get
            {
                return dataVersion;
            }
        }

        public DataViewModel()
        {
            //if (yearViewModels == null)
            //{
            //    yearViewModels = new ObservableCollection<YearViewModel>();
            //    yearViewModels.Add(new YearViewModel() {Time = DateTime.Now });
        }
        [DataMember]
        private ObservableCollection<YearViewModel> yearViewModels = new ObservableCollection<YearViewModel>();

        public ObservableCollection<YearViewModel> YearViewModels
        {
            get
            {
                return yearViewModels;
            }
            set
            {
                yearViewModels = value;
            }
        }
        
        public void AddYear(YearViewModel year)
        {
            yearViewModels.Insert(0,year);
            OnPropertyChanged("YearViewModels");
        }

        public int ItemsCount
        {
            get
            {
                int count = 0;
                foreach (var year in yearViewModels)
                {
                    foreach (var month in year.MonthViewModels)
                    {
                       count += month.Letters.Count();
                    }
                }
                return count;
            }
        }

        public void Update()
        {
            OnPropertyChanged(nameof(YearViewModels));
        }
    }
}
