using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightCenterDBGenerator
{
    public class Command : ICommand
    {
        private Action<object> ExecMethod;
        private Func<object, bool> CanExecMethod;

        public Command(Action<object> execMethod, Func<object, bool> canExecMethod)
        {
            ExecMethod = execMethod;
            CanExecMethod = canExecMethod;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return CanExecMethod(new object());
        }

        public void Execute(object parameter)
        {
            ExecMethod(new object());
        }
    }
}
