using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DogPensionWpfApp.Commands
{
    public class MainNavigateCommand : ICommand
    {
        private readonly Navigator navigator;

        public MainNavigateCommand(Navigator navigator)
        {
            this.navigator = navigator;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is RegisteredViewModel)
            {
                navigator.SetViewModelOnMain((RegisteredViewModel)parameter);
            }
        }
    }
}
