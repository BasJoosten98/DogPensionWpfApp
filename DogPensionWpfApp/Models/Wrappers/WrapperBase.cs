using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DogPensionWpfApp.Models.Wrappers
{
    public class WrapperBase<T> : INotifyPropertyChanged
        where T : WrapperModelBase
    {
        private readonly T model;

        private bool hasChanges;

        public bool HasChanges
        {
            get { return hasChanges; }
            set { hasChanges = value; }
        }

        public T Model => model;

        public WrapperBase(T model)
        {
            this.model = model;
            hasChanges = (model.Id <= 0);
        }

        public virtual List<ValidationResult> GetValidationResults()
        {
            var ctx = new ValidationContext(model);

            // A list to hold the validation result.
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, ctx, results, true))
            {
                //has errors
            }
            return results;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            hasChanges = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
