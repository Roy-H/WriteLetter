using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Helper
{
    public class DataUpdateHelper
    {
        private static DataUpdateHelper instance;
        public static DataUpdateHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataUpdateHelper();
                }
                return instance;
                //DataViewModel
            }
        }
        //public object
    }
}
