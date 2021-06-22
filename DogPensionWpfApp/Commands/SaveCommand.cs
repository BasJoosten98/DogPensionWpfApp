using DogPensionWpfApp.Models.Wrappers;
using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Commands
{
    public class SaveCommand<T> : AsyncCommandBase 
        where T : WrapperModelBase
    {
        private readonly ICRUD<T> crud;
        private readonly Func<WrapperBase<T>> getModelWrapper;
        private readonly Func<object> onSuccess;
        private readonly Action<List<ValidationResult>> onFailure;

        public SaveCommand(ICRUD<T> crud, Func<WrapperBase<T>> modelWrapper, Func<object> OnSuccess, Action<List<ValidationResult>> OnFailure)
        {
            this.crud = crud;
            this.getModelWrapper = modelWrapper;
            onSuccess = OnSuccess;
            onFailure = OnFailure;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (getModelWrapper() == null)
            {
                results.Add(new ValidationResult("No model has been loaded yet"));
                onFailure(results);
                return;
            }
            results = getModelWrapper().GetValidationResults();
            if (!getModelWrapper().HasChanges) { results.Add(new ValidationResult("No changes have been made")); }
            if (results.Count > 0)
            {
                onFailure(results);
                return;
            }
            try
            {
                await crud.CreateOrUpdate(getModelWrapper().Model);
            }
            catch(Exception ex)
            {
                results.Add(new ValidationResult("A Exception happenned: " + ex.Message));
                onFailure(results);
                return;
            }
            onSuccess();
        }
    }
}
