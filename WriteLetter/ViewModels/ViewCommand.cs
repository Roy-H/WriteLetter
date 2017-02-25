using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WriteLetter.ViewModels
{
    class ViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Func<object, bool> CanExecuteCallback { get; set; }
        public Action<object> ExecuteCallback { get; set; }
        
        public bool CanExecute(object parameter)
        {
            if (CanExecuteCallback != null)
            {
                return CanExecuteCallback(parameter);
            }
            return true;
        }

        public void Execute(object parameter)
        {

            try
            {
                if (ExecuteCallback != null)
                {
                    ExecuteCallback(parameter);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
