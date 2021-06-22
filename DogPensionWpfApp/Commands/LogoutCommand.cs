using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DogPensionWpfApp.Commands
{
    public class LogoutCommand : ICommand
    {
        private readonly Authenticator authenticator;
        private readonly Navigator navigator;

        public LogoutCommand(Authenticator auth, Navigator nav)
        {
            this.authenticator = auth;
            this.navigator = nav;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            authenticator.Logout();
            navigator.SetViewModelOnMain(RegisteredViewModel.Login);
        }

    }
}
