using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteLetter.Helper
{
    
    public static class DateToChineseHelper
    {
        static IDictionary<int, string> ToChineseDigit;
        static IDictionary<int, string> ToChineseWeight;
        static public string GetChineseDigit(int digit)
        {
            if (ToChineseDigit == null)
            {
                ToChineseDigit = new Dictionary<int, string>()
                {
                    { 0,"零"},
                    { 1,"一"},
                    { 2,"二"},
                    { 3,"三"},
                    { 4,"四"},
                    { 5,"五"},
                    { 6,"六"},
                    { 7,"七"},
                    { 8,"把"},
                    { 9,"九"},
                };
            }
         
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
            if(ToChineseDigit == null)
              ToChineseDigit = new Dictionary<int, string>()
            {
                { 0,"零"},
                { 1,"一"},
                { 2,"二"},
                { 3,"三"},
                { 4,"四"},
                { 5,"五"},
                { 6,"六"},
                { 7,"七"},
                { 8,"把"},
                { 9,"九"},
            };
            if(ToChineseWeight == null)
                ToChineseWeight = new Dictionary<int, string>()
                {
                    { 1,""},
                    { 2,"十"},
                    { 3,"百"},
                    { 3,"千"},
                    { 4,"万"},
                    { 5,"十万"},
                    { 6,"百万"},
                    { 7,"千万"},
                    { 8,"亿"},
                    { 9,"十亿"},
                };
            int weightNum = 0;
            int t = num;
            string NumText = string.Empty;
            while (t != 0)
            {
                t /= 10;
                weightNum++;
            }
            for (int i = 0; i < weightNum; i++)
            {
                int digit = num % 10;
                string str = string.Empty;
                if (ToChineseDigit.TryGetValue(digit, out str))
                {
                    string str2 = string.Empty;
                    if (ToChineseWeight.TryGetValue(i, out str2))
                        NumText = str + str2 + NumText;
                }
            }
            return NumText;
        }
    }
}
