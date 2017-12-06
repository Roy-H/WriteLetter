using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using WriteLetter.ViewModels;

namespace AppCore.Helper
{
    public class DataManager
    {
        private static DataManager instance;

        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataManager();
                }
                return instance;
            }
        }

        public DataViewModel Data { get; set; }

        const string fileName = "data.json";
        const string fileNameOneDrive = "onedrive_backup.json";
        const string version = "1.0.0";
        const string newFileName = "data_" + version + ".json";

        public async Task SaveData(DataViewModel data)
        {
            //var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            //var a = resourceLoader.GetString("Farewell");
            
            if (data != null)
            {
                await DataHelper.Save(typeof(DataViewModel), data, fileName);
            }
            else if (Data != null)
            {
                await DataHelper.Save(typeof(DataViewModel), Data, fileName);
            }
            else
            {
                Data = new DataViewModel();
                await DataHelper.Save(typeof(DataViewModel), Data, fileName);
            }

            
        }

        public async Task LoadData()
        {
            object data = null;
            try
            {
                var fileExist = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName) as StorageFile;
                if (fileExist != null)
                {
                    data = await DataHelper.Load(typeof(DataViewModel), fileName);
                }

                if (data != null && ((DataViewModel)data) != null)
                {
                    Data = data as DataViewModel;
                    Data.Update();
                }
            }
            catch (Exception)
            {
                var msgDialog = new Windows.UI.Popups.MessageDialog(Strings.IDS_THE_FORMER_DATA_LOST) { Title = Strings.IDS_FAIL_TO_LOAD_DATA };
                msgDialog.Commands.Add(new Windows.UI.Popups.UICommand(Strings.IDS_OK, uiCommand => { }));
                await msgDialog.ShowAsync();
                Data = new DataViewModel();
                if (Data.YearViewModels.Count == 0)
                    Data.AddYear(new YearViewModel(DateTime.Now));
            }
            
        }

        public async Task<DataViewModel> GetDataFromOneDrive()
        {
            DataViewModel dataViewModel = null;
            var data = await DataHelper.LoadFromOneDrive(typeof(DataViewModel), fileNameOneDrive);
            if (data != null && (data is DataViewModel))
            {
                dataViewModel = data as DataViewModel;
            }
            return dataViewModel;
        }

        public async Task<bool> UploadDataToOneDrive()
        {
            //DataViewModel dataViewModel = null;
            var result = await DataHelper.UpLoadToOneDrive(typeof(DataViewModel), Data, fileNameOneDrive);
            
            return result;
        }

        public MonthViewModel GetMonthViewModelByTime(DateTime time)
        {
            foreach (var year in Data.YearViewModels)
            {
                if(year.Year == time.Year)
                {
                    foreach(var month in year.MonthViewModels)
                    {
                        if (time.Month == month.Month)
                            return month;
                    }
                }
            }
            return null;
        }

        public YearViewModel GetYearViewModelByTime(DateTime time)
        {
            foreach (var year in Data.YearViewModels)
            {
                if (year.Year == time.Year)
                {
                    return year;
                }
            }
            return null;
        }

        public async Task DeleteLetterAndSave(LetterViewModel letter)
        {
            foreach(var year in Data.YearViewModels)
            {
                if(year.Year == letter.Time.Year)
                {
                    foreach(var month in year.MonthViewModels)

                    {
                        if(month.Month == letter.Time.Month)
                        {
                            if (month.Letters.Contains(letter))
                                month.Letters.Remove(letter);
                            break;
                        }
                    }
                }
            }
            await SaveData(null);
        }

        public async Task AddOneLetterToDataAndSave(LetterViewModel letter)
        {
            if (Data == null)
                Data = new DataViewModel();
            bool findedYear = false;
            bool findedMonth = false;
            YearViewModel yearFind = null;
            foreach (var year in Data.YearViewModels.ToList())
            {
                if (findedYear && findedMonth)
                    break;
                if (letter.Time.Year == year.Year)
                {
                    findedYear = true;
                    yearFind = year;
                    foreach (var month in year.MonthViewModels.ToList())
                    {
                        if (findedYear && findedMonth)
                            break;
                        if (letter.Time.Month == month.Month)
                        {
                            findedMonth = true;
                            month.Letters.Add(letter);
                        }
                    }
                }
            }
            if (!findedYear)
            {
                var newYear = new YearViewModel(letter.Time);
                Data.AddYear(newYear);
                var newMonth = new MonthViewModel(letter.Time);
                newYear.AddMonth(newMonth);
                newMonth.Letters.Add(letter);
                await SaveData(Data);
                return;
            }
            else if (!findedMonth)
            {
                var newMonth = new MonthViewModel(letter.Time);
                yearFind.AddMonth(newMonth);
                newMonth.Letters.Add(letter);
                await SaveData(Data);
                return;
            }
            else if (findedMonth && findedYear)
            {
                await SaveData(Data);
                return;
            }
        }
    }
}
