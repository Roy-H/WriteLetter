using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteLetter.Helper
{
    
    public class DateToChineseHelper
    {
        static IDictionary<int, string> ToChineseDigit = new Dictionary<int, string>()
                {
                    { 0,"零"},
                    { 1,"一"},
                    { 2,"二"},
                    { 3,"三"},
                    { 4,"四"},
                    { 5,"五"},
                    { 6,"六"},
                    { 7,"七"},
                    { 8,"八"},
                    { 9,"九"},
                };
        static IDictionary<int, string> ToChineseWeight = new Dictionary<int, string>()
                {
                    { 1,""},
                    { 2,"十"},
                    { 3,"百"},
                    { 4,"千"},
                    { 5,"万"},
                    { 6,"十万"},
                    { 7,"百万"},
                    { 8,"千万"},
                    { 9,"亿"},
                    { 10,"十亿"},
                };
        static public string GetChineseDigit(int digit)
        {

            string digitText = string.Empty;
            while (digit != 0)
            {
                int num = digit % 10;
                string str = string.Empty;
                //if (ToChineseDigit.TryGetValue(num,out str))
                {
                    digitText = ToChineseDigit[num] + digitText;
                }
                digit /= 10;
            }
            return digitText;
        }

        static public string GetChineseNumber(int num)
        {
            int weightNum = 0;
            int t = num;
            string NumText = string.Empty;
            while (t != 0)
            {
                t /= 10;
                weightNum++;
            }
            int tempNum = num;
            for (int i = 1; i <= weightNum && tempNum != 0; i++)
            {
                int digit = tempNum % 10;
                string str = string.Empty;
                if (digit == 0 && i == 1)
                {
                    tempNum /= 10;
                    continue;
                }


                if (ToChineseDigit.TryGetValue(digit, out str))
                {
                    if (digit == 0)
                    {
                        NumText = string.Format("{0}{1}", str, NumText);
                        tempNum /= 10;
                        continue;
                    }
                    string str2 = string.Empty;
                    if (ToChineseWeight.TryGetValue(i, out str2))
                        NumText = string.Format("{0}{1}{2}", str, str2, NumText);
                }
                tempNum /= 10;
            }
            return NumText;
        }

        static public string GetTimeText(DateTime time)
        {            
            var year = GetChineseDigit(time.Year);
            var month = GetChineseNumber(time.Month);
            var day = GetChineseNumber(time.Day);
            return string.Format("{0}年{1}月{2}日", year,month,day);            
        }
    }
}
