using System;
//using System.Windows.Input;

namespace CollisionEditor.viewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action action;
        public RelayCommand(Action action) => this.action = action;
        public bool CanExecute(object parametr) => true;
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter) => action();
    }
}
