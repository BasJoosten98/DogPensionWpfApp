using DogPensionWpfApp.Models.Wrappers;
using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Commands
{
    public class DeleteCommand<T> : AsyncCommandBase
        where T : WrapperModelBase
    {
        private readonly ICRUD<T> crud;
        private readonly Func<WrapperBase<T>> getModelWrapper;
        private readonly Func<object> onSuccess;
        private readonly Action<List<ValidationResult>> onFailure;

        public DeleteCommand(ICRUD<T> crud, Func<WrapperBase<T>> modelWrapper, Func<object> OnSuccess, Action<List<ValidationResult>> OnFailure)
        {
            this.crud = crud;
            this.getModelWrapper = modelWrapper;
            onSuccess = OnSuccess;
            onFailure = OnFailure;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            WrapperBase<T> model = getModelWrapper();
            if(model == null)
            {
                results.Add(new ValidationResult("No model has been loaded yet"));
                onFailure(results);
                return;
            }
            else
            {
                if (model.Model.Id > 0)
                {
                    try
                    {
                        await crud.Remove(getModelWrapper().Model);
                    }
                    catch (Exception ex)
                    {
                        results.Add(new ValidationResult("A Exception happenned: " + ex.Message));
                        onFailure(results);
                        return;
                    }
                    onSuccess();
                }
                else
                {
                    results.Add(new ValidationResult("This model can not be deleted (invalid id)"));
                    onFailure(results);
                    return;
                }
            }
        }
    }
}
