using System;
using System.Windows.Input;

namespace ExampleForGoldenMaster.ViewModel.Command
{
    class CommonCommand : ICommand
    {
        public Action<object> action;
        public Func<object, bool> func;

        public CommonCommand(Action<object> action, Func<object, bool> func = null)
        {
            this.action = action;
            this.func = func;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return action != null || func != null;
        }

        public void Execute(object parameter)
        {
            action.Invoke(parameter);
        }
    }
}
