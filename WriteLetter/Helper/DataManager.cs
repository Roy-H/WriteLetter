using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using WriteLetter.ViewModels;

namespace WriteLetter.Helper
{
    public static class DataManager
    {
        public static DataViewModel Data;

        const string fileName = "data.json";

        static public async Task AddOneLetterToDataAndSave(LetterViewModel letter)
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
            else if (findedMonth&& findedYear)
            {
                await SaveData(Data);
                return;
            }
        }

        static public async Task SaveData(DataViewModel data)
        {
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

        static public async Task<object> LoadData()
        {
            object data = null;
            var fileExist = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName) as StorageFile;
            if (fileExist != null)
            {
                data = await DataHelper.Load(typeof(DataViewModel), fileName);
            }
            
            if (data!=null&&((DataViewModel)data) != null)
            {
                Data = data as DataViewModel;
            }
            return data;
        }

        static public MonthViewModel GetMonthViewModelByTime(DateTime time)
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

        static public YearViewModel GetYearViewModelByTime(DateTime time)
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

        static public async Task DeleteLetterAndSave(LetterViewModel letter)
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
    }
}
