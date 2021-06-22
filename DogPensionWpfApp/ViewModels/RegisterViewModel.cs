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
    public class RegisterViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(_email)); }
        }
        private SecureString _password;

        public SecureString Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(_password)); }
        }

        private SecureString _comfimedPassword;

        public SecureString ComfirmedPassword
        {
            get { return _comfimedPassword; }
            set { _comfimedPassword = value; OnPropertyChanged(nameof(_comfimedPassword)); }
        }

        private string errors;
        public string Errors
        {
            get { return errors; }
            set { errors = value; OnPropertyChanged(nameof(Errors)); OnPropertyChanged(nameof(HasErrors)); }
        }
        public bool HasErrors => !string.IsNullOrEmpty(Errors);


        public ICommand RegisterCommand { get; }
        public ICommand NavigateLoginCommand { get; }

        public RegisterViewModel(Authenticator auth, Navigator nav)
        {
            RegisterCommand = new RegisterCommand(this, auth, displayErrors);
            NavigateLoginCommand = new RelayCommand(o => nav.SetViewModel(this, RegisteredViewModel.Login, o));
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
