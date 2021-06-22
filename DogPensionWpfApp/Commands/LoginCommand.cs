using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services;
using DogPensionWpfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel loginViewModel;
        private readonly Authenticator authenticator;
        private readonly Navigator navigator;
        private readonly Action<List<ValidationResult>> onFailure;

        public LoginCommand(LoginViewModel loginViewModel,
            Authenticator auth,
            Navigator nav,
            Action<List<ValidationResult>> OnFailure)
        {
            this.loginViewModel = loginViewModel;
            this.authenticator = auth;
            this.navigator = nav;
            onFailure = OnFailure;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                bool loggedIn = await authenticator.Login(
                        loginViewModel.Name,
                        loginViewModel.Password);
                if (loggedIn)
                {
                    navigator.SetViewModelOnMain(null);
                }
            }
            catch (Exception ex)
            {
                List<ValidationResult> results = new List<ValidationResult>();
                results.Add(new ValidationResult(ex.Message));
                onFailure(results);
            }
        }
    }
}
