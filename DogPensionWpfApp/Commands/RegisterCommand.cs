using DogPensionWpfApp.Extensions;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services;
using DogPensionWpfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DogPensionWpfApp.Commands
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly RegisterViewModel registerViewModel;
        private readonly Authenticator authenticator;
        private readonly Action<List<ValidationResult>> onFailure;

        public RegisterCommand(RegisterViewModel registerViewModel, 
            Authenticator authenticator,
            Action<List<ValidationResult>> OnFailure)
        {
            this.registerViewModel = registerViewModel;
            this.authenticator = authenticator;
            onFailure = OnFailure;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                bool result = await authenticator.Register(
                    registerViewModel.Name,
                    registerViewModel.Email,
                    registerViewModel.Password,
                    registerViewModel.ComfirmedPassword);
            }
            catch(Exception ex)
            {
                List<ValidationResult> results = new List<ValidationResult>();
                results.Add(new ValidationResult(ex.Message));
                onFailure(results);
            }
        }
    }
}
