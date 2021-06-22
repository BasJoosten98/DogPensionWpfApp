using DogPensionWpfApp.Commands;
using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Text;
using System.Windows.Input;

namespace DogPensionWpfApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private SecureString _password;

        public SecureString Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(_password)); }
        }

        private string errors;
        public string Errors
        {
            get { return errors; }
            set { errors = value; OnPropertyChanged(nameof(Errors)); OnPropertyChanged(nameof(HasErrors)); }
        }
        public bool HasErrors => !string.IsNullOrEmpty(Errors);

        public ICommand LoginCommand { get; }

        public ICommand NavigateRegisterCommand { get; }

        public LoginViewModel(Authenticator auth, Navigator nav)
        {
            LoginCommand = new LoginCommand(this, auth, nav, displayErrors);
            NavigateRegisterCommand = new RelayCommand(o => nav.SetViewModel(this, RegisteredViewModel.Register, o));
        }

        private void displayErrors(List<ValidationResult> results)
        {
            string errors = "";
            foreach (ValidationResult result in results)
            {
                if (string.IsNullOrEmpty(errors)) { errors = result.ErrorMessage; }
                else { errors += "\n" + result.ErrorMessage; }
            }
            Errors = errors;
        }

        public override void Load(object arguments)
        {
            //No loading needed
        }
    }
}
