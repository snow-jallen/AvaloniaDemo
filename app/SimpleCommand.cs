using System;
using System.Windows.Input;

namespace Demo
{
    public class SimpleCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;

        public SimpleCommand(Action execute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public SimpleCommand(Func<bool> canExecute, Action execute)
        {
            this.canExecute = canExecute;
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if(canExecute != null)
                return canExecute();
            return true;
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}